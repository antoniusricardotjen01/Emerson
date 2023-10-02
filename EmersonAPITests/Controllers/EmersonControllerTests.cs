using EmersonAPI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using EmersonAPI.BusinessService;
using EmersonDB.Model;
using EmersonAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using EmersonDB.DTO;
using EmersonAPI.ModelParam;

namespace EmersonAPI.Controllers.Tests
{
    [TestClass]
    public class EmersonControllerTests
    {
        #region Setup
        private EmersonController _controller;
        private Mock<IVariableService> _variableService;
        private Mock<ICityService> _cityService;
        private List<City> Cities = new List<City>() { new City() { Id = 1, CityName = "Singapore" }, new City() { Id = 2, CityName = "Colombo" } };
        private List<(string cityname, decimal humidity)> HumidityResult = new List<(string cityname, decimal humidity)>() { ("Jakarta", 38.5M), ("Singapore", 39.53M) };
        private List<(string cityname, int numDay)> HotWeatherResult = new List<(string cityname, int numDay)>() { ("Jakarta", 38), ("Singapore", 39) };
        private List<CityTemperatureHumidityDTO> CityTempDTO = new List<CityTemperatureHumidityDTO>()
        {
            new CityTemperatureHumidityDTO() { name = "Temperature", city = "Singapore", unit= "°F", value = "85", timestamp = "2023-01-10"   },
            new CityTemperatureHumidityDTO() { name = "Temperature", city = "Colombo", unit= "°C", value = "32", timestamp = "2023-01-11"   },
            new CityTemperatureHumidityDTO() { name = "Temperature", city = "Bangalore", unit= "°C", value = "50", timestamp = "2023-01-12"   },
            new CityTemperatureHumidityDTO() { name = "Humidity", city = "Singapore", unit= "%", value = "75", timestamp = "2023-01-12"   },
            new CityTemperatureHumidityDTO() { name = "Humidity", city = "Bangalore", unit= "%", value = "80", timestamp = "2023-01-12"   }
        };
        //            _variableRepo.Setup(y => y.GetAllVariable()).Returns(Variables.AsQueryable()); //doesn't work

        public void Setup()
        {
            _variableService = new Mock<IVariableService>();
            _cityService = new Mock<ICityService>();
            _controller = new EmersonController(_cityService.Object, _variableService.Object);
        }
        #endregion

        #region Average Humidity
        [TestMethod]
        public void AttemptReturnAvgHumidityBadRequest()
        {
            Setup();
            var result = _controller.GetCitiesWithAvgHumidity(0, 0) as BadRequestResult;
            Assert.AreEqual(((int)HttpStatusCode.BadRequest), result?.StatusCode);
        }

        [TestMethod()]
        public void AttemptReturnAvgHumidityOK()
        {
            Setup();
            _variableService.Setup(b => b.GetCitiesWithAvgHumidity(new DateTime(2023, 10, 01), new DateTime(2023, 10, 31))).Returns(HumidityResult);
            Assert.AreEqual((int)HttpStatusCode.OK, (_controller.GetCitiesWithAvgHumidity(2023, 10) as ObjectResult)?.StatusCode);
        }

        [TestMethod()]
        public void AttemptGetCitiesWithAvgHumidityReturnNoData()
        {
            Setup();
            _variableService.Setup(b => b.GetCitiesWithAvgHumidity(new DateTime(2023, 01, 01), new DateTime(2023, 01, 31))).Returns(HumidityResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (_controller.GetCitiesWithAvgHumidity(2023, 10) as BadRequestResult)?.StatusCode);
        }
        #endregion

        #region NumOfDays - Hot Weather
        [TestMethod()]
        public void AttemptGetNumDaysTempOverInCelciusTestReturnBadRequest()
        {
            Setup();
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (_controller.GetNumDaysTempOverInCelcius(0, 0, 30) as BadRequestResult)?.StatusCode);
        }

        [TestMethod()]
        public void AttemptGetNumDaysTempOverInCelciusReturnNoData()
        {
            Setup();
            _variableService.Setup(b => b.GetNumDaysTempOverInCelcius(new DateTime(2023, 01, 01), new DateTime(2023, 01, 31), 35M)).Returns(HotWeatherResult);
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (_controller.GetNumDaysTempOverInCelcius(2023, 10, 35) as BadRequestResult)?.StatusCode);
        }

        [TestMethod()]
        public void AttemptGetNumDaysTempOverInCelciusReturnOk()
        {
            Setup();
            _variableService.Setup(b => b.GetNumDaysTempOverInCelcius(new DateTime(2023,10,01),new DateTime(2023,10,31), 35M)).Returns(HotWeatherResult);
            Assert.AreEqual((int)HttpStatusCode.OK, (_controller.GetNumDaysTempOverInCelcius(2023, 10, 35) as OkObjectResult)?.StatusCode);
        }
        #endregion

        #region Get Variables
        //No Data Returned
        [TestMethod()]
        public void AttemptGetVariablesReturnBadRequest()
        {
            Setup();
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (_controller.GetVariables(It.IsAny<GetVariableParam>()) as BadRequestResult)?.StatusCode);
        }

        //Partial Data = in this scenario, no city records in DB
        [TestMethod()]
        public void AttemptGetVariablesWithPartialDataReturnGoodRequest()
        {
            Setup();
            _variableService.Setup(b => b.GetAllVariables(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string?>(), It.IsAny<int?>())).Returns(CityTempDTO.AsEnumerable());
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (_controller.GetVariables(It.IsAny<GetVariableParam>()) as BadRequestResult)?.StatusCode);
        }

        //Both City and Variable Data exist but invalid parameter
        [TestMethod()]
        public void AttemptGetVariablesReturnBadRequestParsingInvalidParameter()
        {
            Setup();
            GetVariableParam sample = new GetVariableParam() { startFilterDate = new DateTime(2023, 10, 2), endFilterDate = new DateTime(2023, 9, 02) };
            _cityService.Setup(x => x.GetAllCities()).Returns(Cities);
            _variableService.Setup(b => b.GetAllVariables(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string?>(), It.IsAny<int?>())).Returns(CityTempDTO.AsEnumerable());
            Assert.AreEqual((int)HttpStatusCode.BadRequest, (_controller.GetVariables(sample) as BadRequestResult)?.StatusCode);
        }

        [TestMethod()]
        public void AttemptGetVariablesReturnOkWithNoParameter()
        {
            Setup();
            _variableService.Setup(b => b.GetAllVariables(It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<string?>(), It.IsAny<int?>())).Returns(CityTempDTO.AsEnumerable());
            _cityService.Setup(x => x.GetAllCities()).Returns(Cities);
            GetVariableParam sample = new GetVariableParam() { name = "Temperature" };
            Assert.AreEqual((int)HttpStatusCode.OK, (_controller.GetVariables(sample) as OkObjectResult)?.StatusCode);
        }
        #endregion

        #region City

        [TestMethod]
        public void AttemptReturnCitiesShouldNotBeNull()
        {
            Setup();
            _cityService.Setup(x => x.GetAllCities()).Returns(Cities);
            Assert.IsNotNull(_controller.GetCities());
        }

        [TestMethod]
        public void AttemptReturnCitiesShouldBeNull()
        {
            Setup();
            Assert.IsNull(_controller.GetCities());
        }

        #endregion
    }
}