using MvcApplication1.DataAccessLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace MvcApplication1.YandexMethods
{
    public class YandexMethods
    {
        //public Forecast GetYandexForecast(string id, Forecast fc)
        //{
        //    string city = id;
        //    string f_date = "";
        //    string time_of_day = "";
        //    string region = id;
        //    string temperature = "";
        //    string hummidity = "";
        //    string pressure = "";
        //    string wind_direction = "";
        //    string wind_speed = "";
        //    string symbol = "";
        //    string description = "";


        //    XmlDocument xDoc = new XmlDocument();

        //    xDoc.Load("http://export.yandex.ru/weather-ng/forecasts/" + id + ".xml");

        //    CurrentWeather curWeather = new CurrentWeather();
        //    List<HourlyForecastsDataContext> ListHours = new List<HourlyForecastsDataContext>();
        //    List<DailyForecastsDataContext> ListDaily = new List<DailyForecastsDataContext>();
        //    List<TenDaysForecastsDataContext> ListTenDays = new List<TenDaysForecastsDataContext>();
        //    foreach (XmlNode node in xDoc.DocumentElement)
        //    {
        //        if (node.Name == "fact")
        //        {
        //            string pthPic = ParseXMLfact(node.OuterXml, "image-v3 type=\"mono\"").Replace("-", "0");
        //            pthPic = pthPic.Replace("+", "1");
        //            curWeather.temperature = ParseXMLfact(node.OuterXml, "temperature");
        //            curWeather.hummidity = ParseXMLfact(node.OuterXml, "humidity");
        //            curWeather.pressure = ParseXMLfact(node.OuterXml, "pressure");
        //            curWeather.description = ParseXMLfact(node.OuterXml, "weather_type");
        //            curWeather.windDirection = ParseXMLfact(node.OuterXml, "wind_direction");
        //            curWeather.windSpeed = ParseXMLfact(node.OuterXml, "wind_speed");
        //            curWeather.symbol = pthPic;

        //        }

        //        if (node.Name == "day")
        //        {

        //            f_date = ParseXMLString(node.OuterXml, "day date=\"");
        //            foreach (XmlNode node2 in node.ChildNodes)
        //            {
        //                if (node2.Name == "day_part")
        //                {
        //                    time_of_day = ParseXMLString(node2.OuterXml, "typeid=\"");
        //                    foreach (XmlNode node3 in node2.ChildNodes)
        //                    {
        //                        if (node3.Name == "temperature_from")
        //                            temperature = node3.InnerText;
        //                        //if (node3.Name == "temperature_to")
        //                        //    temperature += node3.InnerText;
        //                        if (node3.Name == "image-v3")
        //                            symbol = node3.InnerText;
        //                        if (node3.Name == "weather_type")
        //                            description = node3.InnerText;
        //                        if (node3.Name == "wind_direction")
        //                            wind_direction = node3.InnerText;
        //                        if (node3.Name == "wind_speed")
        //                            wind_speed = node3.InnerText;
        //                        if (node3.Name == "humidity")
        //                            hummidity = node3.InnerText;
        //                        if (node3.Name == "pressure")
        //                            pressure = node3.InnerText;
        //                        if (node3.Name == "image-v3")
        //                            symbol = node3.InnerText;
        //                    }
        //                    if (time_of_day == "1" || time_of_day == "2" || time_of_day == "3" || time_of_day == "4")
        //                    {
        //                        ListDaily.Add(new DailyForecastsDataContext
        //                        {
        //                            description = description,
        //                            hummidity = hummidity,
        //                            pressure = pressure,
        //                            windSpeed = wind_speed,
        //                            periodDate = Convert.ToDateTime(f_date).Date,
        //                            symbol = symbol,
        //                            temperature = temperature,
        //                            timeOfDay = ParsePartOfDay(time_of_day),
        //                            windDirection = ParseWind(wind_direction),
        //                        });
        //                    }
        //                    if (time_of_day == "5" || time_of_day == "6")
        //                    {
        //                        ListTenDays.Add(new TenDaysForecastsDataContext
        //                        {
        //                            periodDate = Convert.ToDateTime(f_date).Date,
        //                            symbol = symbol,
        //                            temperature = temperature,
        //                            timeOfDay = ParsePartOfDay(time_of_day)
        //                        });
        //                    }
        //                }
        //                if (node2.Name == "hour")
        //                {
        //                    time_of_day = ParseXMLString(node2.OuterXml, "at=\"");
        //                    foreach (XmlNode node3 in node2.ChildNodes)
        //                    {
        //                        if (node3.Name == "temperature")
        //                            temperature = node3.InnerText;
        //                        if (node3.Name == "image-v3")
        //                            symbol = node3.InnerText;
        //                    }
        //                    if (time_of_day == "0" || time_of_day == "3" || time_of_day == "6" || time_of_day == "9" || time_of_day == "12" || time_of_day == "15" || time_of_day == "18" || time_of_day == "21")
        //                    {
        //                        ListHours.Add(new HourlyForecastsDataContext
        //                        {
        //                            periodDate = Convert.ToDateTime(f_date).Date,
        //                            symbol = symbol,
        //                            temperature = temperature,
        //                            periodTime = Convert.ToInt16(time_of_day)
        //                        });
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    fc.curWeather = curWeather;
        //    fc.hourlyList = ListHours;
        //    fc.tenDaysList = ListTenDays;
        //    fc.dailyList = ListDaily;
        //    return fc;
        //}
        private string ParseXMLString(string bigStr, string litStr)
        {
            string ret = "";
            int i = bigStr.IndexOf(litStr) + litStr.Length;
            while (bigStr[i] != '\"')
            {
                ret += bigStr[i];
                i++;
            }
            return ret;
        }
        private string ParseWind(string str)
        {
            string ret = "";
            if (str == "n")
                return "С";
            if (str == "w")
                return "З";
            if (str == "e")
                return "В";
            if (str == "s")
                return "Ю";
            if (str == "nw")
                return "СЗ";
            if (str == "ne")
                return "СВ";
            if (str == "sw")
                return "ЮЗ";
            if (str == "se")
                return "ЮВ";
            return ret;
        }
        private string ParsePartOfDay(string str)
        {
            string ret = "";
            if (str == "1")
                return "Утро";
            if (str == "2")
                return "День";
            if (str == "3")
                return "Вечер";
            if (str == "4")
                return "Ночь";
            if (str == "5")
                return "День";
            if (str == "6")
                return "Ночь";
            return ret;
        }


        private string ParseXMLfact(string bigStr, string litStr)
        {
            string ret = "";
            int i = bigStr.IndexOf(litStr) + litStr.Length;
            while (bigStr[i] != '<')
            {
                if (bigStr[i] == '>')
                {
                    while (bigStr[i + 1] != '<')
                    {
                        i++;
                        ret += bigStr[i];
                    }
                    return ret;
                }
                else
                {
                    i++;
                }

            }

            return ret;
        }

        //Возвращает регионы
        public RegionCitiesLists GetRegionCitiesList()
        {
            RegionCitiesLists reglist = new RegionCitiesLists();

            List<RegionsDataContext> regionsList = new List<RegionsDataContext>();
            List<CitiesDataContext> citiesList = new List<CitiesDataContext>();
            List<Cities1> ListOfCities = new List<Cities1>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("https://pogoda.yandex.ru/static/cities.xml");

            foreach (XmlNode node in xDoc.DocumentElement)
            {
                foreach (XmlNode node2 in node.ChildNodes)
                {
                    if (ParseXMLString(node2.OuterXml, "country=\"") == "Россия")
                    {
                        ListOfCities.Add(new Cities1
                        {
                            citName = node2.InnerText,
                            id = ParseXMLString(node2.OuterXml, "id=\""),
                            part = ParseXMLString(node2.OuterXml, "part=\""),
                            region = ParseXMLString(node2.OuterXml, "region=\"")
                        });
                    }
                }
            }
            var custs = (from customer in ListOfCities
                         select new { customer.part }).Distinct();


            int k = 1;
            foreach (var item in custs)
            {
                regionsList.Add(new RegionsDataContext
                {
                    name = item.part,
                    regionID = k
                });
                var custs2 = (from customer in ListOfCities
                              select new { customer.citName, customer.part, customer.id }).Where(t => t.part.ToString() == item.part.ToString());
                foreach (var item2 in custs2)
                {
                    citiesList.Add(new CitiesDataContext
                {
                    name = item2.citName,
                    regionID = k,
                    yandexID = int.Parse(item2.id)

                });
                }
                k++;
            }
            reglist.citiesList = citiesList;
            reglist.regionsList = regionsList;
            return reglist;

        }

    }
}
public class Cities1
{
    public string id { get; set; }
    public string region { get; set; }
    public string country { get; set; }
    public string part { get; set; }
    public string citName { get; set; }
}



