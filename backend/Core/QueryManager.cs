using System;
using LinqToDB;
using PManager.Cryptography;
using PManager.Data;

namespace PManager.Core
{
  public class QueryManager : IQueryManager
  {
    private readonly IDatabaseContext _context; 
    private readonly IEncryptor _encryptor;
    
    public QueryManager(IDatabaseContext context, IEncryptor encryptor)
    {
      _context = context;
      _encryptor = encryptor;
    }  

    
    public async Task SavePasswordAsync(string login, string password)
    {
      if (string.IsNullOrEmpty(login)) throw new ArgumentException("Login cannot be null or empty.", nameof(login)); 
      if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password cannot be null or empty.", nameof(password));
      try
      {
        string encryptedPassword = _encryptor.Encrypt(password);
        
        var existingPassword =  await _context.Passwords.FirstOrDefaultAsync(p => p.Login == login);

        if (existingPassword != null)
        {
          existingPassword.PasswordHash = encryptedPassword;
          await _context.UpdateAsync(existingPassword);
        }
        else
        {
          var newPassword = new PasswordEntity
          {
            Login = login,
            PasswordHash = encryptedPassword
          };
          await _context.InsertAsync(newPassword);
        }
      }
      catch (Exception ex)
      {
        throw new DbException("Error occurred saving password.", ex);
      }
    }

    public async Task<string> FindPasswordAsync(string login)
    {
      try
      {
        var passwordRecord = await _context.Passwords.FirstOrDefaultAsync(p => p.Login == login);

        if (passwordRecord != null)
        {
          return _encryptor.Decrypt(passwordRecord.PasswordHash);
        }
        return null;
      }
      catch (Exception ex)
      {
        throw new DbException("Error occured finding password.", ex);
      }
    }

    public async Task ChangePasswordAsync(string login, string newPassword)
    {
      try
      {
        var passwordRecord = await _context.Passwords.FirstOrDefaultAsync(p => p.Login == login);

        if (passwordRecord != null)
        {
          passwordRecord.PasswordHash = _encryptor.Encrypt(newPassword);
          await _context.UpdateAsync(passwordRecord);
        }
      }
      catch (Exception ex)
      {
        throw new DbException("Error occured changing password.", ex);
      }
    }
  }
}
