using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoXamarinForms.Services
{
    public interface ISQLite
    {
        SQLite.SQLiteConnection GetSQLiteConnection();
    }
}
