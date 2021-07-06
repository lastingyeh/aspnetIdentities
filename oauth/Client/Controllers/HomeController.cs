using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Secret()
        {
            // check secret from server
            var serverResponse = await AccessTokenRefreshWrapper(
                () => SecuredGetRequest("https://localhost:5000/secret/index"));

            // check secret from api

            var apiResponse = await AccessTokenRefreshWrapper(
                () => SecuredGetRequest("https://localhost:5002/secret/index"));

            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretPolicy()
        {
            return View();
        }

        private async Task<HttpResponseMessage> SecuredGetRequest(string url)
        {
            var client = _httpClientFactory.CreateClient();
            var token = await HttpContext.GetTokenAsync("access_token");

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            return await client.GetAsync(url);
        }

        private async Task<HttpResponseMessage> AccessTokenRefreshWrapper(
            Func<Task<HttpResponseMessage>> initialRequest)
        {
            var response = await initialRequest();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshAccessToken();

                response = await initialRequest();
            }

            return response;
        }

        private async Task RefreshAccessToken()
        {
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var requestData = new Dictionary<string, string>
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
            };

            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:5000/oauth/token")
            {
                Content = new FormUrlEncodedContent(requestData)
            };

            var basicCredentials = "username:password";
            var encodedCredentials = Encoding.UTF8.GetBytes(basicCredentials);
            var base64Credentials = Convert.ToBase64String(encodedCredentials);

            request.Headers.Add("Authorization", $"Basic {base64Credentials}");

            var response = await _httpClientFactory.CreateClient().SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseStream = await response.Content.ReadAsStreamAsync();
                var responseData = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(responseStream);

                var newAccessToken = responseData.GetValueOrDefault("access_token");
                var newRefreshToken = responseData.GetValueOrDefault("refresh_token");

                var authInfo = await HttpContext.AuthenticateAsync("ClientCookie");

                authInfo.Properties.UpdateTokenValue("access_token", newAccessToken);
                authInfo.Properties.UpdateTokenValue("refresh_token", newRefreshToken);

                await HttpContext.SignInAsync("ClientCookie", authInfo.Principal, authInfo.Properties);
            }
        }
    }
}