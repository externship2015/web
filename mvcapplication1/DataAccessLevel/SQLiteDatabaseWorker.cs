using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using System.Data;
using MvcApplication1.DataAccessLevel;


namespace TheTime.DataAccessLevel
{
    /// <summary>
    /// Работает с данными в базе: insert, update, select
    /// </summary>
    class SQLiteDatabaseWorker
    {
      
        #region Done
        
        public SQLiteConnection m_dbConnection;

        /// <summary>
        /// Открывает соединение с базой данных
        /// </summary>
        /// <param name="path">Полный путь до файла БД</param>
        /// <returns></returns>
    
        public void SetConnect()
        {

           // string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Database.db";
           // string path = @"D:\DataBase.db";

            string path = @"C:\C#\Application\SimSoft\web\mvcapplication1\Database.db";
         //   string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Database.db";

            m_dbConnection = new SQLiteConnection(@"Data Source=" + path + ";Version=3;datetimeformat=CurrentCulture");
            m_dbConnection.Open();
        }

        /// <summary>
        /// Закрывает соединение с базой данных
        /// </summary>
        /// <returns></returns>
        public void CloseConnect()
        {
            m_dbConnection.Close();
        }

        /// <summary>
        /// Сохраняет строку с настройками в таблице settings
        /// </summary>
        /// <param name="setObj">объект класса SettingsDataContext</param>       
        public void SaveSettings(SettingsDataContext setObj)
        {
            // проверить наличие настройки с параметрами в базе
            string sql = "SELECT * FROM settings WHERE cityID = '" + setObj.cityID + "' AND sourseID = '" + setObj.sourceID +"';";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            // считаем
            int count = 0;
            foreach (DbDataRecord record in reader)
            {
                count++;
            }
            if (count > 0)
            {
                // update
                sql = @"UPDATE settings SET
                        saveDate = '" + DateTime.Now + "' WHERE cityID = '" + setObj.cityID + "' AND sourseID = '" + setObj.sourceID + "';";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            else
            { 
                // insert
                sql = @"INSERT INTO settings
                                (cityID, sourseID, saveDate)
                           VALUES ('" + setObj.cityID.ToString() + "', '" + setObj.sourceID.ToString() + "', '" + DateTime.Now + "')";
                command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }           
        }

        /// <summary>
        /// Получает настройки из таблицы settings
        /// </summary>
        /// <returns>объект класса SettingsDataContext</returns>
        public SettingsDataContext GetSettings()
        {
            SettingsDataContext ret = new SettingsDataContext();

            string sql = "SELECT * FROM 'settings' order by saveDate DESC  Limit 1;";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                ret.ID = int.Parse(record["ID"].ToString());
                ret.cityID = int.Parse(record["cityID"].ToString());
                ret.sourceID = int.Parse(record["sourseID"].ToString());
                ret.saveDate = DateTime.Parse(record["saveDate"].ToString());
            }
            return ret;
        }


