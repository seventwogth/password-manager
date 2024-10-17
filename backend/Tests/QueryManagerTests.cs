using PManager.Core;
using PManager.Data;
using PManager.Cryptography;
using Moq;
using NUnit.Framework;
using LinqToDB;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB.Async;

namespace PManager.Tests
{
  [TestFixture]
  public class QueryManagerTests
  {
    private Mock<IDatabaseContext> _mockDbContext;
    private Mock<IEncryptor> _mockEncryptor;
    private QueryManager _queryManager;

    [SetUp]
    public void SetUp()
    {
      _mockDbContext = new Mock<IDatabaseContext>();
      _mockEncryptor = new Mock<IEncryptor>();
      _queryManager = new QueryManager(_mockDbContext.Object, _mockEncryptor.Object);
    }

    [Test]
    public async Task SavePasswordAsync_ShouldSavePassword()
    {
      // Arrange
      var login = "testUser";
      var password = "password123";
      var encryptedPassword = "encryptedPassword123";

      _mockEncryptor.Setup(e => e.Encrypt(password)).Returns(encryptedPassword);
      _mockDbContext.Setup(db => db.Passwords.FirstOrDefaultAsync(It.IsAny<Expression<Func<PasswordEntity, bool>>>(),
        It.IsAny<CancellationToken>()))
        .ReturnsAsync((PasswordEntity)null);

      // Act
      await _queryManager.SavePasswordAsync(login, password);

      // Assert
      _mockDbContext.Verify(db => db.InsertAsync(
        It.Is<PasswordEntity>(p => p.Login == login && p.PasswordHash == encryptedPassword),
        It.IsAny<InsertColumnFilter<PasswordEntity>>(),
        "Passwords",
        null,
        null,
        null,
        //TableOptions??
        It.IsAny<CancellationToken>()
        ), Times.Once);
    }
  }
}
