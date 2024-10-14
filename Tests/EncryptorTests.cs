using NUnit;
using NUnit.Framework;
using System.Security.Cryptography;
using PManager.Cryptography;

namespace PManager.Tests
{
  [TestFixture]
  public class EncryptorTests
  {

    private Encryptor _encryptor;
    private byte[] _key;
    private byte[] _iv;

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
    public void Encryptor_ShouldReturnEncryptedText()
    {
      //Arrange
      var plainText = "password";

      //Act
      var encryptedText = _encryptor.Encrypt(plainText);

      //Assert
      Assert.That(plainText, Is.Not.EqualTo(encryptedText));
    }
    
    [Test]
    public void Encryptor_ShouldReturnDecryptedText()
    {
      //Arrange
      var plainText = "password";

      //Act
      var encryptedText = _encryptor.Encrypt(plainText);
      var decryptedText = _encryptor.Decrypt(encryptedText);

      //Assert
      Assert.That(plainText, Is.EqualTo(decryptedText));
    }

    [Test]
    public void Encryptor_ShouldReturnDifferentEncrypted()
    {
      //Arrange
      var plainText = "password";

      //Act
      var encryptedText1 = _encryptor.Encrypt(plainText);
      var encryptedText2 = _encryptor.Encrypt(plainText);

      //Assert
      Assert.That(encryptedText1, Is.Not.EqualTo(encryptedText2));
    }
  }
}

