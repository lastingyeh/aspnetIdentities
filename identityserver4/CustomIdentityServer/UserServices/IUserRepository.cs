using System;
using System.Threading.Tasks;

namespace CustomIdentityServer.UserServices
{
    public interface IUserRepository
    {
        bool ValidateCredentials(string username, string password);

        Task<CustomUser> FindBySubjectId(Guid Id);

        CustomUser FindByUsername(string username);
    }
}
