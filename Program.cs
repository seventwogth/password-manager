using System;
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
    var configuration = new ConfigurationBuilder()
      .SetBasePath(AppContext.BaseDirectory)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .Build();

    string connectionString = configuration.GetConnectionString("DefaultConnection");

    using (var dbContext = new DatabaseContext(connectionString))
    {
      PasswordManager passwordManager = new PasswordManager(dbContext, new Encryptor());
      AppManager appManager = new AppManager(passwordManager, "master");

      await appManager.StartAsync();
    }
  }
}
