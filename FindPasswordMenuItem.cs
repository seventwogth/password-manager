using System;
using System.Threading.Tasks;

namespace PManager.Core
{
  public class FindPasswordMenuItem : MenuItem
  {
    private readonly PasswordManager _passwordManager;

    public FindPasswordMenuItem(PasswordManager passwordManager)
      : base("Find password by login")
    {
      _passwordManager = passwordManager;
    }

    public override async Task ExecuteAsync()
    {
      Console.WriteLine("Enter login:");
      string login = Console.ReadLine();
      Console.WriteLine();

      if (!String.IsNullOrEmpty(login))
      {
        string password = await _passwordManager.FindPasswordAsync(login);
        if (!String.IsNullOrEmpty(password))
        {
          Console.WriteLine($"Password for '{login}' : {password}");
          Console.WriteLine();
        }
        else
        {
          Console.WriteLine("Password not found.");
          Console.WriteLine();
        }
      }
      else
      {
        Console.WriteLine("Login cannot be empty.");
        Console.WriteLine();
      }
    }
  }
}
