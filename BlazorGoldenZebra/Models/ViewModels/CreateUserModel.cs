using System.ComponentModel.DataAnnotations;

namespace BlazorGoldenZebra.Models.ViewModels
{
    public class CreateUserModel
    {
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public List<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();
    }
}
