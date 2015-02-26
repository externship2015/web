using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using System.Data;

namespace TheTime.DataAccessLevel
{
    /// <summary>
    /// Создает файл базы данных и начальную структуру таблиц
    /// </summary>
    class SQLiteDatabaseCreator
    {
        /// <summary>
        /// Создает файл базы данных рядом с exe файлом программы
        /// </summary>
        /// <returns>Вернет true, если файл создан, иначе false</returns>
        public bool CreateDataBaseFile(string databaseName)
        {            
            SQLiteConnection.CreateFile(databaseName);
            if (File.Exists(databaseName))
                return true;
            else
                return false;
        }
        /// <summary>
        /// Создает структуру таблиц в БД
        /// </summary>
        /// <returns> true если все хорошо, иначе false</returns>
        public bool CreateTables(string path)
        {
            try
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source="+path+";Version=3;");
                m_dbConnection.Open();

                string sql = @"CREATE TABLE [regions] (
                                        [ID]        integer         PRIMARY KEY NOT NULL,
                                        [regionID]  integer         NOT NULL,
                                        [name]      varchar(100)    NOT NULL
                                        );";

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE [cities] (
                                    [ID]          integer            PRIMARY KEY NOT NULL,
                                    [name]        varchar(100)       NOT NULL,
                                    [regionID]    integer            NULL, 
                                    [yandexID]    integer            NULL,
                                    [owmID]       varchar(100)       NULL
                                    );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE [settings] (
                                    [ID]          integer         PRIMARY KEY NOT NULL,
                                    [cityID]      integer         NOT NULL,
                                    [sourseID]    integer         NULL, 
                                    [saveDate]    date            NULL                                   
                                    );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE [hourly_forecasts] (
                                    [ID]             integer          PRIMARY KEY NOT NULL,
                                    [settingID]      integer          NOT NULL,
                                    [periodDate]     varchar(200)     NOT NULL, 
                                    [periodTime]     integer          NOT NULL,
                                    [description]    varchar(200)     NOT NULL,
                                    [temperature]    varchar(10)      NOT NULL,
                                    [windSpeed]      varchar(10)      NULL,
                                    [windDirection]  varchar(5)       NULL,
                                    [pressure]       varchar(10)      NULL,
                                    [hummidity]      varchar(10)      NULL, 
                                    [symbol]         varchar(20)      NOT NULL                             
                                    );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE [daily_forecasts] (
                                    [ID]             integer          PRIMARY KEY NOT NULL,
                                    [settingID]      integer          NOT NULL,
                                    [periodDate]     varchar(200)     NOT NULL, 
                                    [timeOfDay]      varchar(10)      NOT NULL,                                   
                                    [description]    varchar(200)     NOT NULL,
                                    [temperature]    varchar(10)      NOT NULL,
                                    [windSpeed]      varchar(10)      NULL,
                                    [windDirection]  varchar(5)       NULL,
                                    [pressure]       varchar(10)      NULL,
                                    [hummidity]      varchar(10)      NULL, 
                                    [symbol]         varchar(20)      NOT NULL                           
                                    );";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE [ten_days_forecasts] (
                                    [ID]             integer          PRIMARY KEY NOT NULL,
                                    [settingID]      integer          NOT NULL,
                                    [periodDate]     varchar(200)     NOT NULL, 
                                    [timeOfDay]      varchar(10)      NOT NULL, 
                                    [temperature]    varchar(10)      NOT NULL,
                                    [symbol]         varchar(20)      NOT NULL                    
                                    );";
                command.ExecuteNonQuery();


                m_dbConnection.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}
