using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheTime.DataAccessLevel
{
    public class TenDaysForecastsDataContext
    {
       public int settingID {get; set;}
       public DateTime periodDate {get; set;} 
       public string timeOfDay {get; set;} 
       public string symbol {get; set;}
       public string temperature { get; set; }
    }
}
