using System;
using password_manager;

class Program
{
  static async Task Main(string[] args)
  { 
    DatabaseManager dbManager = new DatabaseManager("passwords.db");
    Encryptor encryptor = new Encryptor("0000000000000052");
    PasswordManager passwordManager = new PasswordManager(dbManager, encryptor);
    AppManager appManager = new AppManager(passwordManager, "master");

    await appManager.StartAsync();
  }
}
