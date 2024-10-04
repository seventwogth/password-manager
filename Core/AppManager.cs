using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PManager.Core
{
  public class AppManager
  {
    private readonly List<MenuItem> _menuItems;
    private readonly string _masterPassword;
    private bool _running;

    public AppManager(IPasswordManager passwordManager, string masterPassword)
    {
      _masterPassword = masterPassword;
      _menuItems = new List<MenuItem>
      {
        new SavePasswordMenuItem(passwordManager),
        new FindPasswordMenuItem(passwordManager),
        new ChangePasswordMenuItem(passwordManager),
        new ExitMenuItem(this)
      };
    }

    public async Task StartAsync()
    {
      Console.WriteLine("Please, insert master password:");
      string response = Console.ReadLine();
      Console.WriteLine();

      if (response == _masterPassword)
      {
        _running = true;
        while (_running)
        {
          ShowMenu();
          string choice = Console.ReadLine();
          Console.WriteLine();

          if (int.TryParse(choice, out int index) && index > 0 && index <= _menuItems.Count)
          {
            await _menuItems[index - 1].ExecuteAsync();
          }
          else
          {
            Console.WriteLine("Invalid option. Please, try again.");
            Console.WriteLine();
          }
        }
      }
      else
      {
        Console.WriteLine("Invalid master password.\nExiting the program...");
      }
    }

    public void Stop()
    {
      _running = false;
      Console.WriteLine("Exiting...");
    }

    private void ShowMenu()
    {
      Console.WriteLine("Avaliable options:");
      for (int i = 0; i < _menuItems.Count; i++)
      {
        Console.WriteLine($"{i + 1} - {_menuItems[i].id}");
      }
      Console.WriteLine();
    }
  }
}
