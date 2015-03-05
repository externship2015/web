using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.DataAccessLevel
{
    class WebTable
    {
        public string date { get; set; }
        public Nullable<int> yaTempDay { get; set; }
        public Nullable<int> yaTempNight { get; set; }
        public Nullable<int> owmTempDay { get; set; }
        public Nullable<int> owmTempNight { get; set; }
        public string yaSymbolDay { get; set; }
        public string yaSymbolNight { get; set; }
        public string owmSymbolNight { get; set; }
        public string owmSymbolDay { get; set; }
        
        public string owmWindSpeedDay { get; set; }
        public string owmWindSpeedNight { get; set; }  
        public string owmPressureDay { get; set; }
        public string owmPressureNight { get; set; }
        public string owmHummidityDay { get; set; }
        public string owmHummidityNight { get; set; }

        public string yaWindSpeedDay { get; set; }
        public string yaWindSpeedNight { get; set; }
        public string yaPressureDay { get; set; }
        public string yaPressureNight { get; set; }
        public string yaHummidityDay { get; set; }
        public string yaHummidityNight { get; set; }
    }
    /*
    public class PageInfo
    {
        public int PageNumber { get; set; } // номер текущей страницы
        public int PageSize { get; set; } // кол-во объектов на странице
        public int TotalItems { get; set; } // всего объектов
        public int TotalPages  // всего страниц
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }
    public class IndexViewModel
    {
        public List<WebTable> row { get; set; }
        public PageInfo PageInfo { get; set; }
    }*/
}
