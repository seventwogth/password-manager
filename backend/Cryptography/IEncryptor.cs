namespace PManager.Cryptography
{
    public interface IEncryptor
    {
        string Encrypt(string text);

        string Decrypt(string cryptedText);
    }
}
