using Microsoft.AspNetCore.Http;
using NuoroLight.Application.Convertors;
using NuoroLight.Application.Extensions;
using NuoroLight.Application.Security;
using NuoroLight.Application.Services.Interfaces;
using NuoroLight.Domain.Entities.User;
using NuoroLight.Domain.ViewModels.Account;
using NuoroLight.Domain.ViewModels.User;
using NuoroLight.Application.Generators;
using NuoroLight.Application.ViewModels.Account;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Application.Security.PassWordHashing;
namespace NuoroLight.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region constructor
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IUserPermissionRepository _userPermissionRepository;
        public UserService(IUserRepository userRepository, IPasswordHelper passwordHelper, IUserPermissionRepository userPermissionRepository)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _userPermissionRepository = userPermissionRepository;
        }
        #endregion

        #region DisposeAsync
        public async ValueTask DisposeAsync()
        {
            await _userRepository.DisposeAsync();
        }
        #endregion

        #region RegisterUser
        public async Task<RegisterResult> RegisterUser(RegisterViewModel registerUser)
        {
            if (await _userRepository.IsExistsUserByEmail(FixedText.FixEmail(registerUser.Email)))
                return RegisterResult.ExistsEmail;
            if (await _userRepository.IsExistsUserByUserName(registerUser.UserName))
                return RegisterResult.ExistUserName;
            if (await _userRepository.IsExistsUserByMobile(registerUser.Mobile))
                return RegisterResult.ExistMobile;
            var user = new User()
            {
                UserName = registerUser.UserName.SanitizeText(),
                Mobile = registerUser.Mobile.SanitizeText(),
                Email = registerUser.Email.SanitizeText(),
                Password = _passwordHelper.EncodePasswordSha512(registerUser.PassWord.SanitizeText()),
                ActiveCode = NuoroLight.Application.Generators.Generators.GeneratorsUniqueCode(),
                Gender = Gender.Unknown
            };
            try
            {
                await _userRepository.AddUser(user);
                await _userRepository.Save();
                return RegisterResult.Success;
            }
            catch
            {
                return RegisterResult.Failed;
            }
        }
        #endregion

        #region GetUserByEmail
        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }
        #endregion

        #region ActiveUserByActiveCode
        public async Task<bool> ActiveUserByActiveCode(string activeCode)
        {
            var user = await _userRepository.GetUserByActiveCode(activeCode);
            if (user == null || user.IsActive) return false;
            if (string.IsNullOrEmpty(activeCode)) return false;
            try
            {
                user.IsActive = true;
                user.ActiveCode = Application.Generators.Generators.GeneratorsUniqueCode();
                _userRepository.EditUser(user);
                await _userRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region LoginUser
        public async Task<LoginResult> LoginUser(LoginViewModel login)
        {
            if (!await _userRepository.IsExistsUserByEmail(FixedText.FixEmail(login.Email.SanitizeText()))) return LoginResult.NotFound;
            var user = await _userRepository.GetUserByEmail(login.Email.SanitizeText());
            if (!user.IsActive) return LoginResult.NotActive;
            if (user.IsDelete) return LoginResult.DeletedAccount;
            if (user.IsBlock) return LoginResult.UserIsBlock;
            if (user.Password != _passwordHelper.EncodePasswordSha512(login.PassWord.SanitizeText()))
                return LoginResult.NotFound;
            return LoginResult.Success;
        }
        #endregion

        #region ForgotPassWordUser
        public async Task<ForgotPassResult> ForgotPassWordUser(ForgotPassViewModel forgotPass)
        {
            var user = await _userRepository.GetUserByEmail(FixedText.FixEmail(forgotPass.Email.SanitizeText()));
            if (user == null) return ForgotPassResult.NotFount;
            if (user.IsBlock) return ForgotPassResult.UserIsBlock;
            if (!user.IsActive) return ForgotPassResult.NotActive;
            if (user.IsDelete) return ForgotPassResult.UserIsDeleted;
            return ForgotPassResult.FindUser;
        }
        #endregion

        #region IsExistsUserByActiveCode
        public async Task<bool> IsExistsUserByActiveCode(string activeCode)
        {
            return await _userRepository.IsExistsUserByActiveCode(activeCode);
        }
        #endregion

        #region ResetPsssWordUser
        public async Task<ResetPsssWordResult> ResetPsssWordUser(ResetPsssWordViewModel resetPsssWord)
        {
            var user = await _userRepository.GetUserByActiveCode(resetPsssWord.ActiveCode);
            if (user == null) return ResetPsssWordResult.NotFound;

            user.Password = _passwordHelper.EncodePasswordSha512(resetPsssWord.Password);
            user.ActiveCode = NuoroLight.Application.Generators.Generators.GeneratorsUniqueCode();
            _userRepository.EditUser(user);
            await _userRepository.Save();
            return ResetPsssWordResult.Success;
        }
        #endregion

        #region ChangePassWord
        public async Task<ChangePasswordResult> ChangePassWord(ChangePasswordViewModel changePassword, int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return ChangePasswordResult.NotFound;
            if (user.Password != _passwordHelper.EncodePasswordSha512(changePassword.CurrentPassword.SanitizeText()))
                return ChangePasswordResult.UnCurrentPassword;
            if (user.Password == _passwordHelper.EncodePasswordSha512(changePassword.NewPassword.SanitizeText()))
                return ChangePasswordResult.DuplicatePassword;
            user.Password = _passwordHelper.EncodePasswordSha512(changePassword.NewPassword.SanitizeText());
            try
            {
                _userRepository.EditUser(user);
                await _userRepository.Save();
                return ChangePasswordResult.Success;
            }
            catch
            {
                return ChangePasswordResult.Error;
            }
        }
        #endregion

        #region GetUserInformation
        public async Task<UserInformationViewModel> GetUserInformation(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user != null)
            {
                return new UserInformationViewModel()
                {
                    Email = user.Email,
                    Mobile = user.Mobile,
                    Avatar = user.Avatar,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    SitrUrl = user.SiteUrl,
                    RegisterDate = user.CreateDate
                };
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region GetUserInfoForEdit
        public async Task<EditUserProfileViewModel> GetUserInfoForEdit(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;
            return new EditUserProfileViewModel()
            {
                AvatarName = user.Avatar,
                Email = user.Email,
                Gender = user.Gender.Value,
                SiteUrl = user.SiteUrl,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
        #endregion

        #region EditProfile
        public async Task<EditUserProfileResult> EditProfile(EditUserProfileViewModel editUserProfile, IFormFile UserAvatar, int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return EditUserProfileResult.NotFound;
            if (user.IsBlock) return EditUserProfileResult.UserIsBlock;
            if (!user.IsActive) return EditUserProfileResult.UserIsBlock;
            if (user.Email.FixedEmail() != editUserProfile.Email.FixedEmail())
                if (await _userRepository.IsExistsUserByEmail(editUserProfile.Email.FixedEmail()))
                    return EditUserProfileResult.EmailIsExist;
            user.FirstName = editUserProfile.FirstName;
            user.LastName = editUserProfile.LastName;
            user.Gender = editUserProfile.Gender;
            user.SiteUrl = editUserProfile.SiteUrl;

            try
            {
                if (UserAvatar != null)
                {
                    if (UserAvatar.IsImage())
                    {
                        var imageName = NuoroLight.Application.Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(UserAvatar.FileName);
                        var res = UserAvatar.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100, PathExtension.UserAvatarThumbServer, user.Avatar);
                        if (res)
                        {
                            user.Avatar = imageName;

                        }
                        else
                        {
                            return EditUserProfileResult.NotIsIamage;
                        }
                    }
                    else
                    {
                        return EditUserProfileResult.NotIsIamage;
                    }
                }
                _userRepository.EditUser(user);
                await _userRepository.Save();
                return EditUserProfileResult.Success;
            }
            catch
            {
                return EditUserProfileResult.Error;
            }
        }
        #endregion

        #region FilterUser
        public async Task<FilterUserViewModel> FilterUser(FilterUserViewModel filterUser)
        {
            return await _userRepository.FilterUser(filterUser);
        }
        #endregion

        #region DeleteUser
        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return false;
            try
            {
                user.IsDelete = true;
                _userRepository.EditUser(user);
                await _userRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region RestoreUser
        public async Task<bool> RestoreUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return false;
            try
            {
                user.IsDelete = false;
                _userRepository.EditUser(user);
                await _userRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region CreateUser
        public async Task<CreateUserResult> CreateUser(CreateUserViewModel createUser, List<int> SelectedPermission, IFormFile userAvatar)
        {
            if (await _userRepository.IsExistsUserByEmail(FixedText.FixEmail(createUser.Email)))
                return CreateUserResult.ExistsEmail;
            if (await _userRepository.IsExistsUserByUserName(createUser.UserName))
                return CreateUserResult.ExistUserName;
            if (await _userRepository.IsExistsUserByMobile(createUser.Mobile))
                return CreateUserResult.ExistMobile;

            var user = new User()
            {
                UserName = createUser.UserName.SanitizeText(),
                Mobile = createUser.Mobile.SanitizeText(),
                Email = createUser.Email.SanitizeText(),
                Password = _passwordHelper.EncodePasswordSha512(createUser.PassWord.SanitizeText()),
                ActiveCode = NuoroLight.Application.Generators.  Generators.GeneratorsUniqueCode(),
                IsActive = true

            };
            try
            {
                if (userAvatar != null && userAvatar.IsImage())
                {
                    var imageName = NuoroLight.Application.Generators. Generators.GeneratorsUniqueCode() + Path.GetExtension(userAvatar.FileName);
                    var res = userAvatar.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100,
                        PathExtension.UserAvatarThumbServer);
                    if (res)
                    {
                        user.Avatar = imageName;
                    }
                }
                await _userRepository.AddUser(user);
                await _userRepository.Save();
                await _userPermissionRepository.AddUserPermission(SelectedPermission, user.UserId);
                await _userPermissionRepository.Save();
                return CreateUserResult.Success;
            }
            catch
            {
                return CreateUserResult.Error;
            }
        }
        #endregion

        #region UserInfoForEdit
        public async Task<EditUserForAdminViewModel> UserInfoForEdit(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;
            return new EditUserForAdminViewModel()
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Avatar = user.Avatar,
                IsActive = user.IsActive,
                IsBlock = user.IsBlock
            };
        }
        #endregion

        #region EditUserForAdmin
        public async Task<EditUserResult> EditUserForAdmin(EditUserForAdminViewModel editUser, List<int> SelectedPermission, IFormFile userAvatar)
        {
            var user = await _userRepository.GetUserById(editUser.UserId);
            if (user == null) return EditUserResult.NotFound;
            user.FirstName = editUser.FirstName.SanitizeText();
            user.LastName = editUser.LastName.SanitizeText();
            user.IsActive = editUser.IsActive;
            user.IsBlock = editUser.IsBlock;

            if (!string.IsNullOrEmpty(editUser.PassWord))
            {
                user.Password = _passwordHelper.EncodePasswordSha512(editUser.PassWord.SanitizeText());
            }
            try
            {
                if (userAvatar != null && userAvatar.IsImage())
                {
                    var imageName = NuoroLight.Application.Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(userAvatar.FileName);
                    var res = userAvatar.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100,
                        PathExtension.UserAvatarThumbServer, user.Avatar);
                    if (res)
                    {
                        user.Avatar = imageName;
                    }
                }
                _userRepository.EditUser(user);
                await _userRepository.Save();
                _userPermissionRepository.RemoveUserPermission(editUser.UserId);
                await _userPermissionRepository.Save();
                await _userPermissionRepository.AddUserPermission(SelectedPermission, editUser.UserId);
                await _userPermissionRepository.Save();

                return EditUserResult.Success;
            }
            catch
            {
                return EditUserResult.Error;
            }

        }
        #endregion

        #region GetUserByUserId
        public async Task<User> GetUserByUserId(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }
        #endregion

        #region GetUserDetialByUserId
        public async Task<UserDetialViewModel> GetUserDetialByUserId(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;
            return new UserDetialViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                Mobile = user.Mobile,
                IsBlock = user.IsBlock,
                RegisterDate = user.CreateDate,
                UserAvatar = user.Avatar,
                FullName = user.FullName,
                IsDelete = user.IsDelete
            };
        }
        #endregion

    }
}
