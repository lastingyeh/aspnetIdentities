using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Server.Services
{
    public class GenTokenService
    {
        private readonly TokenSettings _tokenSettings;
        public GenTokenService(IOptions<TokenSettings> options)
        {
            _tokenSettings = options.Value;
        }
        public string TokenGenerator(string refresh_token = null)
        {
            var claims = new[] {
            new Claim (ClaimTypes.DateOfBirth, "2020/01/01"),
            new Claim (JwtRegisteredClaimNames.Sub, "some_id"),
            new Claim ("granny", "cookie"),
            };

            var secretBytes = Encoding.UTF8.GetBytes(_tokenSettings.Secret);
            var key = new SymmetricSecurityKey(secretBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;
            var signinCredentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                _tokenSettings.Issuer,
                _tokenSettings.Audiance,
                claims,
                notBefore: DateTime.Now,
                expires: !string.IsNullOrEmpty(refresh_token) ?
                DateTime.Now.AddMinutes(5) : DateTime.Now.AddMilliseconds(15),
                signinCredentials);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenJson;
        }

        public string RefreshTokenGenerator()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}