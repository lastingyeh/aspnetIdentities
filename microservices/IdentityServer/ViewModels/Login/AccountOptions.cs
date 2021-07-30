using System;

namespace IdentityServer.ViewModels.Login
{
    public class AccountOptions
    {
        public static bool AllowLocalLogin = true;
        public static bool AllowRememberLogin = true;
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);
        public static bool ShowLogoutPrompt = true;
        public static bool AutomaticRedirectAfterSignOut = true; // set true if you want to auto redirect
        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}