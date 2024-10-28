using NUnit.Framework;
using Moq;
using PManager.Data;
using PManager.Cryptography;

namespace PManager.Tests
{
    [TestFixture]
    public class QueryManagerTests
    {
        private Mock<IDatabaseContext> _dbContext;
        private Mock<IEncryptor> _encryptor;

        [SetUp]
        public void Setup()
        {
        }
    }
}
