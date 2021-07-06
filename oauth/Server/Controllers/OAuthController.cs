using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    public class OAuthController : Controller
    {
        private readonly GenTokenService _tokenService;
        public OAuthController(GenTokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpGet]
        public IActionResult Authorize(
            string response_code, // authorization flow type
            string client_id, // client id
            string redirect_uri,
            string scope, // what info want, ex: email, cookie, tel
            string state) // random string generated to confirm that back to verifty the same client
        {
            var query = new QueryBuilder();

            query.Add("redirectUri", redirect_uri);
            query.Add("state", state);

            return View(model: query.ToString());
        }

        [HttpPost]
        public IActionResult Authorize(string username, string redirectUri, string state)
        {
            const string code = "servercode";

            var query = new QueryBuilder();

            query.Add("code", code);
            query.Add("state", state);

            return Redirect($"{redirectUri}{query.ToString()}");
        }

        // regular parameters
        // grant_type, code, redirect_uri, client_id
        public IActionResult Token(
            string grant_type, // access_token request flow
            string code, // confirmation of the authentication process
            string redirect_uri,
            string client_id,
            string refresh_token)
        {
            var access_token = _tokenService.TokenGenerator(refresh_token);
            var refreshToken = _tokenService.RefreshTokenGenerator();

            var responseObject = new
            {
                access_token,
                token_type = "Bearer",
                raw_claim = "oauthTutorial",
                refresh_token = refreshToken,
            };

            return Ok(responseObject);
        }

        // auth jwt validation from API
        [Authorize]
        public IActionResult Validate()
        {
            if (HttpContext.Request.Query.TryGetValue("access_token", out var _))
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
