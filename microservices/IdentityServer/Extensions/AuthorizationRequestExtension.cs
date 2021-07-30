using System;
using IdentityServer4.Models;

namespace IdentityServer.Extensions
{
    public static class AuthorizationRequestExtension
    {
        // <summary>
        /// Checks if the redirect URI is for a native client.
        /// </summary>
        /// <returns></returns>
        public static bool IsNativeClient(this AuthorizationRequest context)
        {
            return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
                && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
        }
    }
}