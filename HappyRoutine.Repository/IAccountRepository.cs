using HappyRoutine.Models.Account;
using Microsoft.AspNetCore.Identity;

namespace HappyRoutine.Repository
{
    public interface IAccountRepository
    {
        public Task<IdentityResult> CreateAsync(ApplicationUserIdentity user,
            CancellationToken cancellationToken);

        public Task<ApplicationUserIdentity> GetByUsernameAsync(string normalizedUsername,
            CancellationToken cancellationToken);
    }
}