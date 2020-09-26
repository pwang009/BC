using BC.Utilities.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace BC21
{
    public interface IBitCoinValidationService
    {
        void ValidateBlockHash();
    }

    public class BitCoinValidationService : IBitCoinValidationService
    {
        private readonly Block _options;

        public BitCoinValidationService(IOptions<Block> options)
        {
            _options = options.Value;
        }

        public void ValidateBlockHash()
        {
            PrintBlockInfo();
            ValidateHash();
        }

        private void PrintBlockInfo()
        {
            Console.WriteLine($"Block Version Number: {_options.versionNumber}");
            Console.WriteLine($"Block Merkle Root Hash: {_options.MerkleRootHash}");
            Console.WriteLine($"Previous Block Hash: {_options.previousBlockHash}");
            Console.WriteLine($"Block Date and Time: {_options.blockDateTime}");
            Console.WriteLine($"nBits: {_options.nbits}");
            Console.WriteLine($"Nonce: {_options.nonce}");
        }

        private void ValidateHash()
        { 
            var s = _options.CombinedBlockStringToBeHashed.StringToByteArray().ComputeDoubleHashBySHA256();
            Console.WriteLine($"Block Hash: {s.ByteArrayToHex().StringSwapAndReverse()}");
        }
    }
}
