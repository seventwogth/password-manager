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
    string keyFilePath = "encryption_keys.json";
    EncryptionConfig encryptionConfig;

    if (File.Exists(keyFilePath))
    {
      encryptionConfig = EncryptionSetup.LoadKey(keyFilePath);
      Console.WriteLine("Encryption keys loaded.");
    }
    else
    {
      EncryptionSetup.GenerateAndSaveKey(keyFilePath);
      encryptionConfig = EncryptionSetup.LoadKey(keyFilePath);
      Console.WriteLine("Encryption keys generated and saved.");
    }

    IEncryptor encryptor = new Encryptor(encryptionConfig.Key, encryptionConfig.IV);
    var connectionString = "Data Source=passwords.db";

    using (var dbContext = new DatabaseContext(connectionString))
    {
      PasswordManager passwordManager = new PasswordManager(dbContext, encryptor);
      AppManager appManager = new AppManager(passwordManager, "master");

      await appManager.StartAsync();
    }
  }
}
