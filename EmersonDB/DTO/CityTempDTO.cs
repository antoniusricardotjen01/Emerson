using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmersonDB.DTO
{
    public class CityTempDTO
    {
        public int NumOfDay { get; set; }

        public string CityName { get; set; }

        public decimal AverageHumidity { get; set; }
    }
}
