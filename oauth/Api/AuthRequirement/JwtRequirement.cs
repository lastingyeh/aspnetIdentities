using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Api.AuthRequirement
{
    public class JwtRequirement : IAuthorizationRequirement { }

    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly HttpContext _httpContext;
        private readonly HttpClient _client;
        private readonly IConfiguration _config;
        public JwtRequirementHandler(
        IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration config)
        {
            _config = config;
            _httpContext = httpContextAccessor.HttpContext;
            _client = httpClientFactory.CreateClient();
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, JwtRequirement requirement)
        {
            if (_httpContext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];
                var authServerHost = _config["AuthSeverHost"];

                var response = await _client.GetAsync($"{authServerHost}/oauth/validate?access_token={accessToken}");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}