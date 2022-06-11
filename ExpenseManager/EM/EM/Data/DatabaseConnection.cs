using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EM.Data
{
    public class DatabaseConnection
    {
        public SQLiteAsyncConnection databaseConn;
        public DatabaseConnection(string dbPath)
        {
            databaseConn = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex | SQLiteOpenFlags.ReadWrite);
        }
    }
}
