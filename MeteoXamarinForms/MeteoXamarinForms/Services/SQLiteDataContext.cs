using MeteoXamarinForms.Models;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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

        private SQLiteAsyncConnection connection;

        private SQLiteDataContext()
        {
            connection = DependencyService.Get<ISQLite>().GetSQLiteConnection();
            //CreateTableIfNotExist();
            connection.CreateTableAsync<Root>().Wait();
            connection.CreateTableAsync<Models.Weather>().Wait();
            connection.CreateTableAsync<Current>().Wait();
            connection.CreateTableAsync<Rain>().Wait();
            connection.CreateTableAsync<Hourly>().Wait();
            connection.CreateTableAsync<Temp>().Wait();
            connection.CreateTableAsync<FeelsLike>().Wait();
            connection.CreateTableAsync<Daily>().Wait();
        }

        public async Task<List<Root>> GetAllRoot()
        {
            return await ReadOperations.GetAllWithChildrenAsync<Root>(connection, recursive: true);
        }

        public async Task<Root> AddRoot(Root root)
        {            
            if(!await ExistRoot(root.Timezone))
                await WriteOperations.InsertWithChildrenAsync(connection, root, recursive: true);
            return root;
        }

        public async Task<Root> GetRootAsync(string timezone)
        {
            return (await ReadOperations.GetAllWithChildrenAsync<Root>(connection, recursive:true)).FirstOrDefault(root => root.Timezone == timezone);
        }

        public async Task<Root> GetRootAsync(Root root)
        {
            int id = await GetRootId(root);
            return await ReadOperations.FindWithChildrenAsync<Root>(connection, id, recursive:true);
        }

        public async Task<bool> ExistRoot(string timezone) 
        {
            Root result = (await ReadOperations.GetAllWithChildrenAsync<Root>(connection, recursive:true)).FirstOrDefault(root => root.Timezone == timezone);
            if(result == null)
                return false;
            else
                return true;
        }

        public async Task<int> GetRootId(Root root)
        {
            return (await ReadOperations.GetAllWithChildrenAsync<Root>(connection, recursive: true)).FirstOrDefault(r => r.Timezone == root.Timezone).Id;
        }

        public async Task<int> GetRootId()
        {
            var timezone = Preferences.Get("CurrentTimezone", "");
            if(timezone == "")
                return 0;
            return (await ReadOperations.GetAllWithChildrenAsync<Root>(connection, recursive: true)).FirstOrDefault(r => r.Timezone == timezone).Id;
        }

        public async Task DeleteAsync(string timezone)
        {
            Root root = await GetRootAsync(timezone);
            await WriteOperations.DeleteAsync(connection, root, true);
        }

        public async Task<Root> UpdateRootAsync(Root root)
        {
            await DeleteAsync(root.Timezone);
            await WriteOperations.InsertWithChildrenAsync(connection, root, true);
            return root;
        }

        public async Task DeleteRootsAsync()
        {
            IEnumerable<Root> roots = await GetAllRoot();
            await WriteOperations.DeleteAllAsync(connection, roots);
        }
    }
}
