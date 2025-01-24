using NuoroLight.Domain.Entities.User;

namespace NuoroLight.Domain.ViewModels.Account;
public class UserInformationViewModel
{
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public string Mobile { get; set; }
    public string SitrUrl { get; set; }
    public Gender Gender { get; set; }

    public DateTime RegisterDate { get; set; }
}

