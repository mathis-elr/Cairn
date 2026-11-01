namespace Cairn.Application.Interfaces.Services
{
    public interface ICryptService
    {
        string Encrypt(string plainText, string masterKey);

        string Decrypt(string cipherText, string masterKey);
    }
}
