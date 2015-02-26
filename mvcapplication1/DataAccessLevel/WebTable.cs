using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcApplication1.DataAccessLevel
{
    class WebTable
    {
        public DateTime date { get; set; }
        public int yaTempDay { get; set; }
        public int yaTempNight { get; set; }
        public int owmTempDay { get; set; }
        public int owmTempNight { get; set; }
        public string yaSymbolDay { get; set; }
        public string yaSymbolNight { get; set; }
        public string owmSymbolNight { get; set; }
        public string owmSymbolDay { get; set; }

    }
}
