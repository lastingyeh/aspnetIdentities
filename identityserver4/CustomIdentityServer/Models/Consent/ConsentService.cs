using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.Logging;

namespace CustomIdentityServer.Models.Consent
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly ILogger<ConsentService> _logger;
        public ConsentService(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            ILogger<ConsentService> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _logger = logger;
        }

        public async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);

            if (request != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(request.Client.ClientId);

                if (client != null)
                {
                    var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.RequestObjectValues.Select(x => x.Key));

                    if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
                    {
                        return CreateConsentViewModel(model, returnUrl, request, client, resources);
                    }
                    else
                    {
                        _logger.LogError("No scopes matching: {0}", request.RequestObjectValues.Select(x => x.Key));
                    }
                }
                else
                {
                    _logger.LogError("Invalid client id: {0}", request.Client.ClientId);
                }
            }
            else
            {
                _logger.LogError("No consent request matching request: {0}", returnUrl);
            }

            return null;
        }

        private ConsentViewModel CreateConsentViewModel(
            ConsentInputModel model,
            string returnUrl,
            AuthorizationRequest request,
            Client client, IdentityServer4.Models.Resources resources)
        {
            var vm = new ConsentViewModel
            {
                RememberConsent = model?.RememberConsent ?? true,
                ScopesConsented = model?.ScopesConsented ?? Enumerable.Empty<string>(),

                ReturnUrl = returnUrl,

                ClientName = client.ClientName,
                ClientUrl = client.ClientUri,
                ClientLogoUrl = client.LogoUri,
                AllowRememberConsent = client.AllowRememberConsent
            };

            vm.IdentityScopes = resources.IdentityResources.Select(x =>
                CreateScopeViewModel(x, vm.ScopesConsented.Contains(x.Name) || model == null)).ToArray();

            vm.APIResourceScopes = resources.ApiResources.Select(x =>
                CreateScopeViewModel(x, vm.ScopesConsented.Any(sc => x.Scopes.Contains(sc)) || model == null)).ToArray();

            if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
            {
                vm.APIResourceScopes = vm.APIResourceScopes.Union(new ScopeViewModel[]
                    {
                        GetOfflineAccessScope(vm.ScopesConsented.Contains(IdentityServerConstants.StandardScopes.OfflineAccess) || model == null)
                    });
            }

            return vm;
        }

        public ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required,
            };
        }

        public ScopeViewModel CreateScopeViewModel(ApiResource api, bool check)
        {
            return new ScopeViewModel
            {
                Name = api.Name,
                DisplayName = api.DisplayName,
                Description = api.Description,
                Emphasize = false,
                Required = false,
                Checked = check,
            };
        }

        private ScopeViewModel GetOfflineAccessScope(bool check)
        {
            return new ScopeViewModel
            {
                Name = IdentityServerConstants.StandardScopes.OfflineAccess,
                DisplayName = ConsentOptions.OfflineAccessDisplayName,
                Description = ConsentOptions.OfflineAccessDescription,
                Emphasize = true,
                Checked = check
            };
        }
    }
}
