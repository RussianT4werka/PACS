using System.Security.Cryptography;
using System.Text;

namespace PACS.Tools
{
    public class Hash
    {
        public static string HashPass(string pass)
        {
            var bytes = Encoding.UTF8.GetBytes(pass);
            var hashBytes = SHA256.Create().ComputeHash(bytes);
            return Encoding.UTF8.GetString(hashBytes);
        }
    }
}