        /// <summary>
        /// Сохраняет / обновляет прогнозы в трех таблицах
        /// </summary>
        /// <param name="forecast">объект класса Forecast</param>
        public void SaveForecast(Forecast forecast)
        {
            // получаем текущий SettingID
            SettingsDataContext set = GetSettings();

            // для каждого  public List<HourlyForecastsDataContext> hourlyList { get; set; } - проверяем и обновляем / переписываем
            #region
            for (int i = 0; i < forecast.hourlyList.Count; i++)
            {
                // проверить наличие такой строки в базе               
                string sql = "SELECT * FROM 'hourly_forecasts' WHERE settingId = '" + set.ID + "' AND periodDate = '" + forecast.hourlyList[i].periodDate.Date.ToString() + "' AND periodTime = '" + forecast.hourlyList[i].periodTime + "';";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                // считаем
                int count = 0;
                foreach (DbDataRecord record in reader)
                {
                    count++;
                }

                if (count > 0)
                {
                    // делаем update
                    sql = @"UPDATE 'hourly_forecasts' SET
                            description = '" + forecast.hourlyList[i].description + "', temperature = '" + forecast.hourlyList[i].temperature + "', windSpeed = '" + forecast.hourlyList[i].windSpeed + "', windDirection = '" + forecast.hourlyList[i].windDirection + "', pressure = '" + forecast.hourlyList[i].pressure + "', hummidity = '" + forecast.hourlyList[i].hummidity + "', symbol ='" + forecast.hourlyList[i].symbol + "' WHERE settingId = '" + set.ID + "' AND periodDate = '" + forecast.hourlyList[i].periodDate.Date.ToString() + "' AND periodTime = '" + forecast.hourlyList[i].periodTime + "';";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    // сохраняем
                    sql = @"INSERT INTO hourly_forecasts
                                (settingID, periodDate, periodTime, description, temperature, windSpeed, windDirection, pressure, hummidity, symbol)
                          VALUES ('" + set.ID + "', '" + forecast.hourlyList[i].periodDate.Date.ToString() + "', '" + forecast.hourlyList[i].periodTime + "', '" + forecast.hourlyList[i].description + "', '" + forecast.hourlyList[i].temperature + "', '" + forecast.hourlyList[i].windSpeed + "', '" + forecast.hourlyList[i].windDirection + "', '" + forecast.hourlyList[i].pressure + "', '" + forecast.hourlyList[i].hummidity + "', '" + forecast.hourlyList[i].symbol + "')";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
            }
            #endregion

            // для каждого public List<DailyForecastsDataContext> dailyList { get; set; } - проверяем и обновляем / переписываем
            #region
            for (int i = 0; i < forecast.dailyList.Count; i++)
            {
                // проверить наличие такой строки в базе               
                string sql = "SELECT * FROM 'daily_forecasts' WHERE settingId = '" + set.ID + "' AND periodDate = '" + forecast.dailyList[i].periodDate.Date.ToString() + "' AND timeOfDay = '" + forecast.dailyList[i].timeOfDay + "';";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                // считаем
                int count = 0;
                foreach (DbDataRecord record in reader)
                {
                    count++;
                }

                if (count > 0)
                {
                    // делаем update
                    sql = @"UPDATE 'daily_forecasts' SET
                            description = '" + forecast.dailyList[i].description + "', temperature = '" + forecast.dailyList[i].temperature + "', windSpeed = '" + forecast.dailyList[i].windSpeed + "', windDirection = '" + forecast.dailyList[i].windDirection + "', pressure = '" + forecast.dailyList[i].pressure + "', hummidity = '" + forecast.dailyList[i].hummidity + "', symbol ='" + forecast.dailyList[i].symbol + "' WHERE settingId = '" + set.ID + "' AND periodDate = '" + forecast.dailyList[i].periodDate.Date.ToString() + "' AND timeOfDay = '" + forecast.dailyList[i].timeOfDay + "';";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    // сохраняем
                    sql = @"INSERT INTO daily_forecasts
                                (settingID, periodDate, timeOfDay, description, temperature, windSpeed, windDirection, pressure, hummidity, symbol)
                          VALUES ('" + set.ID + "', '" + forecast.dailyList[i].periodDate.Date.ToString() + "', '" + forecast.dailyList[i].timeOfDay + "', '" + forecast.dailyList[i].description + "', '" + forecast.dailyList[i].temperature + "', '" + forecast.dailyList[i].windSpeed + "', '" + forecast.dailyList[i].windDirection + "', '" + forecast.dailyList[i].pressure + "', '" + forecast.dailyList[i].hummidity + "', '" + forecast.dailyList[i].symbol + "')";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
            }
            #endregion

            // для каждого public List<TenDaysForecastsDataContext> tenDaysList { get; set; } - проверяем и обновляем / переписываем
            #region
            for (int i = 0; i < forecast.tenDaysList.Count; i++)
            {
                // проверить наличие такой строки в базе               
                string sql = "SELECT * FROM 'ten_days_forecasts' WHERE settingId = '" + set.ID + "' AND periodDate = '" + forecast.tenDaysList[i].periodDate.Date.ToString() + "' AND timeOfDay = '" + forecast.tenDaysList[i].timeOfDay + "';";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                // считаем
                int count = 0;
                foreach (DbDataRecord record in reader)
                {
                    count++;
                }

                if (count > 0)
                {
                    // делаем update
                    sql = @"UPDATE 'ten_days_forecasts' SET
                            temperature = '" + forecast.tenDaysList[i].temperature + "',  symbol ='" + forecast.tenDaysList[i].symbol + "' WHERE settingId = '" + set.ID + "' AND periodDate = '" + forecast.tenDaysList[i].periodDate.Date.ToString() + "' AND timeOfDay = '" + forecast.tenDaysList[i].timeOfDay + "';";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                else
                {
                    // сохраняем
                    sql = @"INSERT INTO ten_days_forecasts
                                (settingID, periodDate, timeOfDay, temperature, symbol)
                          VALUES ('" + set.ID + "', '" + forecast.tenDaysList[i].periodDate.Date.ToString() + "', '" + forecast.tenDaysList[i].timeOfDay + "', '" + forecast.tenDaysList[i].temperature + "', '" + forecast.tenDaysList[i].symbol + "')";
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
            }
            #endregion

        }

        /// <summary>
        /// Получаем прогноз из базы данных
        /// </summary>
        /// <param name="Current">Текущие дата/время в формате DateTime</param>
        /// <returns>объект класса Forecast</returns>
        public Forecast GetForecast(DateTime Current)
        {
            SettingsDataContext sdc = GetSettings(); // получили текущие настройки

            List<string> setID = new List<string>();

            // получаем id всех настроек с похожими параметрами
            string sql = "SELECT * FROM 'settings' WHERE cityID = '" + sdc.cityID + "' AND sourseID = '" + sdc.sourceID + "';";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                setID.Add(record["ID"].ToString());
            }


            Forecast f = new Forecast();

            // вычисляем "текущий час"
            int CurHour = Current.Hour;
            switch (Current.Hour)
            {
                case 1:
                case 2:
                    CurHour = 0;
                    break;
                case 4:
                case 5:
                    CurHour = 3;
                    break;
                case 7:
                case 8:
                    CurHour = 6;
                    break;
                case 10:
                case 11:
                    CurHour = 9;
                    break;
                case 13:
                case 14:
                    CurHour = 12;
                    break;
                case 16:
                case 17:
                    CurHour = 15;
                    break;
                case 19:
                case 20:
                    CurHour = 18;
                    break;
                case 22:
                case 23:
                    CurHour = 21;
                    break;
            }

            

            // получаем прогноз на текущий момент
            sql = "SELECT * FROM 'hourly_forecasts' WHERE settingId = '" + sdc.ID + "' AND periodDate = '" + Current.Date.ToString() + "' AND periodTime = '" + CurHour + "' LIMIT 1;";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                f.curWeather.description = record["description"].ToString();
                f.curWeather.hummidity = record["hummidity"].ToString();
                f.curWeather.pressure = record["pressure"].ToString();
                f.curWeather.symbol = record["symbol"].ToString();
                f.curWeather.temperature = record["temperature"].ToString();
                f.curWeather.windDirection = record["windDirection"].ToString();
                f.curWeather.windSpeed = record["windSpeed"].ToString();
            }

            // получаем прогноз на день
            

            sql = "SELECT * FROM 'daily_forecasts' WHERE settingId = '" + sdc.ID + "' AND (periodDate = '" + Current.Date.ToString() + "' OR periodDate = '" + Current.AddDays(1).Date.ToString() + "' OR periodDate = '" + Current.AddDays(2).Date.ToString() + "' OR periodDate = '" + Current.AddDays(3).Date.ToString() + "' OR periodDate = '" + Current.AddDays(4).Date.ToString() + "' OR periodDate = '" + Current.AddDays(5).Date.ToString() + "' OR periodDate = '" + Current.AddDays(6).Date.ToString() + "' OR periodDate = '" + Current.AddDays(7).Date.ToString() + "' OR periodDate = '" + Current.AddDays(8).Date.ToString() + "' OR periodDate = '" + Current.AddDays(9).Date.ToString() + "' );";
            // должны получить 4 записи
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                DailyForecastsDataContext context = new DailyForecastsDataContext();
                context.description = record["description"].ToString();
                context.hummidity = record["hummidity"].ToString();
                context.periodDate = DateTime.Parse(record["periodDate"].ToString());
                context.pressure = record["pressure"].ToString();
                context.settingID = int.Parse(record["settingID"].ToString());
                context.symbol = record["symbol"].ToString();
                context.temperature = record["temperature"].ToString();
                context.timeOfDay = record["timeOfDay"].ToString();
                context.windDirection = record["windDirection"].ToString();
                context.windSpeed = record["windSpeed"].ToString();
                f.dailyList.Add(context);
            }

            // получаем прогноз на 10 дней -> 10*2 = 20 записей
            sql = "SELECT * FROM 'ten_days_forecasts' WHERE settingId = '" + sdc.ID + "' AND (periodDate = '" + Current.Date.ToString() + "' OR periodDate = '" + Current.AddDays(1).Date.ToString() + "' OR periodDate = '" + Current.AddDays(2).Date.ToString() + "' OR periodDate = '" + Current.AddDays(3).Date.ToString() + "' OR periodDate = '" + Current.AddDays(4).Date.ToString() + "' OR periodDate = '" + Current.AddDays(5).Date.ToString() + "' OR periodDate = '" + Current.AddDays(6).Date.ToString() + "' OR periodDate = '" + Current.AddDays(7).Date.ToString() + "' OR periodDate = '" + Current.AddDays(8).Date.ToString() + "' OR periodDate = '" + Current.AddDays(9).Date.ToString() + "' );";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                TenDaysForecastsDataContext context = new TenDaysForecastsDataContext();
                context.periodDate = DateTime.Parse(record["periodDate"].ToString());
                context.settingID = int.Parse(record["settingID"].ToString());
                context.symbol = record["symbol"].ToString();
                context.temperature = record["temperature"].ToString();
                context.timeOfDay = record["timeOfDay"].ToString();
                f.tenDaysList.Add(context);

            }

            // получаем почасовой прогноз на день - должны получить 9 записей
            sql = "SELECT * FROM 'hourly_forecasts' WHERE settingId = '" + sdc.ID + "' AND periodDate = '" + Current.Date.ToString() + "';";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                HourlyForecastsDataContext context = new HourlyForecastsDataContext();
                context.description = record["description"].ToString();
                context.hummidity = record["hummidity"].ToString();
                context.periodDate = Current;
                context.pressure = record["pressure"].ToString();
                context.settingID = int.Parse(record["settingID"].ToString());
                context.symbol = record["symbol"].ToString();
                context.temperature = record["temperature"].ToString();
                context.periodTime = int.Parse(record["periodTime"].ToString());
                context.windDirection = record["windDirection"].ToString();
                context.windSpeed = record["windSpeed"].ToString();
                f.hourlyList.Add(context);
            }

