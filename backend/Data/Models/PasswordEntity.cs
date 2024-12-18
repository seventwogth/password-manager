using LinqToDB.Mapping;

namespace PManager.Data
{
    [Table(Name = "Passwords")]
    public class PasswordEntity
    {
        [PrimaryKey, Identity]
        public int Id { get; set; }

        [Column(Name = "Login"), NotNull]
        public string Login { get; set; }

        [Column(Name = "PasswordHash"), NotNull]
        public string PasswordHash { get; set; }
    }
}


