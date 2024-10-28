using NUnit.Framework;
using Moq;
using System.Linq.Expressions;
using LinqToDB;
using PManager.Core;
using PManager.Data;
using PManager.Cryptography;

namespace PManager.Tests
{
    [TestFixture]
    public class QueryManagerTests
    {
        private Mock<IDatabaseContext> _dbContext;
        private Mock<IEncryptor> _encryptor;
        private QueryManager _queryManager;

        [SetUp]
        public void SetUp()
        {
            _dbContext = new Mock<IDatabaseContext>();
            _encryptor = new Mock<IEncryptor>();
            _queryManager = new QueryManager(_dbContext.Object, _encryptor.Object);
        }


        [Test]
        public async Task SavePassword_ShouldSaveNewPassword_WhenLoginIsNew()
        {
            // Arrange
            var login = "test";
            var password = "test";
            var encryptedPassword = "e_test";

            _encryptor.Setup(e => e.Encrypt(password)).Returns(encryptedPassword);

            var mockPasswordData = new List<PasswordEntity>().AsQueryable();
            var mockTable = new Mock<ITable<PasswordEntity>>();

            mockTable.As<IQueryable<PasswordEntity>>().Setup(m => m.Provider).Returns(mockPasswordData.Provider);
            mockTable.As<IQueryable<PasswordEntity>>().Setup(m => m.Expression).Returns(mockPasswordData.Expression);
            mockTable.As<IQueryable<PasswordEntity>>().Setup(m => m.ElementType).Returns(mockPasswordData.ElementType);
            mockTable.As<IQueryable<PasswordEntity>>().Setup(m => m.GetEnumerator()).Returns(mockPasswordData.GetEnumerator);

            _dbContext.Setup(db => db.Passwords).Returns(mockTable.Object);
            _dbContext.Setup(db => db.InsertPasswordAsync(It.IsAny<PasswordEntity>())).ReturnsAsync(1);

            // Act
            await _queryManager.SavePasswordAsync(login, password);

            // Assert
            _dbContext.Verify(db => db.InsertPasswordAsync(
                It.Is<PasswordEntity>(p => p.Login == login && p.PasswordHash == encryptedPassword)
            ), Times.Once);
        }


        [Test]
        public async Task FindPasswordAsync_ShouldReturnDecryptedPassword_WhenPasswordExists()
        {
            // Arrange
            var login = "test";
            var password = "test";
            var encryptedPassword = "e_test";

            _encryptor.Setup(e => e.Encrypt(password)).Returns(encryptedPassword);
            _encryptor.Setup(e => e.Decrypt(encryptedPassword)).Returns(password);

            var passwordEntity = new PasswordEntity { Login = login, PasswordHash = encryptedPassword };
            var passwordEntities = new List<PasswordEntity> { passwordEntity }.AsQueryable();

            var passwordTableMock = new Mock<ITable<PasswordEntity>>();
            passwordTableMock.As<IQueryable<PasswordEntity>>().Setup(m => m.Provider).Returns(passwordEntities.Provider);
            passwordTableMock.As<IQueryable<PasswordEntity>>().Setup(m => m.Expression).Returns(passwordEntities.Expression);
            passwordTableMock.As<IQueryable<PasswordEntity>>().Setup(m => m.ElementType).Returns(passwordEntities.ElementType);
            passwordTableMock.As<IQueryable<PasswordEntity>>().Setup(m => m.GetEnumerator()).Returns(passwordEntities.GetEnumerator());

            passwordTableMock.As<IAsyncEnumerable<PasswordEntity>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<PasswordEntity>(passwordEntities.GetEnumerator()));

            _dbContext.Setup(db => db.Passwords).Returns(passwordTableMock.Object);

            // Act
            var result = await _queryManager.FindPasswordAsync(login);

            // Assert
            Assert.That(result, Is.EqualTo(password));
        }


        [Test]
        public async Task FindPasswordAsync_ShouldReturnMessage_WhenPasswordDoesNotExist()
        {
            // Arrange
            var login = "test";

            var passwordEntities = new List<PasswordEntity>().AsQueryable();

            var passwordTableMock = new Mock<ITable<PasswordEntity>>();
            passwordTableMock.As<IQueryable<PasswordEntity>>().Setup(m => m.Provider).Returns(passwordEntities.Provider);
            passwordTableMock.As<IQueryable<PasswordEntity>>().Setup(m => m.Expression).Returns(passwordEntities.Expression);
            passwordTableMock.As<IQueryable<PasswordEntity>>().Setup(m => m.ElementType).Returns(passwordEntities.ElementType);
            passwordTableMock.As<IQueryable<PasswordEntity>>().Setup(m => m.GetEnumerator()).Returns(passwordEntities.GetEnumerator());

            passwordTableMock.As<IAsyncEnumerable<PasswordEntity>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<PasswordEntity>(passwordEntities.GetEnumerator()));

            _dbContext.Setup(db => db.Passwords).Returns(passwordTableMock.Object);

            // Act
            var result = await _queryManager.FindPasswordAsync(login);

            // Assert
            Assert.That(result, Is.EqualTo("Password does not exist."));
        }


        [Test]
        public async Task ChangePasswordAsync_ShouldChangePassword()
        {
            // Arrange
            var login = "test";
            var password = "test";
            var encryptedPassword = "e_test";

            var newPassword = "test2";
            var newEncryptedPassword = "e_test2";

            _encryptor.Setup(e => e.Encrypt(password)).Returns(encryptedPassword);
            _encryptor.Setup(e => e.Encrypt(newPassword)).Returns(newEncryptedPassword);

            var passwordEntity = new PasswordEntity { Login = login, PasswordHash = encryptedPassword };
            var mockPasswordData = new List<PasswordEntity> { passwordEntity }.AsQueryable();
            var mockTable = new Mock<ITable<PasswordEntity>>();

            mockTable.As<IQueryable<PasswordEntity>>().Setup(m => m.Provider).Returns(mockPasswordData.Provider);
            mockTable.As<IQueryable<PasswordEntity>>().Setup(m => m.Expression).Returns(mockPasswordData.Expression);
            mockTable.As<IQueryable<PasswordEntity>>().Setup(m => m.ElementType).Returns(mockPasswordData.ElementType);
            mockTable.As<IQueryable<PasswordEntity>>().Setup(m => m.GetEnumerator()).Returns(mockPasswordData.GetEnumerator());

            _dbContext.Setup(db => db.Passwords).Returns(mockTable.Object);

            _dbContext.Setup(db => db.UpdateAsync(
                  It.IsAny<PasswordEntity>(),
                  null,
                  null,
                  null,
                  null,
                  TableOptions.NotSet,
                  default(CancellationToken)))
                  .ReturnsAsync(1);

            // Act
            await _queryManager.ChangePasswordAsync(login, newPassword);

            // Assert
            _dbContext.Verify(db => db.UpdateAsync(
                It.Is<PasswordEntity>(p => p.Login == login && p.PasswordHash == newEncryptedPassword),
                null,
                null,
                null,
                null,
                TableOptions.NotSet,
                default(CancellationToken)), Times.Once);
        }
    }

}

