using BC.Utilities.Extensions;
using BC11.Entities;
using BC11.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BC.Test
{
    [TestClass]
    public class UnitTestBlockChain
    {

        private void ArrangeData()
        {
            var txn1 = new ClaimSettlement("ABC123", 1000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss);
            var txn2 = new ClaimSettlement("VBG345", 2000.00m, DateTime.Now, "JKH567", 20000, ClaimType.TotalLoss);
            var txn3 = new ClaimSettlement("XCF234", 3000.00m, DateTime.Now, "DH23ED", 30000, ClaimType.TotalLoss);
            var txn4 = new ClaimSettlement("CBHD45", 4000.00m, DateTime.Now, "DH34K6", 40000, ClaimType.TotalLoss);
        }

        [TestMethod]
        public void TestComputeHash()
        {
            var s = "Mary has a little lamb";
            var h = s.ComputeHashBySHA256();
            Assert.AreEqual(64, h.Length);
        }

        [TestMethod]
        public void TestSetBlockHash()
        {
            //Arrange
            var txn1 = new ClaimSettlement("ABC123", 1000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss);
            var txn2 = new ClaimSettlement("ABC123", 1000.00m, DateTime.Now, "QWE123", 10000, ClaimType.TotalLoss);
            IBlock<ClaimSettlement> block = new Block(0, 3);
            IBlockChain<ClaimSettlement> blockChain = new BlockChain<ClaimSettlement>();

            //Act
            block.Add(txn1);
            block.Add(txn2);
            blockChain.Add(block);

            //Assert
            Assert.AreEqual(32, block.BlockHash.Length);
        }
    }
}
