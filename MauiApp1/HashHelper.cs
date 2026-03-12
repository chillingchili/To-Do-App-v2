using System.Security.Cryptography;
using System.Text;

namespace MauiApp1;

public static class HashHelper
{
    public static string Hash(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLower();
    }
}
