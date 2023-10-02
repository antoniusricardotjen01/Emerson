using EmersonAPI.Interfaces;
using EmersonAPI.Repositories;
using EmersonDB.Model;

namespace EmersonAPI.BusinessService
{
    public class CityService : ICityService
    {
        public IUnitOfWork _iunitofwork;

        public CityService(IUnitOfWork mainRepo) { _iunitofwork = mainRepo; }

        public List<City> GetAllCities()
        {
            return _iunitofwork.iCityRepo.GetAllCity().ToList();
        }

    }
}
