using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace password_manager 
{
  public class Password
  {
    public string Login {get; init;}
    public string PasswordValue {get; init;}

    private static readonly string _secretKey = "0000000000000052";

    public Password(string login, string password)
    {
      this.Login = login;
      this.PasswordValue = password;      
    }

    public static string Encrypt(string password)
    {
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(_secretKey);
        aes.IV = aes.IV;

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

        using (MemoryStream ms = new MemoryStream())
        {
          ms.Write(aes.IV, 0, aes.IV.Length);
          using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
          {
            using (StreamWriter sw = new StreamWriter(cs))
            {
              sw.Write(password);
            }
          }
          return Convert.ToBase64String(ms.ToArray());
        }
      }
    }
      
    public static string Decrypt(string cryptedPassword)
    {
      byte[] fullCipher = Convert.FromBase64String(cryptedPassword);
      using (Aes aes = Aes.Create())
      {
        aes.Key = Encoding.UTF8.GetBytes(_secretKey);

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

    public void print()
    {
      Console.WriteLine("Login: '" + Login + "'  Password: " + PasswordValue);
    }
  }
}
