using NUnit.Framework;
using PManager.Cryptography;
using System;
using System.Security.Cryptography;

namespace PManager.Tests
{
    [TestFixture]
    public class EncryptorTests
    {
        private byte[]? _key;
        private byte[]? _iv;
        private Encryptor _encryptor;

        [SetUp]
        public void Setup()
        {
            _key = new byte[32];
            _iv = new byte[16];
            RandomNumberGenerator.Fill(_key);
            RandomNumberGenerator.Fill(_iv);
            _encryptor = new Encryptor(_key, _iv);
        }

        [Test]
        public void Encrypt_ShouldReturnEncryptedText()
        {
            //Arrange
            var plainText = "password";

            //Act
            var encryptedText = _encryptor.Encrypt(plainText);

            //Assert
            Assert.That(encryptedText, Is.Not.Null);
            Assert.That(plainText, Is.Not.EqualTo(encryptedText));
        }

        [Test]
        public void Decrypt_ShouldReturnDecryptedText()
        {
            //Arrange
            var plainText = "password";
            var encryptedText = _encryptor.Encrypt(plainText);

            //Act
            var decryptedText = _encryptor.Decrypt(encryptedText);

            //Assert
            Assert.That(plainText, Is.EqualTo(decryptedText));
        }
    }
}
