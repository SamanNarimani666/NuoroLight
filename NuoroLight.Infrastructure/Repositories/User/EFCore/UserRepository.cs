using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NuoroLight.Domain.IRepositories;
using NuoroLight.Domain.ViewModels.Paging;
using NuoroLight.Domain.ViewModels.User;
using NuoroLight.Infrastructure.Context;
namespace NuoroLight.Data.Repositories.User;

public partial class UserRepository : IUserRepository
{
    #region Constructor
    private readonly NuoroLightContext _context;
    public UserRepository(NuoroLightContext context)
    {
        _context = context;
    }
    #endregion

    #region IsExistsUserByEmail
    public async Task<bool> IsExistsUserByEmail(string email) => await _context.Users.AnyAsync(p => p.Email == email);
    #endregion

    #region IsExistsUserByMobile

    public async Task<bool> IsExistsUserByMobile(string mobile)=> await _context.Users.AnyAsync(p => p.Mobile == mobile);
    #endregion

    #region IsExistsUserByUserName
    public async Task<bool> IsExistsUserByUserName(string userName) => await _context.Users.AnyAsync(p => p.UserName == userName);
    #endregion

    #region IsExistsUserByActiveCode
    public async Task<bool> IsExistsUserByActiveCode(string activeCode) => await _context.Users.AnyAsync(p => p.ActiveCode == activeCode);

    #endregion

    #region GetUserByEmail
    public async Task<Domain.Entities.User.User> GetUserByEmail(string email) => await _context.Users.SingleOrDefaultAsync(p => p.Email == email);
    #endregion

    #region GetUserByActiveCode
    public async Task<Domain.Entities.User.User> GetUserByActiveCode(string activeCode) => await _context.Users.SingleOrDefaultAsync(p => p.ActiveCode == activeCode);
    #endregion

    #region GetUserByMobile
    public async Task<Domain.Entities.User.User> GetUserByMobile(string mobile) => await _context.Users.SingleOrDefaultAsync(p => p.Mobile == mobile);
    #endregion

    #region GetUserById
    public async Task<Domain.Entities.User.User> GetUserById(int userId) => await _context.Users.FindAsync(userId);
    #endregion

    #region EditUser
    public void EditUser(Domain.Entities.User.User user)
    {
        user.ModifiedDate = DateAndTime.Now;
        _context.Users.Update(user);
    }
    #endregion

    #region AddUser
    public async Task AddUser(Domain.Entities.User.User user)
    {
        user.CreateDate = DateAndTime.Now;
        user.ModifiedDate = DateAndTime.Now;
        await _context.Users.AddAsync(user);
    }
    #endregion

    #region FilterUser
    public async Task<FilterUserViewModel> FilterUser(FilterUserViewModel filterUser)
    {
        var user = _context.Users.AsQueryable();

        #region Order
        switch (filterUser.UserOrderBy)
        {
            case FilterUserOrderBy.Create_Date_Asc:
                user = user.OrderBy(p => p.CreateDate);
                break;

            case FilterUserOrderBy.Create_Date_Desc:
                user = user.OrderByDescending(p => p.CreateDate);
                break;
        }
        #endregion

        #region filter
        if (filterUser.Users != null)
            user = user.Where(p => EF.Functions.Like(p.UserName, $"%{filterUser.UserName}%"));
        if (filterUser.Email != null)
            user = user.Where(p => EF.Functions.Like(p.Email, $"%{filterUser.Email}%"));
        if (filterUser.Mobile != null)
            user = user.Where(p => EF.Functions.Like(p.Mobile, $"%{filterUser.Mobile}%"));
        #endregion

        #region State
        switch (filterUser.UserState)
        {
            case UserState.All:
                break;
            case UserState.Active:
                user = user.Where(p => p.IsActive);
                break;
            case UserState.NotActive:
                user = user.Where(p => !p.IsActive);
                break;
            case UserState.IsDelete:
                user = user.Where(p => p.IsDelete);
                break;
            case UserState.NotDelete:
                user = user.Where(p => !p.IsDelete);
                break;
            case UserState.IsBolick:
                user = user.Where(p => p.IsBlock);
                break;
            case UserState.NotBlock:
                user = user.Where(p => !p.IsBlock);
                break;
        }
        #endregion

        #region Paging
        var pager = Pager.Build(filterUser.PageId, await user.CountAsync(), filterUser.TakeEntity,
            filterUser.HowManyShowPageAfterAndBefore);
        var allProduct = user.Paging(pager).ToList();
        return filterUser.SetPaging(pager).SetUser(allProduct);

        #endregion
    }
    #endregion

    #region Save
    public async Task Save()
    {
        await _context.SaveChangesAsync();
    }
    #endregion

    #region Dispose
    public async ValueTask DisposeAsync()
    {
        if (_context != null)
        {
            await _context.DisposeAsync();
        }
    }
    #endregion

}

