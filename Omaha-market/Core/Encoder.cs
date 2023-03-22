using System.Security.Cryptography;
using System.Text;

namespace Omaha_market.Core
{
    public static class Encoder
    {
        public static string Encode(IConfiguration config,string str)
        {
            SHA256 hm = SHA256.Create();
            
            byte[] result = hm.ComputeHash(Encoding.UTF8.GetBytes(str));
            return BitConverter.ToString(result).Replace("-", "") + $"{config["Encoder:SecurityKey"]}";
        }

    }
}
