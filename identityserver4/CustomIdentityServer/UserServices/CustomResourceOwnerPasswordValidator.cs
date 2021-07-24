using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;

namespace CustomIdentityServer.UserServices
{
    public class CustomResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;
        public CustomResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_userRepository.ValidateCredentials(context.UserName, context.Password))
            {
                var user = _userRepository.FindByUsername(context.UserName);

                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            }

            return Task.CompletedTask;
        }
    }
}
