using System.Threading.Tasks;
using CustomIdentityServer.Models.Consent;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentityServer.Controllers
{
    public class ConsentController : Controller
    {
        private readonly ConsentService _consentService;
        public ConsentController(ConsentService consentService)
        {
            _consentService = consentService;
        }

        /// <summary>
        /// Shows the consent screen
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await _consentService.BuildViewModelAsync(returnUrl);
            
            if (vm != null)
            {
                return View("Index", vm);
            }

            return View("Error");
        }
    }
}
