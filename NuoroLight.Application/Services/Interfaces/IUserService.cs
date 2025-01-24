using Microsoft.AspNetCore.Http;
using NuoroLight.Application.ViewModels.Account;
using NuoroLight.Domain.Entities.User;
using NuoroLight.Domain.ViewModels.Account;
using NuoroLight.Domain.ViewModels.User;

namespace NuoroLight.Application.Services.Interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        Task<RegisterResult> RegisterUser(RegisterViewModel registerUser);
        Task<User> GetUserByEmail(string email);
        Task<bool> ActiveUserByActiveCode(string activeCode);
        Task<LoginResult> LoginUser(LoginViewModel login);
        Task<ForgotPassResult> ForgotPassWordUser(ForgotPassViewModel forgotPass);
        Task<bool> IsExistsUserByActiveCode(string activeCode);
        Task<ResetPsssWordResult> ResetPsssWordUser(ResetPsssWordViewModel resetPsssWord);
        Task<ChangePasswordResult> ChangePassWord(ChangePasswordViewModel changePassword, int userId);
        Task<UserInformationViewModel> GetUserInformation(int userId);
        Task<EditUserProfileViewModel> GetUserInfoForEdit(int userId);
        Task<EditUserProfileResult> EditProfile(EditUserProfileViewModel editUserProfile, IFormFile UserAvatar, int userId);
        Task<FilterUserViewModel> FilterUser(FilterUserViewModel filterUser);
        Task<bool> DeleteUser(int userId);
        Task<bool> RestoreUser(int userId);
        Task<CreateUserResult> CreateUser(CreateUserViewModel createUser, List<int> SelectedPermission, IFormFile userAvatar);
        Task<EditUserForAdminViewModel> UserInfoForEdit(int userId);
        Task<EditUserResult> EditUserForAdmin(EditUserForAdminViewModel editUser, List<int> SelectedPermission, IFormFile userAvatar);
        Task<User> GetUserByUserId(int userId);
        Task<UserDetialViewModel> GetUserDetialByUserId(int userId);

    }
}
