using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BlazorGoldenZebra.Data
{
    public static class UserExtentions
    {
        public static async Task<IList<string>> GetRoles(this ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            var roles = await userManager.GetRolesAsync(user);
            return roles;
        }
    }
}
