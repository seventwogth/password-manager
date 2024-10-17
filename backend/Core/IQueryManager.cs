using System;
using System.Threading.Tasks;


namespace PManager.Core
{
  public interface IQueryManager
  {
    Task SavePasswordAsync(string login, string password);

    Task<string> FindPasswordAsync(string login);
    
    Task ChangePasswordAsync(string login, string newPassword);
  }
}
