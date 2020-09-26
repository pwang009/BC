using BC11.Interfaces;
using System.Collections.Generic;

namespace BC11.Entities
{
    public class BlockChain<T> : IBlockChain<T> where T : ITransaction
    {
        public IBlock<T> CurrentBlock { get; private set; }
        public IBlock<T> HeadBlock { get; private set; }

       private List<IBlock<T>> Blocks;

        public int NextBlockNumber {
            get => HeadBlock == null ? 0 : CurrentBlock.BlockNumber + 1;
        }

        public BlockChain() =>
            Blocks = new List<IBlock<T>>();

        public void Add(IBlock<T> block)
        {
            block.SetBlockHash(CurrentBlock);
            if (HeadBlock == null)
            {
                //block.PreviousBlockHash = null;
                HeadBlock = block;
            }
            Blocks.Add(block);
            CurrentBlock = block;
        }

        public bool Verify() =>
            HeadBlock.IsValidChain(null, true);
    }
}
