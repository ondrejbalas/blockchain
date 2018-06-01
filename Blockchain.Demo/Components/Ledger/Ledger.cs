using System;
using System.Collections.Generic;
using System.Linq;
using Blockchain.Demo.Models;
using Blockchain.Demo.Static;

namespace Blockchain.Demo.Components.Ledger
{
    public class Ledger
    {
        public Dictionary<byte[], decimal> LedgerEntries { get; } = new Dictionary<byte[], decimal>();

        private Ledger()
        {
        }

        public void Print()
        {
            Console.WriteLine("=== LEDGER START ===");
            foreach (var entry in LedgerEntries)
            {
                Console.WriteLine($"    {entry.Key.ToBase64Short()} {entry.Value:0.00000000}");
            }
            Console.WriteLine("=== LEDGER END ===");
        }

        private void IncrementAddress(byte[] address, decimal amount)
        {
            if (LedgerEntries.ContainsKey(address))
            {
                LedgerEntries[address] += amount;
            }
            else
            {
                LedgerEntries[address] = amount;
            }
        }

        public static Ledger Create(Chain chain)
        {
            var ledger = new Ledger();

            for (int i = 0; i < chain.Blocks.Count; i++)
            {
                var block = chain.Blocks[i];
                foreach (var tx in block.Transactions)
                {
                    if (tx.Verify())
                    {
                        switch (tx.Type)
                        {
                            case BlockType.Genesis:
                            case BlockType.ProofOfWork:
                                ledger.IncrementAddress(tx.Recipients.Single().Address, tx.Recipients.Single().Amount);

                                break;
                            case BlockType.Normal:
                                var sourceAmount = ledger.LedgerEntries[tx.SourceAddress];
                                var spendAmount = tx.Recipients.Sum(r => r.Amount);
                                if (spendAmount > sourceAmount)
                                {
                                    throw new Exception("Ledger is invalid");
                                }

                                ledger.LedgerEntries[tx.SourceAddress] -= spendAmount;
                                foreach (var recipient in tx.Recipients)
                                {
                                    ledger.IncrementAddress(recipient.Address, recipient.Amount);
                                }
                                break;
                        }
                    }
                }
            }
            return ledger;
        }
    }
}