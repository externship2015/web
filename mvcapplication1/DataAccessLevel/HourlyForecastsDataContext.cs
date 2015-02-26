using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheTime.DataAccessLevel
{
    public class HourlyForecastsDataContext
    {
        public int settingID { get; set; }
        public DateTime periodDate { get; set; }
        public int periodTime { get; set; }
        public string description { get; set; }
        public string temperature { get; set; }
        public string windSpeed { get; set; }
        public string windDirection { get; set; }  // в формате С, СЗ, Ю, ....
        public string pressure { get; set; }
        public string hummidity { get; set; }
        public string symbol { get; set; }
    }
}
