using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
using DataAccess.Repositories;

namespace Business.Services
{
    public interface IUserService : IService<UserModel>
    {

    }

    public class UserService : IUserService
    {
        private readonly UserRepoBase _userRepo;

        public UserService(UserRepoBase userRepo)
        {
            _userRepo = userRepo;
        }

        public Result Add(UserModel model)
        {
            //string userName = model.UserName.ToUpper(new System.Globalization.CultureInfo("tr-TR"));
            //if (_userRepo.Query().Any(u => u.UserName.ToUpper() == userName))
            //    return new ErrorResult("User with the same name exists!");
            var users = _userRepo.Query().ToList();
            if (users.Any(u => u.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase)))
                return new ErrorResult("User with the same name exists!");
            var entity = new User()
            {
                IsActive = model.IsActive,
                Password = model.Password,
                RoleId = model.RoleId,
                UserName = model.UserName
            };
            _userRepo.Add(entity);
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _userRepo.Dispose();
        }

        public IQueryable<UserModel> Query()
        {
            return _userRepo.Query(u => u.Role).OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName).Select(u => new UserModel()
            {
                Guid = u.Guid,
                Id = u.Id,
                IsActive = u.IsActive,
                Password = u.Password,
                RoleId = u.RoleId,
                UserName = u.UserName,
                RoleNameDisplay = u.Role.Name
            });
        }

        public Result Update(UserModel model)
        {
            throw new NotImplementedException();
        }
    }
}
