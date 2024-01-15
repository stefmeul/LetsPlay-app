using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using K4os.Compression.LZ4.Engine;

namespace LetsPlay_app
{
    internal class Encrypt
    {
        internal static string HashString(string passwordString) // or 'public static string' if database not on localhost?
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(passwordString))
                    sb.Append(b.ToString("X3"));
                return sb.ToString();
            
        }

        public static byte[] GetHash(string passwordString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(passwordString));
                      
        }
    }
}
