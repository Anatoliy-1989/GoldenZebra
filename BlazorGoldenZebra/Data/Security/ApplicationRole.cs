using Microsoft.AspNetCore.Identity;

namespace BlazorGoldenZebra.Data
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }

        public ApplicationRole(string roleName)
            : base(roleName) { }
    }
}
