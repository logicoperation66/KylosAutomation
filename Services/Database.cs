using Dapper;
using Microsoft.Data.Sqlite;

namespace KylosNotify.Services;

public class Database
{
    private SqliteConnection _connection;

    public Database(SqliteConnection connection)
    {
        _connection = connection;
    }

    
}