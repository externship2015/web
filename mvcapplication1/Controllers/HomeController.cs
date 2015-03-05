﻿using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using MvcApplication1.Models;
using MvcApplication1.DataAccessLevel;
using TheTime.DataAccessLevel;
//using MvcApplication1.Helpers;

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
        }

        //public string GetData(PagingRequest request)
        //{
        //    int a = 0;
        //    return "1323";
        //}

        public string GetTotalCNT(List<String> values)
        {
            string formatsrc = "dd-MM-yyyy";
            DateTime date1 = DateTime.MinValue;
            DateTime date2 = DateTime.MaxValue;
            try
            {
                date1 = DateTime.ParseExact(values[0], formatsrc, CultureInfo.InvariantCulture);
                date2 = DateTime.ParseExact(values[1], formatsrc, CultureInfo.InvariantCulture);
            }
            catch
            { }


            SQLiteDatabaseWorker SQLworker = new SQLiteDatabaseWorker();
            SQLworker.SetConnect();
            int cnt = SQLworker.GetTotalCnt(values[2], values[3], date1, date2);
            SQLworker.CloseConnect();
            decimal dd = decimal.Parse(cnt.ToString()) / decimal.Parse(values[5].ToString());
            var cnt2 = Math.Ceiling(dd);

            return cnt2.ToString();
        }

        public string GetData(List<String> values)
        {
            /* приходит массив строк
             * values[0] - с - дата
             * values[1] - по - дата
             * values[2] - ид региона - 81
             * values[3] - название города - Ульяновск
             * values[4] - номер страницы
             * values[5] - количество элементов на странице
             * values[6] - порядок сортировки
             */


            string formatsrc = "dd-MM-yyyy";
            DateTime date1 = DateTime.MinValue;
            DateTime date2 = DateTime.MaxValue;
            try
            {
                date1 = DateTime.ParseExact(values[0], formatsrc, CultureInfo.InvariantCulture);
                date2 = DateTime.ParseExact(values[1], formatsrc, CultureInfo.InvariantCulture);
            }
            catch
            { }

            SQLiteDatabaseWorker SQLworker = new SQLiteDatabaseWorker();
            List<WebTable> table = new List<WebTable>();
            SQLworker.SetConnect();
            table = SQLworker.GetWebTable(values[2], values[3], date1, date2, int.Parse(values[5]), int.Parse(values[4]), values[6]);
            SQLworker.CloseConnect();

            int page = 1;
            int pageSize = 3;
            IEnumerable<WebTable> phonesPerPages = table.Skip((page - 1) * pageSize).Take(pageSize);
            // PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = phones.Count };
            return JsonConvert.SerializeObject(table);
        }

            
        public string GetRegions()
        {

            return JsonConvert.SerializeObject(rsl.regionsList.OrderBy(x => x.name));
        }

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
            int s = 0;
            try
            {
                s = Convert.ToInt32(regionID);
            }
            catch { }

            IQueryable cities = stCities.GetCities().Where(x => x.regionID == s).OrderBy(x => x.name);

            if (HttpContext.Request.IsAjaxRequest())
            {
                return Json(new SelectList(
                    cities, "regionID", "name"), JsonRequestBehavior.AllowGet);

            }
            return View(cities);
        }
    }
}