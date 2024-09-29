using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace password_manager
{
  public class AppManager
  {
    private readonly PasswordManager _passwordManager;
    private readonly string _masterPassword;
    private bool _running;
    private readonly Dictionary<string, Func<Task>> _menuActions;

    public AppManager(PasswordManager passwordManager, string masterPassword)
    {
      _passwordManager = passwordManager;
      _masterPassword = masterPassword;
      _running = false;

      _menuActions = new Dictionary<string, Func<Task>>
      {
        { "1", SavePasswordAsync },
        { "2", FindPasswordAsync },
        { "3", ChangePasswordAsync },
        { "4", ExitAsync }
      };
    }

    public async Task StartAsync()
    {
      Console.WriteLine("Please insert master-password:");
      string response = Console.ReadLine();
      if (response == _masterPassword)
      {
        _running = true;
        while (_running)
        {
          ShowMenu();
          string choice = Console.ReadLine();
          Console.WriteLine();

          if (!string.IsNullOrEmpty(choice) && _menuActions.ContainsKey(choice))
          {
            await _menuActions[choice]();
          }
          else
          {
            Console.WriteLine("\nInvalid option. Please choose an existing option.");
          }
        }
      }
      else
      {
        Console.WriteLine("\nInvalid master-password! Exiting the program...");
      }
    }

    private void ShowMenu()
    {
      Console.WriteLine("\nAvailable options:");
      Console.WriteLine("1 - Save new password");
      Console.WriteLine("2 - Find existing password by login");
      Console.WriteLine("3 - Change existing password by login");
      Console.WriteLine("4 - Exit");
      Console.WriteLine();
    }

    private async Task SavePasswordAsync()
    {
      Console.WriteLine("Enter login:");
      string login = Console.ReadLine();
      Console.WriteLine();
      Console.WriteLine("Enter password:");
      string password = Console.ReadLine();
      Console.WriteLine();

      if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
      {
      await _passwordManager.SavePasswordAsync(login, password);
      }
      else
      {
        Console.WriteLine("\nLogin and password can't be null!");
      }
    }

    private async Task FindPasswordAsync()
    {
      Console.WriteLine("Enter login:");
      string login = Console.ReadLine();
      if (!string.IsNullOrEmpty(login))
      {
        string password = await _passwordManager.FindPasswordAsync(login);
        if (password != null)
        {
          Console.WriteLine($"\nPassword for login {login}: {password}");
        }
        else
        {
          Console.WriteLine("\nPassword not found.");
        }
      }
      else
      {
        Console.WriteLine("\nLogin can't be null!");
      }
    }

    private async Task ChangePasswordAsync()
    {
      Console.WriteLine("Enter login:");
      string login = Console.ReadLine();
      Console.WriteLine("Enter new password:");
      string newPassword = Console.ReadLine();

      if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(newPassword))
      {
        await _passwordManager.ChangePasswordAsync(login, newPassword);
      }
      else
      {
        Console.WriteLine("\nLogin and password can't be null!");
      }
    }

    private Task ExitAsync()
    {
      _running = false;
      Console.WriteLine("Exiting...");
      return Task.CompletedTask;
    }
  }
}
