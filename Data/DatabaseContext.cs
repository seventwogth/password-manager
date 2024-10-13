using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace PManager.Data
{
  public class DatabaseContext : DataConnection, IDatabaseContext
  {
    public DatabaseContext(string connectionString) : base(connectionString)
    {
      CreateDatabase();
    }

    private void CreateDatabase()
    {
      this.CreateTable<PasswordEntity>();
    }

    public ITable<PasswordEntity> Passwords => this.GetTable<PasswordEntity>();
  }
}
