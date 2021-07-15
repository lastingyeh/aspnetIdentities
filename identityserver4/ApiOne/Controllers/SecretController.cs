using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiOne.Controllers
{
    public class SecretController : Controller
    {
        [Route("/secret/crm")]
        [Authorize(Policy = "CRM")]
        public string Crm()
        {
            var claims = User.Claims.ToList();

            return "crm message from ApiOne";
        }

        [Route("/secret/sap")]
        [Authorize(Policy = "SAP")]
        public string Sap()
        {
            var claims = User.Claims.ToList();

            return "sap message from ApiOne";
        }
    }
}
