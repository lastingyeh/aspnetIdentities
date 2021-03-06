using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;

namespace CustomIdentityServer.UserServices
{
    public class CustomProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CustomProfileService> _logger;
        public CustomProfileService(IUserRepository userRepository, ILogger<CustomProfileService> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = Guid.Parse(context.Subject.GetSubjectId());

            _logger.LogDebug("Get profile called for subject {subject} from client {client} with claim types {claimTypes} via {caller}",
                context.Subject.GetSubjectId(),
                context.Client.ClientName ?? context.Client.ClientId,
                context.RequestedClaimTypes,
                context.Caller);

            var user = await _userRepository.FindBySubjectId(sub);
            var claims = new List<Claim>
            {
                new Claim("role", "dataEventRecords.admin"),
                new Claim("role", "dataEventRecords.user"),
                new Claim("username", user.UserName),
                new Claim("email", user.Email),
            };

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = Guid.Parse(context.Subject.GetSubjectId());
            var user = await _userRepository.FindBySubjectId(sub);

            context.IsActive = user != null;
        }
    }
}
