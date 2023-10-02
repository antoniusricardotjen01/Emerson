using EmersonAPI.BusinessService;
using EmersonAPI.ModelParam;
using EmersonCommonUtility;
using EmersonDB.DTO;
using EmersonDB.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Collections;

namespace EmersonAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmersonController : ControllerBase
    {
        private readonly ICityService _iCityService;
        private readonly IVariableService _ivariableService;

        public EmersonController(ICityService iCityService, IVariableService ivariableService) { _iCityService = iCityService; _ivariableService = ivariableService; }

        [HttpGet]
        [Route("GetAllCities")]
        public List<City> GetCities()
        {
            var allCities = _iCityService.GetAllCities();
            return allCities;
        }

        [HttpPost]
        [Route("GetVariables")]
        public object GetVariables(GetVariableParam input) {

            if (input == null || (input.startFilterDate.HasValue && input.endFilterDate.HasValue && input.startFilterDate.Value > input.endFilterDate.Value))
            {
                return BadRequest();
            }

            var allData = _ivariableService.GetAllVariables(input.startFilterDate, input.endFilterDate, input.name, input.cityid);
            if (allData.Any())
            {

                var allCities = _iCityService.GetAllCities().OrderBy(x => x.CityName).Select(x => x.CityName);
                List<string> header = new List<string>() { "Date" };
                header.AddRange(allCities);

                ArrayList result = new ArrayList() { header };

                var groupCities = allData.GroupBy(x => x.timestamp).Select(x => new { Timestamp = x.Key, Data = x });
                foreach (var group in groupCities)
                {
                    ArrayList entryData = new ArrayList() { group.Timestamp };
                    var data = group.Data;
                    foreach (string city in allCities)
                    {
                        var datapercity = data.FirstOrDefault(x => x.city == city);
                        entryData.Add(datapercity != null ? decimal.Parse(datapercity.value) : 0);
                    }
                    result.Add(entryData);
                }

                return Ok(new { DataResult = result.ToArray(), variablename = input.name });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetNumDaysTempOverInCelcius/{year:int}/{month:int}/{temp:int}")]
        public IActionResult GetNumDaysTempOverInCelcius(int year, int month, int temp)
        {
            if (year <= 0 || month <= 0 || temp <= 0)
            {
                return BadRequest();
            }

            CityTempDTO result = new CityTempDTO();
            var data = _ivariableService.GetNumDaysTempOverInCelcius(new DateTime(year, month, 1), new DateTime(year, month, DateTime.DaysInMonth(year, month)), temp);
            if (data != null && data.Any())
            {
                var orderedData = data.OrderByDescending(x => x.numberOfDays).FirstOrDefault();
                result.NumOfDay = orderedData.numberOfDays;
                result.CityName = orderedData.cityname;
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("GetCitiesWithAvgHumidity/{year:int}/{month:int}")]
        public IActionResult GetCitiesWithAvgHumidity(int year, int month)
        {
            if (year <= 0 || month <= 0)
            {
                return BadRequest();
            }

            CityTempDTO result = new CityTempDTO();
            var data = _ivariableService.GetCitiesWithAvgHumidity(new DateTime(year, month, 1), new DateTime(year, month, DateTime.DaysInMonth(year, month)));
            if (data != null && data.Any())
            {
                var orderedData = data.OrderByDescending(x => x.avgHumidity).FirstOrDefault();
                result.AverageHumidity = orderedData.avgHumidity;
                result.CityName = orderedData.cityname;
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
