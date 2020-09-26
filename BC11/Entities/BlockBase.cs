using BC.Utilities.Extensions;
using BC11.Interfaces;
using BlockChainCourse.Cryptography;
using Clifton.Blockchain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BC11.Entities
{
    public abstract class BlockBase<T> : IBlock<T> where T : ITransaction
    {
        public List<T> Transactions { get; }
        public int BlockNumber { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public string BlockHash { get; private set; }
        public string PreviousBlockHash { get; private set; }
        public string BlockSignature { get; private set; }
        public int Difficulty { get; private set; }
        public int Nonce { get; private set; }
        public IBlock<T> NextBlock { get; set; }
        private MerkleTree merkleTree;
        public IKeyStore KeyStore { get; private set; }

        private string blockHashString {
            get => BlockNumber + CreatedDate.ToString() + PreviousBlockHash + merkleTree.RootNode;
        }

        public BlockBase(int blockNumber, int miningDifficulty) =>
            (BlockNumber, CreatedDate, Difficulty, merkleTree, Transactions) =
            (blockNumber, DateTime.UtcNow, miningDifficulty, new MerkleTree(), new List<T>());

        public BlockBase(int blockNumber, int miningDifficulty, IKeyStore keyStore)
            : this(blockNumber, miningDifficulty) => 
            (KeyStore) = (keyStore);

        private string CreateHashString(string prevBlockHash) => blockHashString.ComputeHashBySHA256();
        private string CreateHashString(string prevBlockHash, byte[] key) => blockHashString.ComputeHMACHashBySHA256(key);

        public void Add(T transaction)
        {
            Transactions.Add(transaction);
        }

        public string CalculateBlockHash(string prevBlockHash) =>
           KeyStore == null ?
            CreateHashString(prevBlockHash) : CreateHashString(prevBlockHash, KeyStore.AuthenticatedHashKey);

        public void SetBlockHash(IBlock<T> parent)
        {
            if (parent == null)
                //genesis block
                PreviousBlockHash = null;
            else
            {
                parent.NextBlock = this;
                PreviousBlockHash = parent.BlockHash;
            }

            //build MerkleTree
            foreach (ITransaction txn in Transactions)
                merkleTree.AppendLeaf(MerkleHash.Create(txn.ComputeTransactionHash()));
            merkleTree.BuildTree();

            // create block hash
            BlockHash = CalculateBlockHash(PreviousBlockHash);

            //if (KeyStore != null) BlockSignature = BlockHash.ToSignedDataBySHA256();
            if (KeyStore != null) BlockSignature = KeyStore.SignBlock(BlockHash);
        }

        public string CalculateProofOfWork(string blockHash)
        {
            string difficulty = DifficultyString();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            while (true)
            {
                string hashedData = Convert.ToBase64String(HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(Nonce + blockHash)));

                if (hashedData.StartsWith(difficulty, StringComparison.Ordinal))
                {
                    stopWatch.Stop();
                    TimeSpan ts = stopWatch.Elapsed;

                    // Format and display the TimeSpan value.
                    string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

                    Console.WriteLine("Difficulty Level " + Difficulty + " - Nonce = " + Nonce + " - Elapsed = " + elapsedTime + " - " + hashedData);
                    return hashedData;
                }

                Nonce++;
            }
        }

        private string DifficultyString()
        {
            string difficultyString = string.Empty;

            for (int i = 0; i < Difficulty; i++)
            {
                difficultyString += "0";
            }

            return difficultyString;
        }

        public bool IsValidChain(string prevBlockHash, bool verbose)
        {
            bool isValid = true;
            bool validSignature = false;

            BuildMerkleTree();

            validSignature = KeyStore.VerifyBlock(BlockHash, BlockSignature);

            // Is this a valid block and transaction
            //string newBlockHash = CalculateBlockHash(prevBlockHash);
            string newBlockHash = Convert.ToBase64String(
                HashData.ComputeHashSha256(Encoding.UTF8.GetBytes(Nonce + CalculateBlockHash(prevBlockHash))));

            validSignature = KeyStore.VerifyBlock(newBlockHash, BlockSignature);

            if (newBlockHash != BlockHash)
            {
                isValid = false;
            }
            else
            {
                // Does the previous block hash match the latest previous block hash
                isValid |= PreviousBlockHash == prevBlockHash;
            }

            PrintVerificationMessage(verbose, isValid, validSignature);

            // Check the next block by passing in our newly calculated blockhash. This will be compared to the previous
            // hash in the next block. They should match for the chain to be valid.
            if (NextBlock != null)
            {
                return NextBlock.IsValidChain(newBlockHash, verbose);
            }

            return isValid;
        }

        private void BuildMerkleTree()
        {
            merkleTree = new MerkleTree();

            foreach (ITransaction txn in Transactions)
            {
                merkleTree.AppendLeaf(MerkleHash.Create(txn.ComputeTransactionHash()));
            }

            merkleTree.BuildTree();
        }

        private void PrintVerificationMessage(bool verbose, bool isValid, bool validSignature)
        {
            if (verbose)
            {
                if (!isValid)
                {
                    Console.WriteLine("Block Number " + BlockNumber + " : FAILED VERIFICATION");
                }
                else
                {
                    Console.WriteLine("Block Number " + BlockNumber + " : PASS VERIFICATION");
                }

                if (!validSignature)
                {
                    Console.WriteLine("Block Number " + BlockNumber + " : Invalid Digital Signature");
                }
            }
        }

    }
}
