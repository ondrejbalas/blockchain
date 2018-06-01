using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;
using ProofOfWork.Demo.Models;

namespace ProofOfWork.Demo.Components.Serialization
{
    /// <inheritdoc cref="ITransactionSerializer"/>
    public class SimpleTransactionSerializer : ITransactionSerializer
    {
        public byte[] Serialize(Transaction transaction)
        {
            var json = JsonConvert.SerializeObject(transaction); // Serialize the entire list to a JSON string.
            var bytes = Encoding.UTF8.GetBytes(json); // Use UTF8 encoding to encode into byte array.
            var compressed = Compress(bytes);
            return compressed;
        }

        /// <summary>
        /// Implemented by converting serializing transactions to JSON and then compressing the JSON.
        /// In the real world I would compare this to other serialization methods and choose the method
        /// that resulted in the most compact serialized transactions while also considering CPU and memory
        /// usage of the algorithm.
        /// </summary>
        /// <param name="transactions">The transactions to serialize.</param>
        public byte[] Serialize(List<Transaction> transactions)
        {
            var json = JsonConvert.SerializeObject(transactions); // Serialize the entire list to a JSON string.
            var bytes = Encoding.UTF8.GetBytes(json); // Use UTF8 encoding t
            var compressed = Compress(bytes);
            return compressed;
        }

        private static byte[] Compress(byte[] data)
        {
            using (var msi = new MemoryStream(data))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return mso.ToArray();
            }
        }
    }
}