using System;
using System.Collections.Generic;

namespace password_manager
{
  public class AppManager
  {
    private readonly PasswordManager _passwordManager;
    private readonly string _masterPassword;
    private bool _running;
    private readonly Dictionary<string, Action> _menuActions;

    public AppManager(PasswordManager passwordManager, string masterPassword)
    {
      _passwordManager = passwordManager;
      _masterPassword = masterPassword;
      _running = false;

      _menuActions = new Dictionary<string, Action>
      {
        { "1", SavePassword },
        { "2", FindPassword },
        { "3", ChangePassword },
        { "4", Exit }
      };
    }

    public void Start()
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
            _menuActions[choice].Invoke();
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

    private void SavePassword()
    {
      Console.WriteLine("Enter login:");
      string login = Console.ReadLine();
      Console.WriteLine();
      Console.WriteLine("Enter password:");
      string password = Console.ReadLine();
      Console.WriteLine();

      if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
      {
      _passwordManager.SavePassword(login, password);
      }
      else
      {
        Console.WriteLine("\nLogin and password can't be null!");
      }
    }

    private void FindPassword()
    {
      Console.WriteLine("Enter login:");
      string login = Console.ReadLine();
      if (!string.IsNullOrEmpty(login))
      {
        string password = _passwordManager.FindPassword(login);
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

    private void ChangePassword()
    {
      Console.WriteLine("Enter login:");
      string login = Console.ReadLine();
      Console.WriteLine("Enter new password:");
      string newPassword = Console.ReadLine();

      if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(newPassword))
      {
        _passwordManager.ChangePassword(login, newPassword);
      }
      else
      {
        Console.WriteLine("\nLogin and password can't be null!");
      }
    }

    private void Exit()
    {
      _running = false;
      Console.WriteLine("Exiting...");
    }
  }
}
