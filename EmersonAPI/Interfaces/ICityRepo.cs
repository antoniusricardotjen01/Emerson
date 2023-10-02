using EmersonDB.Model;

namespace EmersonAPI.Interfaces
{
    public interface ICityRepo
    {
        IQueryable<City> GetAllCity();

        Task<City?> GetCityByID(int id);
    }
}
