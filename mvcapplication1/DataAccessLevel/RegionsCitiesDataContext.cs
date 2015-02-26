using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.DataAccessLevel
{
    public class RegionsDataContext : IEquatable<RegionsDataContext>
    {
        public int regionID { get; set; }
        public string name { get; set; }

        public bool Equals(RegionsDataContext obj)
        {
            return this.regionID.Equals(obj.regionID) && this.name.Equals(obj.name);
        }
    }

    public class CitiesDataContext
    {

        public string name { get; set; }
        public int regionID { get; set; }
        public int yandexID { get; set; }
        public string owmID { get; set; }
    }

    public class RegionCitiesLists
    {
        public List<RegionsDataContext> regionsList = new List<RegionsDataContext>();
        public List<CitiesDataContext> citiesList = new List<CitiesDataContext>();

    }
}