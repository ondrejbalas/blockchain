using System;
using System.Linq;
using ProofOfWork.Demo.Models;

namespace ProofOfWork.Demo.Rules.TransactionValidation
{
    public class DefaultTransactionValidator : ITransactionValidator
    {
        public bool IsValid(Transaction tx)
        {
            var result = tx.Verify();

            switch (tx.Type)
            {
                case BlockType.Genesis:
                case BlockType.ProofOfWork:
                case BlockType.ProofOfStake:
                    result = result && tx.Recipients.Single().Amount == 100m;
                    break;
                case BlockType.Normal:
                    var ledger = Program.Blockchain.GetLedger();

                    var sourceAmount = ledger.LedgerEntries[tx.SourceAddress];
                    var spendAmount = tx.Recipients.Sum(r => r.Amount);
                    if (spendAmount > sourceAmount)
                    {
                        return false;
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }

            return result;
        }
    }
}