            return f;
        }

        /// <summary>
        /// Заполняет таблицы с городами и регионами из xml-ки яндекса
        /// </summary>
        /// <returns></returns>
        public void FillCitiesAndRegionsTables(RegionCitiesLists listRC)
        {
            string sql = "";
            // записываем все регионы в базу
            for (int i = 0; i < listRC.regionsList.Count; i++)
            {
                sql = @"INSERT INTO regions
                                (regionID, name)
                          VALUES ('" + listRC.regionsList[i].regionID + "', '" + listRC.regionsList[i].name + "');";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }

            // записываем все города в базу
            for (int i = 0; i < listRC.citiesList.Count; i++)
            {
                sql = @"INSERT INTO cities
                                (name, regionID, yandexID, owmID)
                          VALUES ('" + listRC.citiesList[i].name + "', '" + listRC.citiesList[i].regionID + "', '" + listRC.citiesList[i].yandexID + "', '" + listRC.citiesList[i].owmID + "');";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
        }    
        #endregion

        #region ToDo

        /// <summary>
        /// Получает список городов из базы
        /// </summary>
        /// <returns>объект класса RegionCitiesLists</returns>
        public RegionCitiesLists GetCitiesList()
        {
            RegionCitiesLists listRC = new RegionCitiesLists();
            string sql = "SELECT * FROM 'cities' ;";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                listRC.citiesList.Add(new CitiesDataContext
                {
                    name = record["name"].ToString(),
                    regionID = int.Parse(record["regionID"].ToString()),
                    yandexID = int.Parse(record["yandexID"].ToString()),
                    owmID = record["owmID"].ToString()
                });
            }

            sql = "SELECT * FROM 'regions' order by name asc;";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                listRC.regionsList.Add(new RegionsDataContext
                {
                    regionID = int.Parse(record["regionID"].ToString()),
                    name = record["name"].ToString()
                });
            }

            return listRC;
        }
          

