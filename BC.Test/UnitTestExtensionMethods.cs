using BC.Utilities.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace BC.Test
{

    //BitCoin Block #503266
    //http://trogers.net/2018/01/29/how-to-validate-a-bitcoin-blocks-proof-of-work-c/
    [TestClass]
    public class UnitTestExtensionMethods
    {
        string versionNumber = "0x20000000";
        string previousBlockHash = "00000000000000000033972c9263e9f1b593a4a555157ecc97735bf2fdea7664";
        string MerkleRootHash = "ad873aebe594e6fa58c828b131d397c4d804355007499f8c9ad8e1a9e000b21a";
        string blockDateTime = "2018-01-09 01:40:31";
        string bits = "402690497";
        string nonce = "3801010522";

        string expectedCombinedBlockStringToBeHashed = 
            "000000206476eafdf25b7397cc7e1555a5a493b5f1e963922c97330000000000000000001ab200e0a9e" +
            "1d89a8c9f4907503504d8c497d331b128c858fae694e5eb3a87ad8f1d545ac19100185ad18ee2";

        private string combinedBlockStringToBeHashed {
            get =>
                versionNumber.HexStringToInteger().IntegerToHex().StringSwapAndReverse() +
                previousBlockHash.StringSwapAndReverse() +
                MerkleRootHash.StringSwapAndReverse() +
                blockDateTime.DateTimeStringToEpoch().IntegerToHex().StringSwapAndReverse() +
                bits.IntegerStringToInteger().IntegerToHex().StringSwapAndReverse() +
                nonce.IntegerStringToInteger().IntegerToHex().StringSwapAndReverse();
                }

        string expectedBlockHash = "000000000000000000231de1aca4a1d5f31d52ee57bf532a68c44aea790b420d";

        [TestMethod]
        public void UnitTestIntegerToHexConversion()
        {
            var i = 255;
            Assert.AreEqual("ff", i.IntegerToHex());
        }

        [TestMethod]
        public void TestBlockHashLength()
        {
            Assert.AreEqual(32 * 2, expectedBlockHash.Length);
        }

        [TestMethod]
        public void UnitTestLittleEndianConversionVersionNumber()
        {
            Assert.AreEqual("00000020", 
                versionNumber.HexStringToInteger().IntegerToHex().StringSwapAndReverse());

            //Assert.AreEqual("00000020", versionNumber.ToLittleEndian('v'));
        }

        [TestMethod]
        public void UnitTestLittleEndianBlockHashConversion()
        {
            Assert.AreEqual("6476eafdf25b7397cc7e1555a5a493b5f1e963922c9733000000000000000000",
                previousBlockHash.StringSwapAndReverse());
        }

        [TestMethod]
        public void UnitTestLittleEndianDataTimeConversion()
        {
            Assert.AreEqual("8f1d545a", 
                blockDateTime.DateTimeStringToEpoch().IntegerToHex().StringSwapAndReverse());
        }

        [TestMethod]
        public void UnitTestLittleEndianBitsConversion()
        {
            Assert.AreEqual("c1910018", 
                bits.IntegerStringToInteger().IntegerToHex().StringSwapAndReverse());
        }

        [TestMethod]
        public void UnitTestLittleEndianNonceConversion()
        {
            Assert.AreEqual("5ad18ee2", 
                nonce.IntegerStringToInteger().IntegerToHex().StringSwapAndReverse());
        }

        [TestMethod]
        public void UnitTestVerifyCombinedString()
        {
            Assert.AreEqual(expectedCombinedBlockStringToBeHashed, combinedBlockStringToBeHashed);
        }

        [TestMethod]
        public void UnitTestDoubleHash()
        {
            Assert.AreEqual(expectedBlockHash, 
                combinedBlockStringToBeHashed.ComputeHashBySHA256().ComputeHashBySHA256().StringSwapAndReverse());
        }

        [TestMethod]
        public void UnitTestSingleHashInByteArray()
        {
            //Assert.AreEqual(SingleSha256(combinedBlockStringToBeHashed.StringToByteArray()).ByteArrayToHex(),
            //    combinedBlockStringToBeHashed.ComputeHashBySHA256());
            byte[] arr = Encoding.ASCII.GetBytes(expectedCombinedBlockStringToBeHashed);
            //HashAlgorithm a = HashAlgorithm.Create("SHA-256");
            SHA256Managed a = new SHA256Managed();
            var resulta = a.ComputeHash(arr);

            SHA256 b = SHA256.Create();
            var resultb = b.ComputeHash(arr);
            Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(resulta, resultb));
        }

        [TestMethod]
        public void UnitTestSingleHashInHex()
        {
            //Assert.AreEqual(SingleSha256(combinedBlockStringToBeHashed.StringToByteArray()).ByteArrayToHex(),
            //    combinedBlockStringToBeHashed.ComputeHashBySHA256());
            byte[] arr = Encoding.ASCII.GetBytes(expectedCombinedBlockStringToBeHashed);
            //HashAlgorithm a = HashAlgorithm.Create("SHA-256");
            SHA256Managed a = new SHA256Managed();
            var resulta = a.ComputeHash(arr);
            
            SHA256 b = SHA256.Create();
            var resultb = b.ComputeHash(arr);
            //Assert.IsTrue(StructuralComparisons.StructuralEqualityComparer.Equals(resulta, resultb));
            //Assert.AreEqual(resulta.ByteArrayToHex(), resultb.ByteArrayToHex());
            Assert.AreEqual(combinedBlockStringToBeHashed.ComputeHashBySHA256(), resulta.ByteArrayToHex());
        }

        [TestMethod]
        public void UnitTestVerifyBlockHash()
        {
            var resulta = 
                expectedCombinedBlockStringToBeHashed.StringToByteArray().ComputeDoubleHashBySHA256(); 
            Assert.AreEqual(expectedBlockHash, resulta.ByteArrayToHex().StringSwapAndReverse());
        }
    }
}
