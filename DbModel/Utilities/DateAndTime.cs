using Persia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbModel.Utilities
{
    public class DateAndTime
    {
        public static DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public static string ConvertToPersian(DateTime dateTime, string mod = "")
        {
            SolarDate solar = Calendar.ConvertToPersian(dateTime);
            return string.IsNullOrEmpty(mod) ? solar.ToString() : solar.ToString(mod);
        }
    }
}