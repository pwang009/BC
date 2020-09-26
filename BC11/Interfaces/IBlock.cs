using System;
using System.Collections.Generic;

namespace BC11.Interfaces
{
    public interface IBlock<T> where T : ITransaction
    {
        List<T> Transactions { get; }

        // Block header data
        int BlockNumber { get; }
        DateTime CreatedDate { get;}
        string BlockHash { get; }
        string PreviousBlockHash { get;}
        string BlockSignature { get; }
        int Difficulty { get; }
        int Nonce { get; }

        // Block Methods
        void Add(T t);
        string CalculateBlockHash(string previousBlockHash);
        void SetBlockHash(IBlock<T> parent);
        IBlock<T> NextBlock { get; set; }
        bool IsValidChain(string prevBlockHash, bool verbose);
        IKeyStore KeyStore { get; }
    }
}
