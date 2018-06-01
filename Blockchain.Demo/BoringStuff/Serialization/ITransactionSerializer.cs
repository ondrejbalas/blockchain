using System.Collections.Generic;
using Blockchain.Demo.Models;

namespace Blockchain.Demo.BoringStuff.Serialization
{
    /// <summary>
    /// Serialize a list of transactions.
    /// </summary>
    /// <param name="transactions"></param>
    /// <returns>A binary representation of the provided transactions.</returns>
    public interface ITransactionSerializer
    {
        byte[] Serialize(Transaction transaction);
        byte[] Serialize(List<Transaction> transactions);
    }
}