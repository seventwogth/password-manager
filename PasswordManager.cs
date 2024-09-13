using System;
using System.Data.SQLite;
using System.Security.Cryptography;
using System.IO;

namespace password_manager
{
  public class PasswordManager
  {
    private readonly string connectionString;

    public PasswordManager(string dbPath)
    {
      connectionString = $"Data Source={dbPath};Version=3;";
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
      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();
        string insertQuery = "INSERT INTO Passwords (Login, PasswordHash) VALUES (@login, @passwordHash)";
        using (var command = new SQLiteCommand(insertQuery, connection))
        {
          command.Parameters.AddWithValue("@login", password.Login);
          command.Parameters.AddWithValue("@passwordHash", password.Encrypt());
          command.ExecuteNonQuery();
        }
      }
    }

    public string FindPassword(string login)
    {
      using (var connection = new SQLiteConnection(connectionString))
      {
        connection.Open();
        string selectQuery = "SELECT PasswordHash FROM Passwords WHERE Login = @login";
        using (var command = new SQLiteCommand(selectQuery, connection))
        {
          command.Parameters.AddWithValue("@login", login);
          using (var reader = command.ExecuteReader())
          {
            if (reader.Read())
            {
              string passwordHash = reader.GetString(0);
              string foundPassword = Decrypt(passwordHash);
              return "Password for login '" + login + "': " + foundPassword;
            }
          }
        }
      }
      return "Password not found!";
    }

    public void ChangePassword(string login, string newPassword)
    {
      string newPasswordHash = Encrypt(newPassword); 
    }
  }
}
