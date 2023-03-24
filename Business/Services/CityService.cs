using AppCore.Business.Services.Bases;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Repositories;

namespace Business.Services
{
    public interface ICityService : IService<CityModel>
    {
        List<CityModel> GetList(int countryId);
    }

    public class CityService : ICityService
    {
        private readonly CityRepoBase _cityRepo;

        public CityService(CityRepoBase cityRepo)
        {
            _cityRepo = cityRepo;
        }

        public Result Add(CityModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _cityRepo.Dispose();
        }

        public IQueryable<CityModel> Query()
        {
            return _cityRepo.Query(c => c.Country).OrderBy(c => c.Name).Select(c => new CityModel()
            {
                CountryId = c.CountryId,
                Guid = c.Guid,
                Name = c.Name,
                Id = c.Id
            });
        }

        public List<CityModel> GetList(int countryId)
        {
            return Query().Where(c => c.CountryId == countryId).ToList();
        }

        public Result Update(CityModel model)
        {
            throw new NotImplementedException();
        }
    }
}
