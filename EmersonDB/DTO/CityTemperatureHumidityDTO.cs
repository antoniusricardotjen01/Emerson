using EmersonDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmersonDB.DTO
{
    public class CityTemperatureHumidityDTO
    {
        public string name { get; set; }
        
        public string unit { get; set; }
        
        public string value { get; set; }
        
        public string timestamp { get; set; } 
        
        public string city { get; set; }
    }
}
