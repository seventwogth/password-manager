using System;
using System.Data.SQLite;

//Класс объекта - менеджера паролей, содержащий всю основную логику
namespace password_manager
{
  public class PasswordManager
  {
    private readonly string connectionString; //информация о базе данных SQLite
    
    //конструктора по умолчанию не будет (опять)
    public PasswordManager(string dbPath)
    {
      connectionString = $"Data Source={dbPath};Version=3;";
      InitializeDatabase();
    }

    private void InitializeDatabase() //инциализация SQLite БД
    {
      try //присутствует обработка ошибок
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
      catch (Exception ex)
      {
        throw new DbException("Error occured initializing database.", ex); 
      }
    }
    // метод сохранения пароля в базу данных.
    // во избежание ошибок, при добавлении уже существующих значений они обновляются.
    // так как параметр Login является уникальным, БД не даст добавить два одинаковых значения
    public void SavePassword(Password password)
    {
      try
      {
        using (var connection = new SQLiteConnection(connectionString))
        {
          connection.Open();
          string checkExistQuery = "SELECT COUNT(*) FROM Passwords WHERE Login = @login";
          using (var checkCommand = new SQLiteCommand(checkExistQuery, connection))
          {
            checkCommand.Parameters.AddWithValue("@login", password.getLogin());
            long count = (long)checkCommand.ExecuteScalar();

            if (count > 0) //если значение уже присутствует в БД
            {
              string updateQuery = "UPDATE Passwords SET PasswordHash = @passwordHash WHERE Login = @login";
              using (var updateCommand = new SQLiteCommand(updateQuery, connection))
              {
                updateCommand.Parameters.AddWithValue("@login", password.getLogin());
                updateCommand.Parameters.AddWithValue("passwordHash", password.Encrypt());
                updateCommand.ExecuteNonQuery();
              }
            }
            else //если значения нет в БД
            {
              string insertQuery = "INSERT INTO Passwords (Login, PasswordHash) VALUES (@login, @passwordHash)";
              using (var command = new SQLiteCommand(insertQuery, connection))
              {
                command.Parameters.AddWithValue("@login", password.getLogin());
                command.Parameters.AddWithValue("@passwordHash", password.Encrypt());
                command.ExecuteNonQuery();
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        throw new DbException("Error occured saving password.", ex); 
      }
    }
    //метод поиска пароля по логину
    public string FindPassword(string login)
    {
      try
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
                Password result = new Password(login, passwordHash);
                string foundPassword = result.Decrypt();
                return foundPassword;
              }
            }
          }
        }
        return "Password not found!";
      }
      catch (Exception ex)
      {
        throw new DbException("Error occured finding password.", ex);
      }
    }
    //метод изменения пароля по логину
    //отмечу, что для каждого логина может быть только одно значения пароля
    //(параметр Login - уникальный в нашей БД)
    public void ChangePassword(string login, string newPassword)
    {
      Password password = new Password(login, newPassword);
      string newPasswordHash = password.Encrypt();
      try
      {
        using (var connection = new SQLiteConnection(connectionString))
        {
          connection.Open();
          string updateQuery = "UPDATE Passwords SET PasswordHash = @passwordHash WHERE Login = @login";
          using (var command = new SQLiteCommand(updateQuery, connection))
          {
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@passwordHash", newPasswordHash);
            command.ExecuteNonQuery();
          }
        }
      }
      catch (Exception ex)
      {
        throw new DbException("Error occured changing password.", ex);
      }
    }
  }
}
