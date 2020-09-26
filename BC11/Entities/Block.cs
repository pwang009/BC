using BC11.Interfaces;
using BlockChainCourse.Cryptography;
using Clifton.Blockchain;
using System;
using System.Text;

namespace BC11.Entities
{
    public class Block : BlockBase<ClaimSettlement>
    {

        private MerkleTree merkleTree = new MerkleTree();

        public Block(int blockNumber, int miningDifficulty)
            : base(blockNumber, miningDifficulty) {}

        public Block(int blockNumber, IKeyStore keyStore, int miningDifficulty)
            : base(blockNumber, miningDifficulty, keyStore) { }

        //public override string CreateHashString() =>
        //    String.Empty;


    }
}
