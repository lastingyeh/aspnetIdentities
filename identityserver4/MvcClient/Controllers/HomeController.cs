using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using IdentityModel.Client;

namespace MvcClient.Controllers
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
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            var claims = User.Claims;

            var result = await GetSecret(accessToken);
            
            await RefreshAccessToken();

            ViewBag.result = result;

            return View();
        }

        [Authorize]
        public IActionResult Logout()
        {
            // remove authentication includes all
            return SignOut("Cookie", "oidc");
        }

        private async Task<string> GetSecret(string accessToken)
        {
            var apiClient = _httpClientFactory.CreateClient("apiOne");

            apiClient.SetBearerToken(accessToken);

            var sapResponse = await apiClient.GetAsync("/secret/sap");
            var crmResponse = await apiClient.GetAsync("/secret/crm");

            var sapContent = await sapResponse.Content.ReadAsStringAsync();
            var crmContent = await crmResponse.Content.ReadAsStringAsync();

            return sapContent;
        }

        private async Task RefreshAccessToken()
        {
            var refreshTokenClient = _httpClientFactory.CreateClient("identityServer");
            var discoveryDocument = await refreshTokenClient.GetDiscoveryDocumentAsync();

            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var tokenResponse = await refreshTokenClient.RequestRefreshTokenAsync(
                new RefreshTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,
                    RefreshToken = refreshToken,
                    ClientId = "client_id_mvc",
                    ClientSecret = "client_secret_mvc",
                });

            // test to check token diff
            await TokenVerify(tokenResponse);

            var authInfo = await HttpContext.AuthenticateAsync("Cookie");

            authInfo.Properties.UpdateTokenValue("access_token", tokenResponse.AccessToken);
            authInfo.Properties.UpdateTokenValue("refresh_token", tokenResponse.RefreshToken);

            await HttpContext.SignInAsync("Cookie", authInfo.Principal, authInfo.Properties);
        }

        private async Task TokenVerify(TokenResponse response)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var accessTokenDiff = !response.AccessToken.Equals(accessToken);
            var idTokenDiff = !response.IdentityToken.Equals(idToken);
            var refreshTokenDiff = !response.RefreshToken.Equals(refreshToken);
        }
    }
}
