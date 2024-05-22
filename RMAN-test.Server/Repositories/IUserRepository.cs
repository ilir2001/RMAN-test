using Microsoft.AspNetCore.Identity;
using RMAN_test.Server.Models;
using System.Security.Claims;

namespace RMAN_test.Server.Repositories
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task SignOutAsync();
        Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure);
        bool IsUserAuthenticated(ClaimsPrincipal user);
        string GetUserName(ClaimsPrincipal user);
        IEnumerable<ApplicationUser> GetAllUsers();
    }
}
