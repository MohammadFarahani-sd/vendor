using System.Security.Cryptography;
using System.Text;
using Framework.Core.Security.Cryptography;

namespace Framework.Security.Cryptography;

public class HashProvider : IHashProvider
{
    public string GetSaltedHash(string value, string salt)
    {
        var valueBytes = Encoding.UTF8.GetBytes(value);
        var saltBytes = Encoding.UTF8.GetBytes(salt);
        var hashBytes = GetSaltedHash(valueBytes, saltBytes);
        var builder = new StringBuilder();

        foreach (var t in hashBytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }

    private static byte[] GetSaltedHash(byte[] value, byte[] salt)
    {
        var algorithm = SHA256.Create();

        var valueWithSaltBytes = new byte[value.Length + salt.Length];

        for (var i = 0; i < value.Length; i++)
        {
            valueWithSaltBytes[i] = value[i];
        }

        for (var i = 0; i < salt.Length; i++)
        {
            valueWithSaltBytes[value.Length + i] = salt[i];
        }

        return algorithm.ComputeHash(valueWithSaltBytes);
    }
}