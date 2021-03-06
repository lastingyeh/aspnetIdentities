using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomIdentityServer.UserServices
{
    public class UserRepository : IUserRepository
    {
        private readonly List<CustomUser> _users = new List<CustomUser>
        {
            new CustomUser
            {
                Id = Guid.NewGuid(),
                UserName = "damienbod",
                Password = "damienbod",
                Email = "damienbod@email.ch"
            },
            new CustomUser
            {
                Id = Guid.NewGuid(),
                UserName = "raphael",
                Password = "raphael",
                Email = "raphael@email.ch"
            },
        };
        public Task<CustomUser> FindBySubjectId(Guid id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);

            return Task.FromResult(user);
        }

        public CustomUser FindByUsername(string username)
        {
            return _users.FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = FindByUsername(username);

            return user?.Password.Equals(password) ?? false;
        }
    }
}
