using System;
using System.Data.SQLite;
using PManager.Cryptography;
using PManager.Data;

namespace PManager.Core
{
  public class PasswordManager : IPasswordManager
  {
    private readonly IDatabaseManager _dbManager; 
    private readonly IEncryptor _encryptor;
    
    public PasswordManager(IDatabaseManager dbManager, IEncryptor encryptor)
    {
      _dbManager = dbManager;
      _encryptor = encryptor;
    }  

    public async Task SavePasswordAsync(string login, string password)
    {
      try
      {
        string encryptedPassword = _encryptor.Encrypt(password);

        string checkExistQuery = "SELECT COUNT(*) FROM Passwords WHERE Login = @login";
        SQLiteParameter[] checkParams = { new SQLiteParameter("@login", login) };

        using (var reader = await _dbManager.ExecuteReaderAsync(checkExistQuery, checkParams))
        {
          if (await reader.ReadAsync() && (long)reader[0] > 0)
          {
            string updateQuery = "UPDATE Passwords SET PasswordHash = @passwordHash WHERE Login = @login";
            SQLiteParameter[] updateParams = 
            {
              new SQLiteParameter("@login", login),
              new SQLiteParameter("@passwordHash", encryptedPassword)
            };
            await _dbManager.ExecuteQueryAsync(updateQuery, updateParams);
          }
          else
          {
            string insertQuery = "INSERT INTO Passwords (Login, PasswordHash) VALUES (@login, @passwordHash)";
            SQLiteParameter[] insertParams = 
            {
              new SQLiteParameter("@login", login),
              new SQLiteParameter("@passwordHash", encryptedPassword)
            };
            await _dbManager.ExecuteQueryAsync(insertQuery, insertParams);
          }
        }
      }
      catch (Exception ex)
      {
        throw new DbException("Error occured saving password.", ex);
      }
    }
    //метод поиска пароля по логину
    public async Task<string> FindPasswordAsync(string login)
    {
      try
      {
        string selectQuery = "SELECT PasswordHash FROM Passwords WHERE Login = @login";
        SQLiteParameter[] selectParams = { new SQLiteParameter("@login", login) };

        using (var reader = await _dbManager.ExecuteReaderAsync(selectQuery, selectParams))
        {
          if (await reader.ReadAsync())
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
    public async Task ChangePasswordAsync(string login, string newPassword)
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
        await _dbManager.ExecuteQueryAsync(updateQuery, updateParams);
      }
      catch (Exception ex)
      {
        throw new DbException("Error occured changing password.", ex);
      }
    }
  }
}
