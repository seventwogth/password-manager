using NUnit.Framework;
using LinqToDB;
using PManager.Data;

namespace PManager.Tests
{
    [TestFixture]
    public class DatabaseContextTests
    {
        private DatabaseContext _dbContext;

        [SetUp]
        public void Setup()
        {
            var connectionString = "Data Source = :memory:";
            _dbContext = new DatabaseContext(connectionString);
        }

        [Test]
        public void DatabaseContext_ShouldCreateTable()
        {
            //Arrange-Act
            var table = _dbContext.GetTable<PasswordEntity>().ToList();

            //Assert
            Assert.That(table, Is.Not.Null);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
        }
    }
}
