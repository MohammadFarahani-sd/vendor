namespace Framework.Core.Security.Cryptography;

public interface IHashProvider
{
    string GetSaltedHash(string value, string salt);
}