using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Enums;

namespace Business.Services
{
    public interface IAccountService
    {
        Result Register(AccountRegisterModel model);
        Result Login(AccountLoginModel model, UserModel user);
    }

    public class AccountService : IAccountService
    {
        private readonly IUserService _userService;

        public AccountService(IUserService userService)
        {
            _userService = userService;
        }

        public Result Login(AccountLoginModel model, UserModel user)
        {
            var existingUser = _userService.Query().SingleOrDefault(u => u.UserName == model.UserName && u.Password == model.Password && u.IsActive);
            if (existingUser is null)
                return new ErrorResult("Username or password is incorrect!");
            user.UserName = existingUser.UserName;
            user.RoleNameDisplay = existingUser.RoleNameDisplay;
            user.Id = existingUser.Id;
            return new SuccessResult();
        }

        public Result Register(AccountRegisterModel model)
        {
            var user = new UserModel()
            {
                IsActive = true,
                Password = model.Password,
                UserName = model.UserName,
                RoleId = (int)Roles.User
            };
            return _userService.Add(user);
        }
    }
}
