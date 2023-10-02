using EmersonAPI.Interfaces;
using EmersonDB;
using EmersonDB.Model;
using Microsoft.EntityFrameworkCore;

namespace EmersonAPI.Repositories
{
    public class CityRepo : GenericRepository<City>, ICityRepo
    {
        public CityRepo(EmersonContext dbContext) : base(dbContext)
        {

        }

        IQueryable<City> ICityRepo.GetAllCity()
        {
            return GetAll();
        }

        async Task<City?> ICityRepo.GetCityByID(int id)
        {
            return (await GetById(id));
        }
    }
}
