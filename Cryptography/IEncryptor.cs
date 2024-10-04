using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PManager.Cryptography
{
  public interface IEncryptor
  {
    string Encrypt(string text);

    string Decrypt(string cryptedText);
  }
}
