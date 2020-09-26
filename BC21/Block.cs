using BC.Utilities.Extensions;
using System;

namespace BC21
{
    public interface IBlock
    {
        string CombinedBlockStringToBeHashed { get; }
        void ValidateBlockHash();
    }

    public class Block : IBlock
    {

        public string versionNumber { get; set; }
        public string previousBlockHash { get; set; }
        public string MerkleRootHash { get; set; }
        public string blockDateTime { get; set; }
        public string nbits { get; set; }
        public string nonce { get; set; }

        public string CombinedBlockStringToBeHashed
        {
            get =>
               versionNumber.HexStringToInteger().IntegerToHex().StringSwapAndReverse() +
                    previousBlockHash.StringSwapAndReverse() +
                    MerkleRootHash.StringSwapAndReverse() +
                    blockDateTime.DateTimeStringToEpoch().IntegerToHex().StringSwapAndReverse() +
                    nbits.IntegerStringToInteger().IntegerToHex().StringSwapAndReverse() +
                    nonce.IntegerStringToInteger().IntegerToHex().StringSwapAndReverse();
        }

        public void ValidateBlockHash()
        {
            var s = CombinedBlockStringToBeHashed.StringToByteArray().ComputeDoubleHashBySHA256();
            PrintBlockInfo();
            Console.WriteLine($"Block Hash: {s.ByteArrayToHex().StringSwapAndReverse()}");
        }

        private void PrintBlockInfo()
        {
            Console.WriteLine($"Block Version Number: {versionNumber}");
            Console.WriteLine($"Block Merkle Root Hash: {MerkleRootHash}");
            Console.WriteLine($"Previous Block Hash: {previousBlockHash}");
            Console.WriteLine($"Block Date and Time: {blockDateTime}");
            Console.WriteLine($"nBits: {nbits}");
            Console.WriteLine($"Nonce: {nonce}");
        }

    }
}
