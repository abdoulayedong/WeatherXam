using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoXamarinForms.Services
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetSQLiteConnection();
    }
}
