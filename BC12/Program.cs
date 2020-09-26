using System;

namespace BC12
{
    using BC.Utilities.Extensions;
    using System;
    using System.Linq;
    using System.Security.Cryptography;

        class Program
        {
            static void Main(string[] args)
            {
                //Stage our data from a block explorer. Copied directly from Block #503266.
                string nVersion = "0x20000000";
                string HashPrevBlock = "00000000000000000033972c9263e9f1b593a4a555157ecc97735bf2fdea7664";
                string HashMerkleRoot = "ad873aebe594e6fa58c828b131d397c4d804355007499f8c9ad8e1a9e000b21a";
                string nTime = "2018-01-09 01:40:31";
                string nBits = "402690497";
                string nNonce = "3801010522";

                //Format the data.
                nVersion = Reverse(Swap(ToHex(Convert.ToUInt32(nVersion, 16)))); //Read in the version as an integer. Convert the integer back to hex. Swap every two characters in the string. Then, Reverse the string.
                HashPrevBlock = Reverse(Swap(HashPrevBlock));  //Swap every two characters in the HashPrevBlock  string. Then, Reverse the string. See the also the Swap() and Reverse() methods.
                HashMerkleRoot = Reverse(Swap(HashMerkleRoot)); //Swap every two characters in the HashMerkleRoot string. Then, Reverse the string. See the also the Swap() and Reverse() methods.
                nTime = Reverse(Swap(ToHex(DateToEpoch(nTime))));  //Convert the timestamp to a valid integer using Unix Epoch. Convert the integer value to hex. Swap every two characters in the value. Reverse the string.
                nBits = Reverse(Swap(ToHex(UInt32.Parse(nBits)))); //Convert the bits  to an integer (required because we are inputting it as a string). Convert the integer to hex. Swap every two characters in the value. Reverse the string.
                nNonce = Reverse(Swap(ToHex(UInt32.Parse(nNonce))));//Convert the nonce to an integer (required because we are inputting it as a string). Convert the integer to hex. Swap every two characters in the value. Reverse the string.

                //Send our pre-hashed data to the console for viewing. This should match the data that we converted earlier in the guide.
                Console.WriteLine("Block: #503266");
                Console.WriteLine("nVersion:       " + nVersion);
                Console.WriteLine("HashPrevBlock:  " + HashPrevBlock);
                Console.WriteLine("HashMerkleRoot: " + HashMerkleRoot);
                Console.WriteLine("nTime:          " + nTime);
                Console.WriteLine("nBits:          " + nBits);
                Console.WriteLine("nNonce:         " + nNonce);
                Console.WriteLine("-------------------------------------------------------------------------");

                //Calculate the PoW
                string blockInfo = nVersion + HashPrevBlock + HashMerkleRoot + nTime + nBits + nNonce; //Concatenate our formatted data into the exact Bitcoin PoW order.
                byte[] blockDataToHash = ToBytes(blockInfo);            //Convert our formatted data into bytes, the final step before hashing.
                Console.WriteLine($"blockInfo Hash: {blockInfo.ComputeHashBySHA256()}");
                byte[] blockPoWHash = DoubleSha256(blockDataToHash); //Perform a double SHA256 on the byte array (the PoW).
            //Console.WriteLine($"DoubleSha Hash: {blockPoWHash.ComputeHashBySHA256()}");
            string successfulHash = ToString(blockPoWHash);        //Convert our PoW hash back to a readable string.

                //Display our results
                Console.WriteLine("Block PoW Hash: " + Reverse(Swap(successfulHash)));  //Reverse the PoW hash so that the 0s are at the start, and display to the user.
                Console.ReadKey();
            }

            public static string Reverse(string input)
            {
                return new string(input.ToCharArray().Reverse().ToArray()); //Convert the input string to a char array -> reverse the values in this array -> convert and return back the string.
            }

            public static string Swap(string input)
            {
                string swappedString = "";
                for (int i = 0; i < input.Length; i += 2)
                {
                    //Loop through the entire string, selecting and reversing every 2 characters. The conditional operator (? :) is to handle cases where the string is of uneven length. In which case, we just a select the last and single character.
                    swappedString += new string((input.Substring(i, input.Length >= i + 2 ? 2 : 1)).ToArray().Reverse().ToArray());
                }
                return swappedString;
            }

            public static byte[] ToBytes(string input)
            {
                byte[] bytes = new byte[(input.Length + 1) / 2]; //Create a Byte array that is half the length of the string input. The length+1 portion is to assure that the array size always rounds up, and has enough available space (note: 1 byte can hold 2 characters).
                for (int i = 0, j = 0; i < input.Length; j++, i += 2)
                    bytes[j] = byte.Parse(input.Substring(i, input.Length >= i + 2 ? 2 : 1), System.Globalization.NumberStyles.HexNumber);
                //Loop through the entire string, selecting and converting every 2 characters to bytes. The conditional operator (? :) is to handle cases where the string is of uneven length. In which case, we just a select the last and single character and convert it to bytes.
                return bytes;
            }

            public static string ToString(byte[] input)
            {
                string result = "";
                foreach (byte b in input)
                    result += b.ToString("x2"); //Convert every byte (2 characters) in the array back to a string value.

                return result;
            }

            public static string ToHex(UInt32 input)
            {
                return input.ToString("X8").ToLower(); //Convert the integer value to hex, making sure that it is all lowercase. X8 also ensures that the result is padded to at least 8 characters.
            }

            public static byte[] DoubleSha256(byte[] input)
            {
                SHA256Managed _hasher = new SHA256Managed(); //The built in .NET library for performing SHA256 hashing.
                byte[] crypto = _hasher.ComputeHash(_hasher.ComputeHash(input)); //Run two rounds of SHA256 hashing over our byte array.
                return crypto; //Return the SHA256 hash.
            }

            public static UInt32 DateToEpoch(string input)
            {
                return (UInt32)(Convert.ToDateTime(input) - new DateTime(1970, 1, 1)).TotalSeconds; //Calculate the number of seconds that have elapsed since the given date and the Unix Epoch. Return the integer value.
            }
        }
    }
