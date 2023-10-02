using EmersonDB.Model;
using Microsoft.EntityFrameworkCore;
using EmersonCommonUtility;
using EmersonDB.DTO;
using EmersonAPI.Interfaces;
using EmersonAPI.Repositories;

namespace EmersonAPI.BusinessService
{
    public class VariableService : IVariableService
    {
        public IUnitOfWork _iunitofwork;

        public VariableService(IUnitOfWork mainRepo) { _iunitofwork = mainRepo; }

        public IEnumerable<CityTemperatureHumidityDTO> GetAllVariables(DateTime? startFilterDate, DateTime? endFilterDate, string? name, int? cityid)
        {
            List<CityTemperatureHumidityDTO> finalResult = new List<CityTemperatureHumidityDTO>();
            var masterlists = _iunitofwork.iVariableRepo.GetAllVariable();
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
            {
                masterlists = masterlists.Where(x => x.Name == name);
            }

            if (cityid.HasValue)
            {
                masterlists = masterlists.Where(x => x.CityId == cityid);
            }

            if (startFilterDate.HasValue)
            {
                masterlists = masterlists.Where(t => (DateTime)(object)t.Timestamp >= startFilterDate.Value);
            }

            if (endFilterDate.HasValue)
            {
                masterlists = masterlists.Where(t => (DateTime)(object)t.Timestamp <= endFilterDate.Value);
            }

            var result =  masterlists.Include(x=>x.City).AsEnumerable();
            if (result.Any())
            {
                foreach (var data in result)
                {
                    finalResult.Add(new CityTemperatureHumidityDTO()
                    {
                        city = data.City.CityName,
                        name = data.Name,
                        unit = data.Unit,
                        timestamp = data.Timestamp.ToString("dd-MMM-yyyy"),
                        value = data.Name == EmersonConstants.Temperature && data.Unit == EmersonConstants.F ? EmersonTempConversion.ConvertFtoC(data.Value).ToString() : data.Value
                    });
                }
            }

            return finalResult;
        }

        public List<(string cityname, int numberOfDays)> GetNumDaysTempOverInCelcius(DateTime StartDateParam, DateTime EndDateParam, Decimal Temperature)
        {
            List<(string cityname, int days)> result = new List<(string cityname, int days)>();
            var DataLists = _iunitofwork.iVariableRepo.GetAllVariable().
                                            Include(x => x.City).
                                            Where(t => t.Name == EmersonConstants.Temperature &&
                                                        (DateTime)(object)t.Timestamp >= StartDateParam &&
                                                        (DateTime)(object)t.Timestamp <= EndDateParam
                                            ).Select(m => new { CityName = m.City.CityName, Temperature = m.Value, Unit = m.Unit }).AsEnumerable();

            if (DataLists.Any())
            {
                var CityLists = DataLists.Where(t => (t.Unit == EmersonConstants.F ? EmersonTempConversion.ConvertFtoC(t.Temperature) : Decimal.Parse(t.Temperature)) > Temperature);
                if (CityLists.Any())
                {
                    var groupCity = CityLists.GroupBy(x => x.CityName).Select(x => new { city = x.Key, day = x.Count() });
                    foreach(var city in groupCity)
                    {
                        result.Add((city.city, city.day));
                    }
                }
            }

            return result;
        }

        public List<(string cityname, decimal avgHumidity)> GetCitiesWithAvgHumidity(DateTime StartDateParam, DateTime EndDateParam)
        {
            List<(string cityname, decimal humidity)> result = new List<(string cityname, decimal humidity)>();
            var DataLists = _iunitofwork.iVariableRepo.GetAllVariable().
                                            Include(x => x.City).
                                            Where(t => t.Name == EmersonConstants.Humidity &&
                                                        (DateTime)(object)t.Timestamp >= StartDateParam &&
                                                        (DateTime)(object)t.Timestamp <= EndDateParam
                                            ).
                                            Select(t => new { CityName = t.City.CityName, Humidity = t.Value }).AsEnumerable().
                                            GroupBy(t => t.CityName).
                                            Select(t => new { cityname = t.Key, Avg = t.Average(k=> Decimal.Parse(k.Humidity)) });

            if (DataLists.Any())
            {
                foreach (var city in DataLists)
                {
                    result.Add((city.cityname, city.Avg));
                }
            }

            return result;
        }
    }
}
