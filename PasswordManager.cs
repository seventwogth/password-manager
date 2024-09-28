using System;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;

//Класс объекта - менеджера паролей, содержащий всю основную логику
namespace password_manager
{
  public class PasswordManager
  {
    private readonly string _connectionString; 
    private readonly string _secretKey;
    
    //конструктора по умолчанию не будет (опять)
    public PasswordManager(string dbPath, string key)
    {
      _connectionString = $"Data Source={dbPath};Version=3;";
      _secretKey = key;
      InitializeDatabase();
    }

    private void InitializeDatabase() //инциализация SQLite БД
    {
      try //присутствует обработка ошибок
      {
        using (var connection = new SQLiteConnection(_connectionString))
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
    public void SavePassword(string login, string password)
    {
      try
      {
        using (var connection = new SQLiteConnection(_connectionString))
        {
          connection.Open();
          string checkExistQuery = "SELECT COUNT(*) FROM Passwords WHERE Login = @login";
          using (var checkCommand = new SQLiteCommand(checkExistQuery, connection))
          {
            checkCommand.Parameters.AddWithValue("@login", login);
            long count = (long)checkCommand.ExecuteScalar();

            if (count > 0) //если значение уже присутствует в БД
            {
              string updateQuery = "UPDATE Passwords SET PasswordHash = @passwordHash WHERE Login = @login";
              using (var updateCommand = new SQLiteCommand(updateQuery, connection))
              {
                updateCommand.Parameters.AddWithValue("@login", login);
                updateCommand.Parameters.AddWithValue("passwordHash", Encrypt(password));
                updateCommand.ExecuteNonQuery();
              }
            }
            else //если значения нет в БД
            {
              string insertQuery = "INSERT INTO Passwords (Login, PasswordHash) VALUES (@login, @passwordHash)";
              using (var command = new SQLiteCommand(insertQuery, connection))
              {
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@passwordHash", Encrypt(password));
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
        using (var connection = new SQLiteConnection(_connectionString))
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
                return foundPassword;
              }
            }
          }
        }
        return null;
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
      string newPasswordHash = Encrypt(newPassword);
      try
      {
        using (var connection = new SQLiteConnection(_connectionString))
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
    // TODO: убрать класс Password, перенести шифровку в методы ниже
    private string Encrypt(string passwordValue)
    {
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(_secretKey);
        aes.IV = aes.IV;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream())
        {
          ms.Write(aes.IV, 0, aes.IV.Length);
          using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter sw = new StreamWriter(cs))
            {
              sw.Write(passwordValue);
            }
          }
          return Convert.ToBase64String(ms.ToArray());
        }
      }
    }

    private string Decrypt(string passwordValue) //метод дешифрования пароля
    {
      var  fullCipher = Convert.FromBase64String(passwordValue);
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(_secretKey);

        byte[] iv = new byte[aes.BlockSize / 8];
        byte[] cipher = new byte[fullCipher.Length - iv.Length];

        Array.Copy(fullCipher, iv, iv.Length);
        Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream(cipher))
        {
          using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader sr = new StreamReader(cs))
            {
              return sr.ReadToEnd();
            }
          }
        }
      }
    }
  }
}
