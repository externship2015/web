using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MvcApplication1.Models;
using MvcApplication1.DataAccessLevel;
using TheTime.DataAccessLevel;
using MvcApplication1.Helpers;

namespace JQGridApp.Controllers
{
    public class HomeController : Controller
    {       
        static RegionCitiesLists rsl = new RegionCitiesLists();
        static HomeController()
        {
        }

        public ActionResult Index(int page = 1)
        {
            return View();
            /*int pageSize = 3; // количество объектов на страницу
            IEnumerable<Phone> phonesPerPages = phones.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = phones.Count };
            IndexViewModel ivm = new IndexViewModel { PageInfo = pageInfo, Phones = phonesPerPages };
            return View(ivm);*/
        }

        public string GetData(List<String> values) 
        { 
            /* приходит массив строк
             * values[0] - с - дата
             * values[1] - по - дата
             * values[2] - ид региона - 81
             * values[3] - название города - Ульяновск
             * values[4] - номер страницы
             */
            SQLiteDatabaseWorker SQLworker = new SQLiteDatabaseWorker();
            List<WebTable> table = new List<WebTable>();
            SQLworker.SetConnect();
            table = SQLworker.GetWebTable(values[2], values[3], DateTime.Now.AddDays(-10), DateTime.Now.AddDays(20), 10, int.Parse(values[4]));
            SQLworker.CloseConnect();

            int page = 1;
            int pageSize = 3;
            IEnumerable<WebTable> phonesPerPages = table.Skip((page - 1) * pageSize).Take(pageSize);
           // PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = phones.Count };
            return JsonConvert.SerializeObject(table);
        }

        public MvcHtmlString getPageData()
        {
            PageInfo pageInfo = new PageInfo { PageNumber = 1, PageSize = 3, TotalItems = 10 };

            // @Html.PageLinks(Model.PageInfo, x => Url.Action("Index",new { page = x}))

            PagingHelpers ph = new PagingHelpers();
            MvcHtmlString links = ph.PageLinks(pageInfo);
            return links;
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