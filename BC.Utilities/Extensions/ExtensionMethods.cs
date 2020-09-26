using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

namespace BC.Utilities.Extensions
{
    public static class ExtensionMethods
    {
        //private byte[] ComputeHash(string s)
        public static string ComputeHashBySHA256(this string stringToBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(stringToBeHashed)).ByteArrayToHex();
            }
        }

        public static byte[] ComputeHashBySHA256(this byte[] byteArrayToBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(byteArrayToBeHashed);
            }
        }

        public static byte[] ComputeDoubleHashBySHA256(this byte[] byteArrayToBeHashed)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(sha256.ComputeHash(byteArrayToBeHashed));
            }
        }

        public static string ComputeHMACHashBySHA256(this string stringToBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToBeHashed)));
            }
        }

        public static string ByteArrayToHex(this byte[] bytes) =>
           BitConverter.ToString(bytes).Replace("-", "").ToLower();

        public static int DateTimeStringToEpoch(this string s) =>
            (int)(Convert.ToDateTime(s) - new DateTime(1970, 1, 1)).TotalSeconds;

        public static int HexStringToInteger(this string stringTobeConverted) =>
            (int)Convert.ToInt32(stringTobeConverted, 16);

        public static int IntegerStringToInteger(this string inputString) =>
            (int)UInt32.Parse(inputString);

        public static string IntegerToHex(this int intToBeConverted)
            => intToBeConverted.ToString("X8").ToLower();

        //Create a Byte array that is half the length of the string input. 
        //The length+1 portion is to assure that the array size always rounds up, 
        //and has enough available space (note: 1 byte can hold 2 characters).
        public static byte[] StringToByteArray(this string input)
        {
            var bytes = new byte[(input.Length + 1) / 2];
            for (int i = 0, j = 0; i < input.Length; j++, i += 2)
                bytes[j] = byte.Parse(input.Substring(i, input.Length >= i + 2 ? 2 : 1), NumberStyles.HexNumber);
            //Loop through the entire string, selecting and converting every 2 characters to bytes. 
            //The conditional operator (? :) is to handle cases where the string is of uneven length. 
            //In which case, we just a select the last and single character and convert it to bytes.
            return bytes;
        }

        //Convert the input string to a char array -> reverse the values in this array -> convert and return back the string.
        public static string StringReverse(this string inputString) =>
            new string(inputString.ToCharArray().Reverse().ToArray()); 

        public static string StringSwap(this string inputString) =>
            new string(inputString?.PadToEvenLength()?.SwapOddEvenChars()?.ToArray());

        public static string StringSwapAndReverse(this string inputString) =>
            inputString.StringSwap().StringReverse();

        static string PadToEvenLength(this string text) =>
            text.Length.IsOdd() ? text + ' ' : text;

        static IEnumerable<char> SwapOddEvenChars(this string text) =>
            text.Select((c, i) => i.IsOdd() ? text[i - 1] : text[i + 1]);

        static bool IsOdd(this int number) => number % 2 == 1;

    }
}
