namespace PManager.Cryptography.Interfaces
{
    public interface IEncryptor
    {
        string Encrypt(string text);

        string Decrypt(string cryptedText);
    }
}
