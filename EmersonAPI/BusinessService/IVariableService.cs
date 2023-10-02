using EmersonDB.DTO;
using EmersonDB.Model;

namespace EmersonAPI.BusinessService
{
    public interface IVariableService
    {
        IEnumerable<CityTemperatureHumidityDTO> GetAllVariables(DateTime? startFilterDate, DateTime? endFilterDate, string? name, int? cityid);

        List<(string cityname, int numberOfDays)> GetNumDaysTempOverInCelcius(DateTime StartDateParam, DateTime EndDateParam, Decimal Temperature);

        List<(string cityname, decimal avgHumidity)> GetCitiesWithAvgHumidity(DateTime StartDateParam, DateTime EndDateParam);
    }
}
