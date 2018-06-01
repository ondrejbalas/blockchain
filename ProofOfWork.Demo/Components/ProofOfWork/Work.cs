using System;

namespace ProofOfWork.Demo.Components.ProofOfWork
{
    public class Work
    {
        public int LeadingZeroes { get; set; } = 2;
        public decimal MinimumDifficulty { get; private set; }

        public Work(decimal minimumDifficulty)
        {
            MinimumDifficulty = minimumDifficulty;
        }

        public decimal? AttemptHash(uint hash)
        {
            bool result = true;

            // Check for leading zeroes
            for (int i = 0; i < LeadingZeroes; i++)
            {
                result = result && !IsBitSet(hash, i);
            }

            if (!result) return null;

            // Check for difficulty
            var maxDifficulty = GetCurrentMostDifficultTarget();
            decimal actualDifficulty = (decimal)(maxDifficulty / (double)Math.Abs(hash));

            if (actualDifficulty > MinimumDifficulty)
            {
                return actualDifficulty;
            }

            return null;
        }

        private uint GetCurrentMostDifficultTarget()
        {
            uint result = 0;
            for (int i = LeadingZeroes; i < 32; i++)
            {
                result |= (uint)(1 << (31-i));
            }

            return result;
        }

        // Source https://stackoverflow.com/a/2431759
        private bool IsBitSet(uint i, int pos)
        {
            return (i & (1 << (31 - pos))) != 0;
        }
    }
}