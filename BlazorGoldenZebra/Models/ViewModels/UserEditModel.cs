namespace BlazorGoldenZebra.Models.ViewModels
{
    public class UserEditModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public bool IsLocked { get; set; }
        public List<UserRoleModel> UserRoles { get; set; } = new List<UserRoleModel>();
    }
}
