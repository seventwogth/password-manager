using System;
using password_manager;

class Program
{
  static void Main(string[] args)
  {
    var manager = new PasswordManager("passwords.db");

    var password_1 = new Password("John@gmail.com", "1234567_");
    manager.SavePassword(password_1);

    var password_2 = new Password("Asd@apple.com", "timcooksucks");
    manager.SavePassword(password_2);
    
    Password found = manager.FindPassword("Asd@apple.com");
    found.print();

    Console.WriteLine("Finished!");

    Console.ReadLine();
  }
}
