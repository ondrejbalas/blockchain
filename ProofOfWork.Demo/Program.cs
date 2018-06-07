using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProofOfWork.Demo.Components.Cryptography;
using ProofOfWork.Demo.Components.Hashing;
using ProofOfWork.Demo.Components.ProofOfWork;
using ProofOfWork.Demo.Components.Serialization;
using ProofOfWork.Demo.Models;
using ProofOfWork.Demo.Rules.BlockValidation;
using ProofOfWork.Demo.Rules.TransactionValidation;

namespace ProofOfWork.Demo
{
    class Program
    {
        /*
            Hello! This is demo code and should be treated as such.
            The architecture does not follow best practices and leaves
            out things like dependency injection to keep the code 
            accessible to the widest possible audience. 
        */

        internal static long GenesisTimeStamp = DateTime.Now.Ticks;
        internal static ConcurrentBag<Transaction> PendingTransactions = new ConcurrentBag<Transaction>();
        internal static ITransactionSerializer TransactionSerializer = new SimpleTransactionSerializer();
        internal static ISimpleHashProvider DefaultSimpleHashProvider = new ShortSimpleHashProvider();
        internal static ICryptoProvider CryptoProvider = new FakeEcdsaProvider();
        internal static ITransactionValidator TransactionValidator = new DefaultTransactionValidator();
        internal static IBlockValidator BlockValidator = new DefaultBlockValidator();
        internal static byte[] GenesisSpendingPrivateKey { get; set; }
        public static Chain Blockchain { get; set; }

        internal static Dictionary<byte[], byte[]> PrivateKeyLookup = new Dictionary<byte[], byte[]>();
        static void Main(string[] args)
        {
            Console.WriteLine("Ondrej Balas - Building a Blockchain");
            Console.WriteLine("Twitter: @ondrejbalas");
            Console.WriteLine("Website: https://ondrejbalas.com");
            Console.WriteLine("Github: https://github.com/ondrejbalas/blockchain");
            Console.WriteLine();
            Console.WriteLine();

            Blockchain = new Chain(); // Create the blockchain
            Blockchain.AddGenesisBlock(); // Add the genesis block

            var genesisBlock = (Blockchain.LastBlock as GenesisBlock);
            var genesisAddress = genesisBlock.Transactions.Single().Recipients.Single().Address;
            var genesisSpendKey = genesisBlock.SpendKey; // Get the genesis spend key (private key needed to spend coins from the genesis transaction)

            Miner.StartMining(Blockchain, 1);

            Console.ReadLine();
        }
    }
}
