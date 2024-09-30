using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PManager.Cryptography
{
  public class Encryptor : IEncryptor
  {
    private readonly byte[] _secretKey;

    public Encryptor()
    {
      _secretKey = getSecretKey(32);
    }
    
    private byte[] getSecretKey(int size)
    {
      byte[] key = new byte[size];
      using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(key);
      }
      return key;
    }

    public string Encrypt(string text)
    {
      using (Aes aes = Aes.Create())
      {
        aes.Key = _secretKey;
        aes.GenerateIV();

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream())
        {
          ms.Write(aes.IV, 0, aes.IV.Length);
          using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter sw = new StreamWriter(cs))
            {
              sw.Write(text);
            }
          }
          return Convert.ToBase64String(ms.ToArray());
        }
      }
    }

    public string Decrypt(string cryptedText)
    {
      var fullCipher = Convert.FromBase64String(cryptedText);
      using (Aes aes = Aes.Create())
      {
        aes.Key = _secretKey;

        byte[] iv = new byte[aes.BlockSize / 8];
        byte[] cipher = new byte[fullCipher.Length - iv.Length];

        Array.Copy(fullCipher, iv, iv.Length);
        Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream(cipher))
        {
          using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
          {
            using (StreamReader sr = new StreamReader(cs))
              {
                return sr.ReadToEnd();
              }
          }
        }
      }
    }
  }
}

