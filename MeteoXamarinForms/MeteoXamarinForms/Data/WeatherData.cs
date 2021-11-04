using MeteoXamarinForms.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeteoXamarinForms.Data
{
    public class WeatherData
    {
        readonly SQLiteAsyncConnection _database;

        public WeatherData(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Root>().Wait();
            _database.CreateTableAsync<Weather>().Wait();
            _database.CreateTableAsync<Current>().Wait();
            _database.CreateTableAsync<Rain>().Wait();
            _database.CreateTableAsync<Hourly>().Wait();
            _database.CreateTableAsync<Temp>().Wait();
            _database.CreateTableAsync<FeelsLike>().Wait();
            _database.CreateTableAsync<Daily>().Wait();
        }

        public Task<List<Root>> GetRootsAsync()
        {
            return _database.Table<Root>().ToListAsync();
        }

        public Task<Root> GetRootAsync(int id)
        {
            return _database.Table<Root>().FirstOrDefaultAsync(r => r.Id == id);
        }

        public Task<int> SaveRootAsync(Root root)
        {
            if(root.Id != 0)
                return _database.UpdateAsync(root);
            else
                return _database.InsertAsync(root);
        }

        public Task<int> DeleteRootAsync(Root root)
        {
            return _database.DeleteAsync(root);
        }
    }
}
