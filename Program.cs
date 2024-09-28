using System;
using password_manager;

class Program
{
  static void Main(string[] args)
  {
    PasswordManager passwordManager = new PasswordManager("passwords.db", "0000000000000052");
    AppManager appManager = new AppManager(passwordManager, "master");

    appManager.Start();
  }
}
