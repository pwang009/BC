using System;
using System.Collections.Generic;
using System.Text;

namespace BC11.Interfaces
{
    public interface IBlockChain<T> where T : ITransaction
    {
        void Add(IBlock<T> block);
        bool Verify();
    }
}
