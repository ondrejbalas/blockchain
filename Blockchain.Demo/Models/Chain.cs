using System;
using System.Collections.Generic;
using System.Linq;
using Blockchain.Demo.Components.Ledger;

namespace Blockchain.Demo.Models
{
    public class Chain
    {
        // The constants below define how this chain functions.
        private const decimal TargetSecondsPerBlock = 0.5m;
        private const int BlocksToEvaluate = 10;
        private Block GenesisBlock => Models.GenesisBlock.Create();
        public Block LastBlock => Blocks.Last();

        public List<Block> Blocks { get; } = new List<Block>();

        public void AddGenesisBlock()
        {
            Blocks.Add(GenesisBlock);
        }

        public bool AddBlock(Block block)
        {
            var allTransactionsValid = block.Transactions.All(tx => tx.Verify());

            if (allTransactionsValid)
            {
                Blocks.Add(block);
                Console.WriteLine($"Block {block.Index} added");
                return true;
            }

            Console.WriteLine($"Block {block.Index} rejected");
            return false;
        }

        public Ledger GetLedger()
        {
            return Ledger.Create(this);
        }
    }
}