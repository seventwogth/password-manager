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
//      if (!this.TableExists<PasswordEntity>())
//      {
      this.CreateTable<PasswordEntity>();
//      }
    }

    public ITable<PasswordEntity> Passwords => this.GetTable<PasswordEntity>();
  }
}
