using System;
using System.Data.SQLite;

//Класс объекта - менеджера паролей, содержащий всю основную логику
namespace password_manager
{
  public class PasswordManager
  {
    private readonly DatabaseManager _dbManager; 
    private readonly Encryptor _encryptor;
    
    //конструктора по умолчанию не будет (опять)
    public PasswordManager(DatabaseManager dbManager, Encryptor encryptor)
    {
      _dbManager = dbManager;
      _encryptor = encryptor;
    }  
    // метод сохранения пароля в базу данных.
    // во избежание ошибок, при добавлении уже существующих значений они обновляются.
    // так как параметр Login является уникальным, БД не даст добавить два одинаковых значения
    public void SavePassword(string login, string password)
    {
      try
      {
        string encryptedPassword = _encryptor.Encrypt(password);

        string checkExistQuery = "SELECT COUNT(*) FROM Passwords WHERE Login = @login";
        SQLiteParameter[] checkParams = { new SQLiteParameter("@login", login) };

        using (var reader = _dbManager.ExecuteReader(checkExistQuery, checkParams))
        {
          if (reader.Read() && (long)reader[0] > 0)
          {
            string updateQuery = "UPDATE Passwords SET PasswordHash = @passwordHash WHERE Login = @login";
            SQLiteParameter[] updateParams = 
            {
              new SQLiteParameter("@login", login),
              new SQLiteParameter("@passwordHash", encryptedPassword)
            };
            _dbManager.ExecuteQuery(updateQuery, updateParams);
          }
          else
          {
            string insertQuery = "INSERT INTO Passwords (Login, PasswordHash) VALUES (@login, @passwordHash)";
            SQLiteParameter[] insertParams = 
            {
              new SQLiteParameter("@login", login),
              new SQLiteParameter("@passwordHash", encryptedPassword)
            };
            _dbManager.ExecuteQuery(insertQuery, insertParams);
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
        string selectQuery = "SELECT PasswordHash FROM Passwords WHERE Login = @login";
        SQLiteParameter[] selectParams = { new SQLiteParameter("@login", login) };

        using (var reader = _dbManager.ExecuteReader(selectQuery, selectParams))
        {
          if (reader.Read())
          {
            string encryptedPassword = reader.GetString(0);
            return _encryptor.Decrypt(encryptedPassword);
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
      try
      {
        string encryptedPassword = _encryptor.Encrypt(newPassword);
        string updateQuery = "UPDATE Passwords SET PasswordHash = @passwordHash WHERE Login = @login";
        SQLiteParameter[] updateParams = 
        {
          new SQLiteParameter("@login", login),
          new SQLiteParameter("@passwordHash", encryptedPassword)
        };
        _dbManager.ExecuteQuery(updateQuery, updateParams);
      }
      catch (Exception ex)
      {
        throw new DbException("Error occured changing password.", ex);
      }
    }
  }
}
