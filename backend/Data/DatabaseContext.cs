using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace PManager.Data
{
  public class DatabaseContext : DataConnection, IDatabaseContext
  {
    public DatabaseContext(string connectionString) : base(ProviderName.SQLiteClassic, connectionString)
    {
      CreateDatabase();
    }

    private void CreateDatabase()
    {
      try
      {
        var exists = this.GetTable<PasswordEntity>().Any();
      }
      catch (Exception ex)
      {
        if (ex.Message.Contains("no such table"))
        {
            try
            {
                Console.WriteLine("Creating table...");
                this.CreateTable<PasswordEntity>();
                Console.WriteLine("Table created.");
            }
            catch (Exception createEx)
            {
                Console.WriteLine($"Error creating table: {createEx.Message}");
                throw;
            }
        }
        else
        {
            throw;
        }
      }
    }



    public ITable<PasswordEntity> Passwords => this.GetTable<PasswordEntity>();
  }
}
