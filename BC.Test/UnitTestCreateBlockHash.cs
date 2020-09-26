using BC11.Entities;
using BC11.Interfaces;
using BlockChainCourse.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace BC.Test
{
    [TestClass]
    public class UnitTestCreateBlockHash
    {

        //ITransaction txn5 = Arrange();
        private readonly static IKeyStore keyStore = new KeyStore(Hmac.GenerateKey());
        private readonly static int levelOfDifficult = 3;
        IBlock<ClaimSettlement> block1 = new Block(0, keyStore, levelOfDifficult);
        IBlock<ClaimSettlement> block2 = new Block(1, keyStore, levelOfDifficult);
        static readonly TransactionPool<ClaimSettlement> txnPool = new TransactionPool<ClaimSettlement>();

        private void Arrange()
        {
            var txn1 = new ClaimSettlement("ABC123", 1000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss);
            var txn2 = new ClaimSettlement("VBG345", 2000.00m, DateTime.Now, "JKH567", 20000, ClaimType.TotalLoss);
            var txn3 = new ClaimSettlement("XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000, ClaimType.TotalLoss);
            var txn4 = new ClaimSettlement("CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000, ClaimType.TotalLoss);

            txnPool.AddTransaction(txn1);
            txnPool.AddTransaction(txn2);
            txnPool.AddTransaction(txn3);
            txnPool.AddTransaction(txn4);

            block1.Add(txnPool.GetTransaction());
            block1.Add(txnPool.GetTransaction());

            block2.Add(txnPool.GetTransaction());
            block2.Add(txnPool.GetTransaction());
        }

        [TestMethod]
        public void TestComputeHash()
        {
            Arrange();
            var h1 = block1.CalculateBlockHash(null);
            Assert.AreEqual(32 * 2, h1.Length);
        }
    }
}