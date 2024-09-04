using System.Text;
using ComputerUtils.Encryption;

namespace craft.Users;

public class CryptographicsHelper
{
    public static string GetHash(string input)
    {
        return Hasher.GetSHA256OfString(input).ToLower();
    }

    public static string GetRandomString()
    {
        Random random = new Random();
        int length = random.Next(4, 6);
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            int index = random.Next(0, chars.Length);
            sb.Append(chars[index]);
        }
        return sb.ToString();
    }
}