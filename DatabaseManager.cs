using System;
using System.Data;
using System.Data.SQLite;

namespace password_manager
{
  public class DatabaseManager
  {
    private readonly string _connectionString;

    public DatabaseManager(string dbPath)
    {
      _connectionString = $"Data Source={dbPath};Version=3;BusyTimeout=3000";
      InitializeDatabase();
    }

    private void InitializeDatabase()
    {
      using (var connection = new SQLiteConnection(_connectionString))
      {
        connection.Open();
        using (var command = new SQLiteCommand("PRAGMA journal_mode = WAL;", connection))
        {
          command.ExecuteNonQuery();
        }
        string createTableQuery = @"CREATE TABLE IF NOT EXISTS Passwords (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Login TEXT NOT NULL UNIQUE,
        PasswordHash TEXT NOT NULL)";
        using (var command = new SQLiteCommand(createTableQuery, connection))
        {
          command.ExecuteNonQuery();
        }
      }
    }

    public void ExecuteQuery(string query, SQLiteParameter[] parameters)
    {
      using (var connection = new SQLiteConnection(_connectionString))
      {
        connection.Open();
        using (var command = new SQLiteCommand(query, connection))
        {
          command.Parameters.AddRange(parameters);
          command.ExecuteNonQuery();
        }
      }
    }

    public SQLiteDataReader ExecuteReader(string query, SQLiteParameter[] parameters)
    {
      var connection = new SQLiteConnection(_connectionString);
      connection.Open();
      try
      {
        var command = new SQLiteCommand(query, connection);
        command.Parameters.AddRange(parameters);
        return command.ExecuteReader(CommandBehavior.CloseConnection);
      }
      catch
      {
        connection.Close();
        throw;
      }
    }
  }
}
