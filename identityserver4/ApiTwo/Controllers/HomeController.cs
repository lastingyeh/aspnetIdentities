using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;

namespace ApiTwo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            // retrieve access_token
            var serverClient = _httpClientFactory.CreateClient("identityServer");

            // var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync(
            //     new DiscoveryDocumentRequest { Policy = new DiscoveryPolicy { RequireHttps = false } });
            var discoveryDocument = await serverClient.GetDiscoveryDocumentAsync();
                
            var tokenResponse = await serverClient.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest
                {
                    Address = discoveryDocument.TokenEndpoint,

                    ClientId = "client_id",
                    ClientSecret = "client_secret",

                    // Scope = "Ebiz",
                });

            var accessToken = tokenResponse.AccessToken;

            // retrieve secret data
            var apiClient = _httpClientFactory.CreateClient("Ebiz");

            apiClient.SetBearerToken(accessToken);

            var response = await apiClient.GetAsync("/secret");
            var content = await response.Content.ReadAsStringAsync();

            return Ok(new
            {
                access_token = accessToken,
                message = content,
            });
        }
    }
}