using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MvcApplication1.Models;
using MvcApplication1.DataAccessLevel;
using TheTime.DataAccessLevel;

namespace JQGridApp.Controllers
{
    public class HomeController : Controller
    {
       
        


        //SQLworker.SetConnect(SQLworker.);
        //rsl = SQLworker.GetCitiesList();
        //SQLworker.CloseConnect();
        static RegionCitiesLists rsl = new RegionCitiesLists();
        static HomeController()
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public string GetData(List<String> values)
        {             
            /* приходит массив строк
             * values[0] - с - дата
             * values[1] - по - дата
             * values[2] - ид региона - 81
             * values[3] - название города - Ульяновск
             */

            SQLiteDatabaseWorker SQLworker = new SQLiteDatabaseWorker();
            List<WebTable> table = new List<WebTable>();
            SQLworker.SetConnect();
            table = SQLworker.GetWebTable(values[2], values[3], DateTime.Now.AddDays(-10), DateTime.Now.AddDays(20));
            SQLworker.CloseConnect();
            return JsonConvert.SerializeObject(table);
           
        }

        [HttpPost]
        public string Index(FormModel Model)
        {
            // получаем id региона и название города, и две даты
            SQLiteDatabaseWorker SQLworker = new SQLiteDatabaseWorker();
            



            string fin = "";

            return "123";
        }
       
        
        public string GetRegions()
        {
           
            return JsonConvert.SerializeObject(rsl.regionsList);
        }

        //public void UpdateCities(string code)
        //{
        //    Cities cities = new Cities(code);
        //  //  RenderView("SelectCity", cities);
        //}



        public ActionResult RegionList()
        {
            IQueryable regions = StRegions.GetRegions();

            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                    regions, "regionID", "name"), JsonRequestBehavior.AllowGet);

            }
            return View(regions);
        }

        public ActionResult CityList(string regionID)
        {
            int s=0;
            try
            {
                s = Convert.ToInt32(regionID);
            }
            catch { }
            IQueryable cities = stCities.GetCities().Where(x => x.regionID == s);


            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                    cities, "regionID", "name"), JsonRequestBehavior.AllowGet);

            }
            return View(cities);
        }
    }
}