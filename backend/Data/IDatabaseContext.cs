using LinqToDB;
namespace PManager.Data
{
    public interface IDatabaseContext : LinqToDB.IDataContext
    {
        ITable<PasswordEntity> Passwords { get; }

        Task<int> InsertPasswordAsync(PasswordEntity entity);

    }
}

