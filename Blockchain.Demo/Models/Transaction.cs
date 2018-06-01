using System.Collections.Generic;
using Newtonsoft.Json;

namespace Blockchain.Demo.Models
{
    public class Transaction
    {
        public BlockType Type { get; set; } = BlockType.Normal;
        public byte[] SourceAddress { get; set; }
        public List<TransactionTarget> Recipients { get; set; }

        [JsonIgnore]
        public byte[] Signature { get; private set; }
        private bool _jsonIncludeSignature = true;

        public void Sign(byte[] privateKey)
        {
            var serialized = Serialize(false);
            Signature = Program.CryptoProvider.Sign(serialized, privateKey);
        }

        public bool Verify()
        {
            var serialized = Serialize(false);
            var result = Program.CryptoProvider.Verify(serialized, SourceAddress, Signature);
            return result;
        }

        private static byte[] Serialize(Transaction transaction)
        {
            return Program.TransactionSerializer.Serialize(transaction);
        }

        public byte[] Serialize(bool includeSignature)
        {
            _jsonIncludeSignature = includeSignature;
            var result = Serialize(this);
            _jsonIncludeSignature = true; // Kind of a hack, but by setting this to true normally, the signature is included as part of the hash when this is serialized as part of the List<Transaction> in Block.SerializeTransactions()
            return result;
        }
        
        public static Transaction CreateGenesisTransaction(out byte[] spendKey)
        {
            var provider = Program.CryptoProvider;
            
            provider.GenerateKeyPair(out byte[] priv0, out byte[] pub0);
            provider.GenerateKeyPair(out byte[] priv1, out byte[] pub1);
            spendKey = priv1;

            var newTx = new Transaction()
            {
                SourceAddress = pub0,
                Recipients = new List<TransactionTarget>()
                    { new TransactionTarget() { Amount = 100.0m, Address = pub1 } },
                Type = BlockType.Genesis
            };

            newTx.Sign(priv0);

            return newTx;
        }

    }
}