using BC11.Entities;
using BC11.Interfaces;
using BlockChainCourse.Cryptography;
using System;

namespace BC11
{
    class Program
    {
        static readonly TransactionPool<ClaimSettlement> txnPool = new TransactionPool<ClaimSettlement>();
        static void Main(string[] args)
        {
            ITransaction txn5 = SetupTransactions();
            IKeyStore keyStore = new KeyStore(Hmac.GenerateKey());
            int levelOfDifficult = 3;
            IBlock<ClaimSettlement> block1 = new Block(0, keyStore, levelOfDifficult);
            IBlock<ClaimSettlement> block2 = new Block(1, keyStore, levelOfDifficult);
            IBlock<ClaimSettlement> block3 = new Block(2, keyStore, levelOfDifficult);
            IBlock<ClaimSettlement> block4 = new Block(3, keyStore, levelOfDifficult);
            AddTransactionsToBlocksAndCalculateHashes(block1, block2, block3, block4);

            var chain = new BlockChain<ClaimSettlement>();
            chain.Add(block1);
            chain.Add(block2);
            chain.Add(block3);
            chain.Add(block4);

            if (chain.Verify())
                Console.WriteLine("chain verified");
            else
                Console.WriteLine("chain verification failed");
        }

        private static ITransaction SetupTransactions()
        {
            var txn1 = new ClaimSettlement("ABC123", 1000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss);
            var txn2 = new ClaimSettlement("VBG345", 2000.00m, DateTime.Now, "JKH567", 20000, ClaimType.TotalLoss);
            var txn3 = new ClaimSettlement("XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000, ClaimType.TotalLoss);
            var txn4 = new ClaimSettlement("CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000, ClaimType.TotalLoss);
            var txn5 = new ClaimSettlement("AJD345", 5000.00m, DateTime.Now, "28FNF4", 50000, ClaimType.TotalLoss);
            var txn6 = new ClaimSettlement("QAX367", 6000.00m, DateTime.Now, "FJK676", 60000, ClaimType.TotalLoss);
            var txn7 = new ClaimSettlement("CGO444", 7000.00m, DateTime.Now, "LKU234", 70000, ClaimType.TotalLoss);
            var txn8 = new ClaimSettlement("PLO254", 8000.00m, DateTime.Now, "VBN456", 80000, ClaimType.TotalLoss);
            var txn9 = new ClaimSettlement("ABC123", 1000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss);
            var txn10 = new ClaimSettlement("VBG345", 2000.00m, DateTime.Now, "JKH567", 20000, ClaimType.TotalLoss);
            var txn11 = new ClaimSettlement("XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000, ClaimType.TotalLoss);
            var txn12 = new ClaimSettlement("CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000, ClaimType.TotalLoss);
            var txn13 = new ClaimSettlement("AJD345", 5000.00m, DateTime.Now, "28FNF4", 50000, ClaimType.TotalLoss);
            var txn14 = new ClaimSettlement("QAX367", 6000.00m, DateTime.Now, "FJK676", 60000, ClaimType.TotalLoss);
            var txn15 = new ClaimSettlement("CGO444", 7000.00m, DateTime.Now, "LKU234", 70000, ClaimType.TotalLoss);
            var txn16 = new ClaimSettlement("PLO254", 8000.00m, DateTime.Now, "VBN456", 80000, ClaimType.TotalLoss);

            txnPool.AddTransaction(txn1);
            txnPool.AddTransaction(txn2);
            txnPool.AddTransaction(txn3);
            txnPool.AddTransaction(txn4);
            txnPool.AddTransaction(txn5);
            txnPool.AddTransaction(txn6);
            txnPool.AddTransaction(txn7);
            txnPool.AddTransaction(txn8);
            txnPool.AddTransaction(txn9);
            txnPool.AddTransaction(txn10);
            txnPool.AddTransaction(txn11);
            txnPool.AddTransaction(txn12);
            txnPool.AddTransaction(txn13);
            txnPool.AddTransaction(txn14);
            txnPool.AddTransaction(txn15);
            txnPool.AddTransaction(txn16);

            return txn5;
        }

        private static void AddTransactionsToBlocksAndCalculateHashes(
            IBlock<ClaimSettlement> block1, 
            IBlock<ClaimSettlement> block2, 
            IBlock<ClaimSettlement> block3, 
            IBlock<ClaimSettlement> block4)
        {
            block1.Add(txnPool.GetTransaction());
            block1.Add(txnPool.GetTransaction());
            block1.Add(txnPool.GetTransaction());
            block1.Add(txnPool.GetTransaction());

            block2.Add(txnPool.GetTransaction());
            block2.Add(txnPool.GetTransaction());
            block2.Add(txnPool.GetTransaction());
            block2.Add(txnPool.GetTransaction());

            block3.Add(txnPool.GetTransaction());
            block3.Add(txnPool.GetTransaction());
            block3.Add(txnPool.GetTransaction());
            block3.Add(txnPool.GetTransaction());

            block4.Add(txnPool.GetTransaction());
            block4.Add(txnPool.GetTransaction());
            block4.Add(txnPool.GetTransaction());
            block4.Add(txnPool.GetTransaction());

            block1.SetBlockHash(null);
            block2.SetBlockHash(block1);
            block3.SetBlockHash(block2);
            block4.SetBlockHash(block3);
        }
    }
}
