using System.Data.SQLite;
using System.Threading.Tasks;

namespace PManager.Data
{
  public interface IDatabaseManager
  { 
    Task ExecuteQueryAsync(string query, SQLiteParameter[] parameters);

    Task<SQLiteDataReader> ExecuteReaderAsync(string query, SQLiteParameter[] parameters);
  }
}