        public CitiesDataContext GetCityByYaId(string yaid)
        {
            CitiesDataContext context = new CitiesDataContext();

            string sql = "select * from cities where yandexID = '"+yaid+"'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                context.name = record["name"].ToString();
                context.owmID = record["owmID"].ToString();
                context.regionID = int.Parse(record["regionID"].ToString());
                context.yandexID = int.Parse(record["yandexID"].ToString());
                
            }

            return context;
        }
        #endregion



        public List<WebTable> GetWebTable(string regID, string CitName, DateTime start, DateTime end)
        {           

            // получаем id города
            int cityID = 0;

            List<WebTable> table = new List<WebTable>();
            SettingsDataContext owmSet = new SettingsDataContext();
            SettingsDataContext yaSet = new SettingsDataContext();

            // достаем id нужного города из базы
            string sql = "select * from cities where name = '"+CitName+"' and regionID = '"+regID+"'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                cityID = int.Parse(record["yandexID"].ToString());
                
            }

            // получаем строки настроек с нужным городом
            sql = "select * from settings where cityID = '" + cityID.ToString() + "' and sourseID = '1' Limit 1";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                owmSet.cityID = int.Parse(record["cityID"].ToString());
                owmSet.ID = int.Parse(record["ID"].ToString());
                owmSet.sourceID = 1;   
            }

            sql = "select * from settings where cityID = '" + cityID.ToString() + "' and sourseID = '2' Limit 1";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                yaSet.cityID = int.Parse(record["cityID"].ToString());
                yaSet.ID = int.Parse(record["ID"].ToString());
                yaSet.sourceID = 1;
            }

            // получаем данные
            sql = "SELECT * FROM ten_days_forecasts where (periodDate > date('" + start.Date.ToString("yyyy-MM-dd") + "') and periodDate < date('" + end.Date.ToString("yyyy-MM-dd") + "')) and (settingID ='" + yaSet.ID.ToString() + "' or settingID = '" + yaSet.ID.ToString() + "')";
            //sql = "select * from ten_days_forecasts where (settingId = '1' or settingID = '2') order by periodDate ASC";
            
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            int count = 0;

            List<TenDaysForecastsDataContext> temp = new List<TenDaysForecastsDataContext>();

            foreach (DbDataRecord record in reader)
            {
                DateTime date = DateTime.Parse(reader["periodDate"].ToString());
                count++;
                int id = int.Parse(reader["ID"].ToString());
                int settingID = int.Parse(reader["settingID"].ToString());
                DateTime period = DateTime.Parse(reader["periodDate"].ToString());
                string timeOfDay = reader["timeOfDay"].ToString();
                string temperature = reader["temperature"].ToString();
                string symbol = reader["symbol"].ToString();

                temp.Add(new TenDaysForecastsDataContext { periodDate = period, settingID = settingID, symbol = symbol, temperature = temperature, timeOfDay = timeOfDay });
            }

            // из полученного списка выбираем нужный диапазон дат           
           // List<TenDaysForecastsDataContext> temp = (from t in _temp where (t.periodDate > start && t.periodDate < end) select t).ToList();


            // формируем список нужного вида
            for (int i = 0; i < temp.Count; i++ )
            {
                // проверяем, есть ли в table запись с такой же датой                
                if (table.Exists(x => x.date == temp[i].periodDate.Date.ToString()))
                { 
                    // если есть, то обновляем ее                  
                    int ind = table.IndexOf(table.First(s => s.date == temp[i].periodDate.Date.ToString()));

                    if (temp[i].settingID == owmSet.ID)
                    {
                        if (temp[i].timeOfDay.ToLower() == "ночь")
                        {
                            table[ind].owmSymbolNight = temp[i].symbol.Trim('_');

                            table[ind].owmTempNight = int.Parse(temp[i].temperature);
                        }
                        else
                        {
                            table[ind].owmSymbolDay = temp[i].symbol.Trim('_'); ;
                            table[ind].owmTempDay = int.Parse(temp[i].temperature);
                        }
                    }
                    else
                    {
                        if (temp[i].timeOfDay.ToLower() == "ночь")
                        {
                            table[ind].yaSymbolNight = temp[i].symbol;
                            table[ind].yaTempNight = int.Parse(temp[i].temperature);
                        }
                        else
                        {
                            table[ind].yaSymbolDay = temp[i].symbol;
                            table[ind].yaTempDay = int.Parse(temp[i].temperature);
                        }
                    }
                }
                else
                {
                    // добавляем новую
                    if (temp[i].settingID == owmSet.ID)
                    {
                        if (temp[i].timeOfDay.ToLower() == "ночь")
                            table.Add(new WebTable { date = temp[i].periodDate.Date.ToString(), owmSymbolNight = temp[i].symbol.Trim('_'), owmTempNight = int.Parse(temp[i].temperature) });
                        else
                            table.Add(new WebTable { date = temp[i].periodDate.Date.ToString(), owmSymbolDay = temp[i].symbol.Trim('_'), owmTempDay = int.Parse(temp[i].temperature) });
                    }
                    else
                    {
                        if (temp[i].timeOfDay.ToLower() == "ночь")
                            table.Add(new WebTable { date = temp[i].periodDate.Date.ToString(), yaSymbolNight = temp[i].symbol, yaTempNight = int.Parse(temp[i].temperature) });
                        else
                            table.Add(new WebTable { date = temp[i].periodDate.Date.ToString(), yaSymbolDay = temp[i].symbol, yaTempDay = int.Parse(temp[i].temperature) });
                  
                    }

                }

            }
                return table;
        }

        public List<WebTable> GetWebTable(string regID, string CitName, DateTime start, DateTime end, int onPageCount, int page, string sort = "DESC")
        {
            // получаем id города
            int cityID = 0;

            List<WebTable> table = new List<WebTable>();
            SettingsDataContext owmSet = new SettingsDataContext();
            SettingsDataContext yaSet = new SettingsDataContext();

            // достаем id нужного города из базы
            string sql = "select * from cities where name = '" + CitName + "' and regionID = '" + regID + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                cityID = int.Parse(record["yandexID"].ToString());

            }

            // получаем строки настроек с нужным городом
            sql = "select * from settings where cityID = '" + cityID.ToString() + "' and sourseID = '1' Limit 1";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                owmSet.cityID = int.Parse(record["cityID"].ToString());
                owmSet.ID = int.Parse(record["ID"].ToString());
                owmSet.sourceID = 1;
            }

            sql = "select * from settings where cityID = '" + cityID.ToString() + "' and sourseID = '2' Limit 1";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                yaSet.cityID = int.Parse(record["cityID"].ToString());
                yaSet.ID = int.Parse(record["ID"].ToString());
                yaSet.sourceID = 1;
            }
            
            // получаем нужные даты
            sql = "SELECT Distinct(periodDate) FROM daily_forecasts where (periodDate >= date('" + start.Date.ToString("yyyy-MM-dd") + "') and periodDate <= date('" + end.Date.ToString("yyyy-MM-dd") + "')) and (settingID ='" + yaSet.ID.ToString() + "' or settingID = '" + owmSet.ID.ToString() + "') order by periodDate "+sort+" Limit "+((page-1)*onPageCount).ToString()+","+onPageCount.ToString()+";";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();
            List<DateTime> dates = new List<DateTime>();
            foreach (DbDataRecord record in reader)
            {
                dates.Add(DateTime.Parse(record["periodDate"].ToString()));
            }

            if (dates.Count > 0)
            {
                // определяем start и end

                DateTime start1 = dates[0];
                DateTime end1 = dates[dates.Count - 1];
                if (start1 > end1)
                { 
                    // меняем их местами
                    DateTime t = start1;
                    start1 = end1;
                    end1 = t;
                }

                // получаем данные - в диапазоне первой и последней из выбранных дат
                sql = "SELECT * FROM daily_forecasts where (periodDate >= date('" + start1.Date.ToString("yyyy-MM-dd") + "') and periodDate <= date('" + end1.Date.ToString("yyyy-MM-dd") + "')) and (settingID ='" + yaSet.ID.ToString() + "' or settingID = '" + owmSet.ID.ToString() + "') order by periodDate " + sort;
                //sql = "select * from ten_days_forecasts where (settingId = '1' or settingID = '2') order by periodDate ASC";

                command = new SQLiteCommand(sql, m_dbConnection);
                reader = command.ExecuteReader();

                int count = 0;

                List<DailyForecastsDataContext> temp = new List<DailyForecastsDataContext>();

                foreach (DbDataRecord record in reader)
                {
                    DateTime date = DateTime.Parse(reader["periodDate"].ToString());
                    count++;
                    int id = int.Parse(reader["ID"].ToString());
                    int settingID = int.Parse(reader["settingID"].ToString());
                    DateTime period = DateTime.Parse(reader["periodDate"].ToString());
                    string timeOfDay = reader["timeOfDay"].ToString();
                    string temperature = reader["temperature"].ToString();
                    string symbol = reader["symbol"].ToString();
                    string windSpeed = reader["windSpeed"].ToString();
                    string pressure = reader["pressure"].ToString();
                    string hummidity = reader["hummidity"].ToString();

                    temp.Add(new DailyForecastsDataContext { periodDate = period, settingID = settingID, symbol = symbol, temperature = temperature, timeOfDay = timeOfDay, hummidity = hummidity, pressure = pressure, windSpeed = windSpeed });
                }

                // формируем список нужного вида
                for (int i = 0; i < temp.Count; i++)
                {
                    // проверяем, есть ли в table запись с такой же датой                
                    if (table.Exists(x => x.date == temp[i].periodDate.Date.ToString("dd.MM.yyyy")))
                    {
                        // если есть, то обновляем ее                  
                        int ind = table.IndexOf(table.First(s => s.date == temp[i].periodDate.Date.ToString("dd.MM.yyyy")));

                        if (temp[i].settingID == owmSet.ID)
                        {
                            if (temp[i].timeOfDay.ToLower() == "ночь")
                            {
                                table[ind].owmSymbolNight = temp[i].symbol.Trim('_');

                                table[ind].owmTempNight = int.Parse(temp[i].temperature);
                                table[ind].owmHummidityNight = temp[i].hummidity;
                                table[ind].owmPressureNight = temp[i].pressure;
                                table[ind].owmWindSpeedNight = temp[i].windSpeed;

                            }
                            if (temp[i].timeOfDay.ToLower() == "день")
                            {
                                table[ind].owmSymbolDay = temp[i].symbol.Trim('_'); ;
                                table[ind].owmTempDay = int.Parse(temp[i].temperature);

                                table[ind].owmHummidityDay = temp[i].hummidity;
                                table[ind].owmPressureDay = temp[i].pressure;
                                table[ind].owmWindSpeedDay = temp[i].windSpeed;
                            }
                        }
                        else
                        {
                            if (temp[i].timeOfDay.ToLower() == "ночь")
                            {
                                table[ind].yaSymbolNight = temp[i].symbol;
                                table[ind].yaTempNight = int.Parse(temp[i].temperature);

                                table[ind].yaHummidityNight = temp[i].hummidity;
                                table[ind].yaPressureNight = temp[i].pressure;
                                table[ind].yaWindSpeedNight = temp[i].windSpeed;
                            }
                            if (temp[i].timeOfDay.ToLower() == "день")
                            {
                                table[ind].yaSymbolDay = temp[i].symbol;
                                table[ind].yaTempDay = int.Parse(temp[i].temperature);

                                table[ind].yaHummidityDay = temp[i].hummidity;
                                table[ind].yaPressureDay = temp[i].pressure;
                                table[ind].yaWindSpeedDay = temp[i].windSpeed;
                            }
                        }
                    }
                    else
                    {
                        // добавляем новую
                        if (temp[i].settingID == owmSet.ID)
                        {
                            if (temp[i].timeOfDay.ToLower() == "ночь")
                                table.Add(new WebTable { date = temp[i].periodDate.Date.ToString("dd.MM.yyyy"), owmSymbolNight = temp[i].symbol.Trim('_'), owmTempNight = int.Parse(temp[i].temperature), owmHummidityNight = temp[i].hummidity, owmPressureNight = temp[i].pressure, owmWindSpeedNight = temp[i].windSpeed });
                            if (temp[i].timeOfDay.ToLower() == "день")
                                table.Add(new WebTable { date = temp[i].periodDate.Date.ToString("dd.MM.yyyy"), owmSymbolDay = temp[i].symbol.Trim('_'), owmTempDay = int.Parse(temp[i].temperature), owmHummidityDay = temp[i].hummidity, owmPressureDay = temp[i].pressure, owmWindSpeedDay = temp[i].windSpeed });
                        }
                        else
                        {
                            if (temp[i].timeOfDay.ToLower() == "ночь")
                                table.Add(new WebTable { date = temp[i].periodDate.Date.ToString("dd.MM.yyyy"), yaSymbolNight = temp[i].symbol, yaTempNight = int.Parse(temp[i].temperature), yaHummidityNight = temp[i].hummidity, yaPressureNight = temp[i].pressure, yaWindSpeedNight = temp[i].windSpeed });
                            if (temp[i].timeOfDay.ToLower() == "день")
                                table.Add(new WebTable { date = temp[i].periodDate.Date.ToString("dd.MM.yyyy"), yaSymbolDay = temp[i].symbol, yaTempDay = int.Parse(temp[i].temperature), yaHummidityDay = temp[i].hummidity, yaPressureDay = temp[i].pressure, yaWindSpeedDay = temp[i].windSpeed });

                        }

                    }

                }
            }
            return table;
        }

        public int GetTotalCnt(string regID, string CitName, DateTime start, DateTime end)
        {
            int cityID = 0;
            List<WebTable> table = new List<WebTable>();
            SettingsDataContext owmSet = new SettingsDataContext();
            SettingsDataContext yaSet = new SettingsDataContext();

            // достаем id нужного города из базы
            string sql = "select * from cities where name = '" + CitName + "' and regionID = '" + regID + "'";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            foreach (DbDataRecord record in reader)
            {
                cityID = int.Parse(record["yandexID"].ToString());

            }

            // получаем строки настроек с нужным городом
            sql = "select * from settings where cityID = '" + cityID.ToString() + "' and sourseID = '1' Limit 1";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                owmSet.cityID = int.Parse(record["cityID"].ToString());
                owmSet.ID = int.Parse(record["ID"].ToString());
                owmSet.sourceID = 1;
            }

            sql = "select * from settings where cityID = '" + cityID.ToString() + "' and sourseID = '2' Limit 1";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();

            foreach (DbDataRecord record in reader)
            {
                yaSet.cityID = int.Parse(record["cityID"].ToString());
                yaSet.ID = int.Parse(record["ID"].ToString());
                yaSet.sourceID = 1;
            }

            // получаем нужные даты
            sql = "SELECT Distinct(periodDate) FROM daily_forecasts where (periodDate >= date('" + start.Date.ToString("yyyy-MM-dd") + "') and periodDate <= date('" + end.Date.ToString("yyyy-MM-dd") + "')) and (settingID ='" + yaSet.ID.ToString() + "' or settingID = '" + owmSet.ID.ToString() + "') order by periodDate asc";
            command = new SQLiteCommand(sql, m_dbConnection);
            reader = command.ExecuteReader();
            int cnt = 0;
            foreach (DbDataRecord record in reader)
            {
                cnt++;
            }

            return cnt;
        }

    }
}
