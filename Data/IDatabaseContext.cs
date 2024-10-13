using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping; 

namespace PManager.Data
{
  public interface IDatabaseContext : LinqToDB.IDataContext
  {
    ITable<PasswordEntity> Passwords { get; }
  }
}

