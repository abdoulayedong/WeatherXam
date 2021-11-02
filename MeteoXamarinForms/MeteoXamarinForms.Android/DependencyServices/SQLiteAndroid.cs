using MeteoXamarinForms.Droid.DependencyServices;
using MeteoXamarinForms.Services;
using SQLite;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteAndroid))]
namespace MeteoXamarinForms.Droid.DependencyServices
{
    internal class SQLiteAndroid : ISQLite
    {
        public SQLiteConnection GetSQLiteConnection()
        {
            var fileName = "weather.db3";
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            var connection = new SQLiteConnection(databasePath, true);

            return connection;
        }
    }
}