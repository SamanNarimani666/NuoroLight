using NuoroLight.Domain.Entities.User;
using NuoroLight.Domain.ViewModels.User;

namespace NuoroLight.Domain.IRepositories;
public interface IUserRepository:IAsyncDisposable
{
    Task<bool> IsExistsUserByEmail(string email);
    Task<bool> IsExistsUserByMobile(string mobile);
    Task<bool> IsExistsUserByUserName(string userName);
    Task<User> GetUserByEmail(string email);
    Task<bool> IsExistsUserByActiveCode(string activeCode);
    Task<User> GetUserByActiveCode(string activeCode);
    Task AddUser(User user);
    void EditUser(User user);
    Task<User> GetUserByMobile(string mobile);
    Task<User> GetUserById(int userId);
    Task<FilterUserViewModel> FilterUser(FilterUserViewModel filterUser);
    Task Save();


}

