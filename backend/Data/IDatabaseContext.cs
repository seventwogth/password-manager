using LinqToDB;

namespace PManager.Data
{
    public interface IDatabaseContext : LinqToDB.IDataContext
    {
        ITable<PasswordEntity> Passwords { get; }
    }
}
