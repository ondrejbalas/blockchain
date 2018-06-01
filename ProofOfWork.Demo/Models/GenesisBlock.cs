using System.Collections.Generic;
using System.IO;

namespace ProofOfWork.Demo.Models
{
    public class GenesisBlock : Block
    {
        public byte[] SpendKey { get; set; }

        public static GenesisBlock Create()
        {
            var result = new GenesisBlock()
            {
                Index = 0,
                CreatedTimestamp = Program.GenesisTimeStamp,
                Transactions = new List<Transaction>()
                {
                    Transaction.CreateGenesisTransaction(out byte[] spendKey)
                },
                Type = BlockType.Genesis,
                SpendKey = spendKey
            };
            return result;
        }

        public override uint Hash()
        {
            var t = SerializeTransactions(Transactions);
            var b = new byte[12 + t.Length];

            using (var bw = new BinaryWriter(new MemoryStream(b)))
            {
                bw.Write(Index);
                bw.Write(CreatedTimestamp);
                bw.Write(t);
            }

            return Program.DefaultSimpleHashProvider.CalculateHash(b);
        }
    }
}