using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmersonCommonUtility
{
    public  static class EmersonConstants
    {
        public const string Temperature = "Temperature";
        public const string F = "°F";
        public const string C = "°C";
        public const string Humidity = "Humidity";
    }

    public static class EmersonTempConversion
    {
        public static Decimal ConvertFtoC(string F)
        {
            return (Decimal.Parse(F) - 32) * 5 / 9;
        }

        public static Decimal ConvertCtoF(string C)
        {
            return (Decimal.Parse(C) * 9 / 5) + 32;
        }
    }
}
