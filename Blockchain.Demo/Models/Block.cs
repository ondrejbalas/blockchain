using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Blockchain.Demo.Models
{
    public class Block
    {
        public int Index { get; protected set; }
        public long CreatedTimestamp { get; set; }
        public int Nonce { get; set; }
        public List<Transaction> Transactions { get; protected set; } = new List<Transaction>();
        [JsonIgnore] public Block PreviousBlock { get; }
        public uint PreviousHash { get; }
        public BlockType Type { get; set; }

        public Block(Block previousBlock)
        {
            PreviousBlock = previousBlock;
            Index = previousBlock.Index + 1;
            CreatedTimestamp = DateTime.Now.Ticks;
            PreviousHash = PreviousBlock.Hash();
        }

        protected Block()
        {
        }

        public virtual uint Hash()
        {
            var t = SerializeTransactions(Transactions);
            var b = new byte[20 + t.Length];

            using (var bw = new BinaryWriter(new MemoryStream(b)))
            {
                bw.Write(Index);
                bw.Write(CreatedTimestamp);
                bw.Write(t);
                bw.Write(PreviousHash);
                bw.Write(Nonce);
            }

            return Program.DefaultSimpleHashProvider.CalculateHash(b);
        }

        protected static byte[] SerializeTransactions(List<Transaction> transactions)
        {
            return Program.TransactionSerializer.Serialize(transactions);
        }
    }
}