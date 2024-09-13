using System;
using password_manager;

class Program
{
  static void Main(string[] args)
  {
    var manager = new PasswordManager("passwords.db");

//    var password_1 = new Password("John@gmail.com", "1234567_");
//    manager.SavePassword(password_1);

//    var password_2 = new Password("Asd@apple.com", "timcooksucks");
//    manager.SavePassword(password_2);
    
    string found = manager.FindPassword("Asd@apple.com");
    Console.WriteLine(found); 

    Console.WriteLine("Finished!");

    Console.ReadLine();
  }
}
