using System;

namespace Blockchain.Demo.Static
{
    public static class PrimitiveExtensions
    {
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static string ToBase64Short(this byte[] bytes)
        {
            return Program.DefaultSimpleHashProvider.CalculateHash(bytes).ToString();
        }
    }
}