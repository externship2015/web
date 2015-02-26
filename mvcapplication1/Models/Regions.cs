using MvcApplication1.DataAccessLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheTime.DataAccessLevel;

namespace MvcApplication1.Models
{
    public class StRegions
    {
        static SQLiteDatabaseWorker SQLworker = new SQLiteDatabaseWorker();
        static RegionCitiesLists rsl = new RegionCitiesLists();

        public static IQueryable<RegionsDataContext> GetRegions()
        {
            SQLworker.SetConnect();
            rsl = SQLworker.GetCitiesList();
            SQLworker.CloseConnect();
            return rsl.regionsList.AsQueryable();
        }    
    }
}