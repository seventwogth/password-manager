using System;
using System.Data.SQLite;
using BCrypt.Net;

namespace password_manager
{
  public class PasswordManager
  {
    private readonly string connectionString;

    public PasswordManager(string dbPath)
    {
      connectionString = $"Data source ={dbPath}, version = 3;";
      InitializeDatabase();
    }

    private void InitializeDatabase()
    {
      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();
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
    // some code lmao
  }
}
