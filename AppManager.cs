using System;
using System.Collections.Generic;

namespace password_manager
{
  public class AppManager
  {
    private PasswordManager passwordManager;
    private bool running;
    private string masterPassword;
    private Dictionary<string, Action> menuActions;

    public AppManager(PasswordManager passwordM, string master)
    {
      passwordManager = passwordM;
      masterPassword = master;
      running = false;

      menuActions = new Dictionary<string, Action>
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
      if (response == masterPassword)
      {
        running = true;
      }
      else
      {
        Console.WriteLine("\nInvalid master-password!\nExiting the program...");
      }
      while (running)
      {
        ShowMenu();
        string choice = Console.ReadLine();
        Console.WriteLine();

        if (!String.IsNullOrEmpty(choice) && menuActions.ContainsKey(choice))
        {
          menuActions[choice].Invoke();
        }
        else
        {
          Console.WriteLine("\nInvalid option. Please choose the existion option.");
        }
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
      if (!String.IsNullOrEmpty(login))
      {
        Console.WriteLine("Enter password:");
        string password = Console.ReadLine();
        Console.WriteLine();
        if (!String.IsNullOrEmpty(password))
        {
          passwordManager.SavePassword(login, password);
        }
        else
        {
          Console.WriteLine("\nPassword can't be null!");
        }
      }
      else 
      {
        Console.WriteLine("\nLogin can't be null!");
      }
    }
    
    private void FindPassword()
    {
      Console.WriteLine("Enter login:");
      string login = Console.ReadLine();
      if (!String.IsNullOrEmpty(login))
      {
        string password = passwordManager.FindPassword(login);
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
      Console.WriteLine();
      if (!String.IsNullOrEmpty(login))
      {
        Console.WriteLine("Enter new password:");
        string newPassword = Console.ReadLine();
        Console.WriteLine();
        if (!String.IsNullOrEmpty(newPassword))
        {
          passwordManager.ChangePassword(login, newPassword);
        }
        else
        {
          Console.WriteLine("Password can't be null!");
        }
      }
      else
      {
        Console.WriteLine("Login can't be null!");   
      }
    }

    private void Exit()
    {
      running = false;
      Console.WriteLine("Exiting...");
    }
  }
}
