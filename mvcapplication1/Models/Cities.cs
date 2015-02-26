using MvcApplication1.DataAccessLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheTime.DataAccessLevel;

namespace MvcApplication1.Models
{
    public class stCities
    {
       static SQLiteDatabaseWorker SQLworker = new SQLiteDatabaseWorker();
       static RegionCitiesLists rsl = new RegionCitiesLists();
        public static  IQueryable<CitiesDataContext> GetCities()
        { 
             SQLworker.SetConnect();
                rsl = SQLworker.GetCitiesList();
                SQLworker.CloseConnect();
                return rsl.citiesList.AsQueryable();}       
    }
        //private string countryCode;


        //public Cities(string CountryCode)
        //{
        //    countryCode = CountryCode;
        //}
        ////public List<CitiesDataContext> CitiesForCountry
        ////{
        ////    get
        ////    {
        ////        return GetCitiesForCountry(countryCode);
        ////    }
        ////}
        //private List<CitiesDataContext> GetCitiesForCountry(string CountryCode)
        //{
        //    List<CitiesDataContext> list = null;
        //    int code = Convert.ToInt32(CountryCode);
        //    SQLworker.SetConnect();
        //    rsl = SQLworker.GetCitiesList();
        //    SQLworker.CloseConnect();

        //    foreach (var item in rsl.citiesList.Where(s => s.regionID == code))
        //    {
        //        list.Add(item);
        //    }
        //    return list;
        //}



        //public IQueryable<CitiesDataContext> CitiesForCountry()
        //{       
        //    List<CitiesDataContext> list = GetCitiesForCountry(countryCode);
        //    return list.AsQueryable();
        //}   
    
}