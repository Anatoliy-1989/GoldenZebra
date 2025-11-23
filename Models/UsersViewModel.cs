using BlazorGoldenZebra.Data;

namespace BlazorGoldenZebra.Models
{
    public class UsersViewModel : ApplicationUser
    {
        public ApplicationUser User { get; set; }
        public IList<string> Roles { get; set; }

        public UsersViewModel(ApplicationUser user)
        {
            this.User = user;

        }
    }
}
