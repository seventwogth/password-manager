using System;
using System.Data;
using System.Data.SQLite;

namespace PManager.Data
{
  public class DatabaseManager : IDatabaseManager
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

    public async Task ExecuteQueryAsync(string query, SQLiteParameter[] parameters)
    {
      using (var connection = new SQLiteConnection(_connectionString))
      {
        await connection.OpenAsync();
        using (var transaction = await connection.BeginTransactionAsync())
        {
          using (var command = new SQLiteCommand(query, connection))
          {
            command.Parameters.AddRange(parameters);
            await command.ExecuteNonQueryAsync();
          }
          await transaction.CommitAsync();
        }
      }
    }

    public async Task<SQLiteDataReader> ExecuteReaderAsync(string query, SQLiteParameter[] parameters)
    {
      var connection = new SQLiteConnection(_connectionString);
      await connection.OpenAsync();
      try
      {
        var command = new SQLiteCommand(query, connection);
        command.Parameters.AddRange(parameters);
        return (SQLiteDataReader)await command.ExecuteReaderAsync();
      }
      catch
      {
        connection.Close();
        throw;
      }
    }
  }
}
