using System;
using System.Collections.Generic;
using System.Linq;
using ProofOfWork.Demo.Components.Ledger;
using ProofOfWork.Demo.Components.ProofOfWork;
using ProofOfWork.Demo.Static;

namespace ProofOfWork.Demo.Models
{
    public class Chain
    {
        // The constants below define how this chain functions.
        private const decimal TargetSecondsPerBlock = 0.5m;
        private const int BlocksToEvaluate = 10;
        private Block GenesisBlock => Models.GenesisBlock.Create();
        public Block LastBlock => Blocks.Last();

        public List<Block> Blocks { get; } = new List<Block>();

        public Chain()
        {
            Console.WriteLine($"Targetting {TargetSecondsPerBlock} seconds per block.");
            Console.WriteLine($" + DIFF: Minimim difficulty adjusted to {work}");
        }

        public void AddGenesisBlock()
        {
            Blocks.Add(GenesisBlock);
        }

        public bool AddBlock(Block block)
        {
            bool isValid = block.Transactions.All(tx => tx.Verify());

            if (block.Type == BlockType.ProofOfWork)
            {
                var w = GetWork();
                IncrementWork();
                var h = block.Hash();
                var result = w.AttemptHash(h);
                if (result != null) block.HashDifficulty = result.Value;
                isValid = isValid && result != null;
            }

            isValid = isValid && Program.BlockValidator.IsValid(block);

            if (isValid)
            {
                Blocks.Add(block);
                Console.WriteLine($" + MINE [{DateTime.Now:hh:MM:ss}] Success! Adding block {block.Index} to chain with difficulty: {block.HashDifficulty}. Winner: {block.Transactions.Single(x => x.Type == BlockType.ProofOfWork).Recipients.Single().Address.ToBase64Short()}");
            }

            return isValid;
        }

        public Ledger GetLedger()
        {
            return Ledger.Create(this);
        }

        public Work GetWork()
        {
            return new Work(work);
        }

        private decimal work = 2;
        private int lastIncrementIndex = 0;
        private void IncrementWork()
        {
            if (lastIncrementIndex == LastBlock.Index) return;
            if (Blocks.Count >= BlocksToEvaluate + 1 && (Blocks.Count % BlocksToEvaluate == 1)) // The mod operation is so we only recalculate every 10 blocks.
            {
                lastIncrementIndex = LastBlock.Index;
                var firstEvaluationBlockTime = Blocks[Blocks.Count - 11].CreatedTimestamp;  // Count 12; We want 11 blocks from index 1 to 11.
                var lastEvaluationBlockTime = Blocks[Blocks.Count - 1].CreatedTimestamp;

                //var workEvaluationBlocks = Enumerable.Reverse(Blocks).Take(BlocksToEvaluate + 1).ToList();

                decimal totalTargetTime = BlocksToEvaluate * TargetSecondsPerBlock;
                decimal actualTime = ((decimal)lastEvaluationBlockTime - firstEvaluationBlockTime) / 10000000;

                decimal prevWork = work;
                work *= (totalTargetTime / actualTime);

                if (prevWork != work) Console.WriteLine($" + DIFF [T:{totalTargetTime:0.00}s A:{actualTime:0.00}s] Minimum difficulty adjusted from {prevWork:0.000000} to: {work:0.000000}");

                var ledger = GetLedger();
                ledger.Print();
            }
        }
    }
}