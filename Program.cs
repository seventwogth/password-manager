using System;

namespace password_manager
{
    class Program
    {
        static void Main(string[] args)
        {
          //по задумке мастер-пароль вшит в код, так как пользователь получает его с копией программы
          string masterPassword = "master";
          PasswordManager passwordManager = new PasswordManager("passwords.db");

          Console.WriteLine("Enter master-password:");
          string inputPassword = Console.ReadLine();

          if (inputPassword != masterPassword)
          {
            Console.WriteLine("Invalid master-password. Exiting the program...");
            return;
          }

          bool running = true; //интерфейс меню
          while (running)
          {
            Console.WriteLine("\nAvaliable options:");
            Console.WriteLine("1 - Save new password");
            Console.WriteLine("2 - Find existing password by login");
            Console.WriteLine("3 - Change existing password by login");
            Console.WriteLine("4 - Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
              case "1":
                SavePassword(passwordManager);
                break;
              case "2":
                FindPassword(passwordManager);
                break;
              case "3":
                ChangePassword(passwordManager);
                break;
              case "4":
                running = false;
                Console.WriteLine("Exiting the program...");
                break;
              default:
                Console.WriteLine("Invalid option. Please choose an existing option.");
                break;
            }
          }
        }
        //статические методы для связи с методами класса PasswordManager
        private static void SavePassword(PasswordManager passwordManager)
        {
            Console.Write("Enter login: ");
            string login = Console.ReadLine();
            if (login != "" && login != null)
            {
              Console.Write("Enter password: ");
              string password = Console.ReadLine();
              if (password != "" && password != null)
              {
                try
                {
                    passwordManager.SavePassword(new Password(login, password));
                    Console.WriteLine("Password saved successfully.");
                }
                catch (Exception ex)
                {
                  throw new DbException("Error occured saving password.", ex);
                }
              }
            }
            else
            {
              Console.WriteLine("Password or Login value can't be null!");
            }
        }

        private static void FindPassword(PasswordManager passwordManager)
        {
            Console.Write("Enter login: ");
            string login = Console.ReadLine();
            if (login != "" && login != null)
            {
              try
              {
                  string password = passwordManager.FindPassword(login);
                  Console.WriteLine($"Password for the login '{login}': {password}");
              }
              catch (Exception ex)
              {
                throw new DbException("Error occured finding password.", ex);
              }
            }
            else
            {
              Console.WriteLine("Login value can't be null!");
            }
        }

        private static void ChangePassword(PasswordManager passwordManager)
        {
            Console.Write("Enter login: ");
            string login = Console.ReadLine();
            if (login != "" && login != null)
            {
              Console.Write("Enter new password: ");
              string newPassword = Console.ReadLine();
              if (newPassword != "" && newPassword != null)
              {
                try
                {
                    passwordManager.ChangePassword(login, newPassword);
                    Console.WriteLine("Password saved successfully.");
                }
                catch (Exception ex)
                {
                  throw new DbException("Error occured changing password.", ex);
                }
              }
            }
            else
            {
              Console.WriteLine("Password or Login value can't be null!");
            }
        }
    }
}

