using MeteoXamarinForms.Models;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MeteoXamarinForms.Services
{
    public class SQLiteDataContext
    {
        private static SQLiteDataContext instance;

        public static SQLiteDataContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SQLiteDataContext();
                }
                return instance;
            }
        }

        private SQLite.SQLiteConnection connection;

        private SQLiteDataContext()
        {
            connection = DependencyService.Get<ISQLite>().GetSQLiteConnection();
            CreateTableIfNotExist();
        }

        private void CreateTableIfNotExist()
        {
            bool isTableNotExist = false;

            try
            {
                var test = connection.Table<Root>().FirstOrDefault();
            }
            catch
            {
                isTableNotExist = true;
            }
            
            if (isTableNotExist)
            {
                connection.CreateTable<Root>();
            }

            isTableNotExist = false;

            try
            {
                var test = connection.Table<Current>().FirstOrDefault();
            }
            catch
            {
                isTableNotExist = true;
            }

            if (isTableNotExist)
            {
                connection.CreateTable<Current>();
            }

            isTableNotExist = false;

            try
            {
                var test = connection.Table<Daily>().FirstOrDefault();
            }
            catch
            {
                isTableNotExist = true;
            }

            if (isTableNotExist)
            {
                connection.CreateTable<Daily>();
            }

            isTableNotExist = false;

            try
            {
                var test = connection.Table<Hourly>().FirstOrDefault();
            }
            catch
            {
                isTableNotExist = true;
            }

            if (isTableNotExist)
            {
                connection.CreateTable<Hourly>();
            }

            isTableNotExist = false;

            try
            {
                var test = connection.Table<FeelsLike>().FirstOrDefault();
            }
            catch
            {
                isTableNotExist = true;
            }

            if (isTableNotExist)
            {
                connection.CreateTable<FeelsLike>();
            }

            isTableNotExist = false;

            try
            {
                var test = connection.Table<Temp>().FirstOrDefault();
            }
            catch
            {
                isTableNotExist = true;
            }

            if (isTableNotExist)
            {
                connection.CreateTable<Temp>();
            }

            isTableNotExist = false;

            try
            {
                var test = connection.Table<Rain>().FirstOrDefault();
            }
            catch
            {
                isTableNotExist = true;
            }

            if (isTableNotExist)
            {
                connection.CreateTable<Rain>();
            }

            isTableNotExist = false;

            try
            {
                var test = connection.Table<Models.Weather>().FirstOrDefault();
            }
            catch
            {
                isTableNotExist = true;
            }

            if (isTableNotExist)
            {
                connection.CreateTable<Models.Weather>();
            }

            isTableNotExist = false;
        }

        public List<Root> GetAllRoot()
        {
            return ReadOperations.GetAllWithChildren<Root>(connection, recursive: true).ToList();
        }

        public int AddRoot(Root root)
        {
            WriteOperations.InsertOrReplaceWithChildren(connection, root, recursive: true);

            return root.Id;
        }

        public Root GetRoot(int id)
        {
            return ReadOperations.GetWithChildren<Root>(connection, recursive:true, pk: id);
        }


    }
}
