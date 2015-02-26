using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheTime.DataAccessLevel
{
    public class Forecast
    {
        //public List<HourlyForecastsDataContext> hourlyList { get; set; }
        //public List<DailyForecastsDataContext> dailyList { get; set; }
        //public List<TenDaysForecastsDataContext> tenDaysList { get; set; }
        //public CurrentWeather curWeather { get; set; }
        public List<HourlyForecastsDataContext> hourlyList = new List<HourlyForecastsDataContext>();
        public List<DailyForecastsDataContext> dailyList = new List<DailyForecastsDataContext>();
        public List<TenDaysForecastsDataContext> tenDaysList = new List<TenDaysForecastsDataContext>();
        public CurrentWeather curWeather = new CurrentWeather();
    }
}
