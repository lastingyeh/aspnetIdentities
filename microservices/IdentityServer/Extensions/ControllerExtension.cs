using IdentityServer.ViewModels.Login;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Extensions
{
    public static class ControllerExtension
    {
        public static IActionResult LoadingPage(this Controller controller, string viewName, string redirectUri)
        {
            controller.HttpContext.Response.StatusCode = 200;
            controller.HttpContext.Response.Headers["Location"] = "";

            return controller.View(viewName, new RedirectViewModel { RedirectUrl = redirectUri });
        }
    }
}