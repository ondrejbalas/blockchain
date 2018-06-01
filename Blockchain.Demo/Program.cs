using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blockchain.Demo.BoringStuff.Serialization;
using Blockchain.Demo.Components.Cryptography;
using Blockchain.Demo.Components.Hashing;
using Blockchain.Demo.Models;

namespace Blockchain.Demo
{
    class Program
    {
        internal static long GenesisTimeStamp = DateTime.Now.Ticks;
        internal static ConcurrentBag<Transaction> PendingTransactions = new ConcurrentBag<Transaction>();
        internal static ITransactionSerializer TransactionSerializer = new SimpleTransactionSerializer();
        internal static ISimpleHashProvider DefaultSimpleHashProvider = new ShortSimpleHashProvider();
        internal static ICryptoProvider CryptoProvider = new FakeEcdsaProvider();

        static void Main(string[] args)
        {
            Console.WriteLine("Ondrej Balas - Building a Blockchain");
            Console.WriteLine("Twitter: @ondrejbalas");
            Console.WriteLine("Website: https://ondrejbalas.com");
            Console.WriteLine("Github: https://github.com/ondrejbalas/blockchain");
            Console.WriteLine();
            Console.WriteLine();

            var blockchain = new Chain(); // Create the blockchain
            blockchain.AddGenesisBlock(); // Add the genesis block

            var genesisBlock = (blockchain.LastBlock as GenesisBlock);
            var pub1 = genesisBlock.Transactions.Single().Recipients.Single().Address;
            var priv1 = genesisBlock.SpendKey; // Get the genesis spend key (private key needed to spend coins from the genesis transaction)

            // Make a key for ourselves so we can send coins to it.
            CryptoProvider.GenerateKeyPair(out byte[] myPrivateKey, out byte[] myPublicKey);

            // Make a couple blocks
            var block1 = new Block(blockchain.LastBlock);
            var b1tx1 = new Transaction()
            {
                SourceAddress = pub1,
                Recipients = new List<TransactionTarget>() { new TransactionTarget() { Address = myPublicKey, Amount = 50m } }
            };

            b1tx1.Sign(priv1); // Comment this line and the transaction will be rejected by the blockchain.
            block1.Transactions.Add(b1tx1);
            blockchain.AddBlock(block1);

            var ledger = blockchain.GetLedger();
            ledger.Print();

            Console.ReadLine();
        }
    }
}
