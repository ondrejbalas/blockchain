using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProofOfWork.Demo.Models;

namespace ProofOfWork.Demo.Components.ProofOfWork
{
    public class Miner
    {
        private readonly Random Rng = new Random();
        private static object obj = new object();
        
        public static void StartMining(Chain chain, int miners)
        {
            for (int i = 0; i < miners; i++)
            {
                var i1 = i;
                Task.Factory.StartNew(() =>
                {
                    var miner = new Miner();
                    miner.Mine(chain, i1);
                });
                Thread.Sleep(250);
            }
        }

        private void Mine(Chain chain, int i)
        {
            Program.CryptoProvider.GenerateKeyPair(out byte[] priv0, out byte[] pub0);
            Program.CryptoProvider.GenerateKeyPair(out byte[] priv1, out byte[] pub1);
            Program.PrivateKeyLookup.Add(pub1, priv1);

            while (true)
            {
                var work = chain.GetWork();
                
                decimal? result = null;
                int nonce = 0;

                Block block = null;

                while (result == null)
                {
                    Thread.Sleep(100);
                    lock (obj)
                    {
                        block = new Block(chain.LastBlock) { Type = BlockType.ProofOfWork };

                        // Increment nonce
                        block.Nonce = nonce++;

                        // add mining reward transaction
                        var miningRewardTransaction = new Transaction()
                        {
                            SourceAddress = pub0,
                            Recipients =
                                new List<TransactionTarget>() { new TransactionTarget() { Amount = 100.0m, Address = pub1 } },
                            Type = BlockType.ProofOfWork
                        };
                        miningRewardTransaction.Sign(priv0);
                        block.Transactions.Add(miningRewardTransaction);

                        foreach (var pendingTransactions in Program.PendingTransactions)
                        {
                            block.Transactions.Add(pendingTransactions);
                        }

                        result = work.AttemptHash(block.Hash());

                        if (result != null)
                        {
                            chain.AddBlock(block);
                            nonce = 0; // success. reset nonce.
                        }
                    }
                }
            }
        }
    }
}