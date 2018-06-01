using System;

namespace ProofOfWork.Demo.Static
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
            //var s = Convert.ToBase64String(bytes);
            //return .Substring();
        }
    }
}