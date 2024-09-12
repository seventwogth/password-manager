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
    
    public void SavePassword(Password password)
    {
      string passwordHash = BCrypt.Net.BCrypt.HashPassword(password.passwordValue);
      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();
        string insertQuery = "INSERT INTO Passwords (Login, PasswordHash) VALUES (@login, @passwordHash)";
        using (var command = new SQLiteCommand(insertQuery, connection))
        {
          command.Parameters.AddWithValue("@login", password.login);
          command.Parameters.AddWithValue("@passwordHash", passwordHash);
          command.ExecuteNonQuery();
        }
      }
    } 
  }
}
