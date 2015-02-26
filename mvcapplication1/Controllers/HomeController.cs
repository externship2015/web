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
        static List<Book> books = new List<Book>();
        static SQLiteDatabaseWorker SQLworker = new SQLiteDatabaseWorker();


        //SQLworker.SetConnect(SQLworker.);
        //rsl = SQLworker.GetCitiesList();
        //SQLworker.CloseConnect();
        static RegionCitiesLists rsl = new RegionCitiesLists();
        static HomeController()
        {
            books.Add(new Book { Id = 1, Name = "Война и мир", Author = "Л. Толстой", Year = 1863, Price = 220 });
            books.Add(new Book { Id = 2, Name = "Отцы и дети", Author = "И. Тургенев", Year = 1862, Price = 195 });
            books.Add(new Book { Id = 3, Name = "Чайка", Author = "А. Чехов", Year = 1895, Price = 158 });
            books.Add(new Book { Id = 4, Name = "Подросток", Author = "Ф. Достоевский", Year = 1875, Price = 210 });

            SQLworker.SetConnect();
            rsl = SQLworker.GetCitiesList();
            SQLworker.CloseConnect();
        }

        public ActionResult Index()
        {
            return View();
        }

        public string GetData()
        {
            //return JsonConvert.SerializeObject(books);
            return JsonConvert.SerializeObject(rsl.regionsList);
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