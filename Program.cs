using System;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using PManager;
using PManager.Core;
using PManager.Cryptography;
using PManager.Data;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

class Program
{
  static async Task Main(string[] args)
  {    
//    var configuration = new ConfigurationBuilder()
//      .SetBasePath(AppContext.BaseDirectory)
//      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//      .Build();
//
    byte[] key = new byte[32];
    byte[] iv = new byte[16];
    RandomNumberGenerator.Fill(key);
    RandomNumberGenerator.Fill(iv);

    var connectionString = "Data Source=passwords.db";

    using (var dbContext = new DatabaseContext(connectionString))
    {
      IEncryptor encryptor = new Encryptor(key, iv);
      PasswordManager passwordManager = new PasswordManager(dbContext, encryptor);
      AppManager appManager = new AppManager(passwordManager, "master");

      await appManager.StartAsync();
    }
  }
}
