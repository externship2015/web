using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheTime.DataAccessLevel
{
    /// <summary>
    /// Представляет контекстный класс для таблицы Settings
    /// </summary>
    class SettingsDataContext
    {
        public int ID { get; set; }
        public int cityID { get; set; }
        public int sourceID { get; set; } // 2 - ya, 1 - owm
        public DateTime saveDate { get; set; }
    }
}
