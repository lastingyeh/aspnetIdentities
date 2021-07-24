using System;

namespace CustomIdentityServer.UserServices
{
    public class CustomUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
