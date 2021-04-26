
using System.Numerics;
using System.Security.Cryptography;
namespace Utils
{
    public class RequestId
    {
        private const string digits = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";

        public string Base58
        {
            get; set;
        }

        public string Id
        {
            get => $"req_{Base58}";
        }

        public RequestId()
        {
            Base58 = Generate();
        }

        // static to guarantee crypthographic randomness of the generated bytes
        private readonly RNGCryptoServiceProvider rng = new();

        private string Generate(int length = 16)
        {
            var data = new byte[length];
            rng.GetBytes(data);

            // Decode byte[] to BigInteger
            BigInteger intData = 0;
            for (int i = 0; i < data.Length; i++)
            {
                intData = intData * 256 + data[i];
            }

            // Encode BigInteger to Base58 string
            var result = string.Empty;
            while (intData > 0)
            {
                var remainder = (int)(intData % 58);
                intData /= 58;
                result = digits[remainder] + result;
            }

            // Append `1` for each leading 0 byte
            for (int i = 0; i < data.Length && data[i] == 0; i++)
            {
                result = '1' + result;
            }

            return result;
        }


    }
};