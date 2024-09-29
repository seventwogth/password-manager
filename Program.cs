using System;
using PManager;
using PManager.Core;
using PManager.Cryptography;
using PManager.Data;

class Program
{
  static async Task Main(string[] args)
  { 
    DatabaseManager dbManager = new DatabaseManager("passwords.db");
    Encryptor encryptor = new Encryptor();
    PasswordManager passwordManager = new PasswordManager(dbManager, encryptor);
    AppManager appManager = new AppManager(passwordManager, "master");

    await appManager.StartAsync();
  }
}
