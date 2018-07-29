using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptSharp.Test
{
    /// <summary>
    /// Summary description for UtilityTests
    /// </summary>
    [TestClass]
    public class UtilityTests
    {
        [TestMethod]
        public void Utility_ArabicNumerals()
        {
            char[] arabic = Utility.ArabicNumerals();

            char[] numerals = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            CollectionAssert.AreEqual(numerals, arabic);
        }

        [TestMethod]
        public void Utility_KeyedEnglishAlphabet()
        {
            char[] keyed = Utility.KeyedEnglishAlphabet("KRYPTOS");

            char[] kryptos = "KRYPTOSABCDEFGHIJLMNQUVWXZ".ToCharArray();

            CollectionAssert.AreEqual(kryptos, keyed);

            keyed = Utility.KeyedEnglishAlphabet("KRYPTOS", true);

            kryptos = "KRYPTOSABCDEFGHIJLMNQUVWXZ".ToLower().ToCharArray();

            CollectionAssert.AreEqual(kryptos, keyed);
        }

        [TestMethod]
        public void Utility_EnglishAlphabet()
        {
            char[] keyed = Utility.EnglishAlphabet();

            char[] kryptos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            CollectionAssert.AreEqual(kryptos, keyed);

            keyed = Utility.EnglishAlphabet(true);

            kryptos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();

            CollectionAssert.AreEqual(kryptos, keyed);
        }

        [TestMethod]
        public void Utility_Cicada3301Alphabet()
        {
            string[] keyed = Utility.Cicada3301Alphabet();

            string[] kryptos = "F U TH O R C G W H N I J EO P X S T B E M L NG OE D A AE Y IA EA".Split(' ');

            CollectionAssert.AreEqual(kryptos, keyed);
        }

        [TestMethod]
        public void Utility_GermanAlphabet()
        {
            char[] keyed = Utility.GermanAlphabet();

            char[] kryptos = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜẞ".ToCharArray();

            CollectionAssert.AreEqual(kryptos, keyed);

            keyed = Utility.GermanAlphabet(true);

            kryptos = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜẞ".ToLower().ToCharArray();

            CollectionAssert.AreEqual(kryptos, keyed);
        }

        [TestMethod]
        public void Utility_EnglishAlphabetAsStrings()
        {
            string[] keyed = Utility.EnglishAlphabetAsStrings();

            string[] kryptos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray().ToStringArray();

            CollectionAssert.AreEqual(kryptos, keyed);

            keyed = Utility.EnglishAlphabetAsStrings(true);

            kryptos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray().ToStringArray();

            CollectionAssert.AreEqual(kryptos, keyed);
        }

        [TestMethod]
        public void Utility_GermanAlphabetAsStrings()
        {
            string[] keyed = Utility.GermanAlphabetAsStrings();

            string[] kryptos = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜẞ".ToCharArray().ToStringArray();

            CollectionAssert.AreEqual(kryptos, keyed);

            keyed = Utility.GermanAlphabetAsStrings(true);

            kryptos = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜẞ".ToLower().ToCharArray().ToStringArray();

            CollectionAssert.AreEqual(kryptos, keyed);
        }

        [TestMethod]
        public void Utility_StringToStringArray()
        {
            CryptSharp.Ciphers.Affine affine = new CryptSharp.Ciphers.Affine(Utility.KeyedEnglishAlphabet("KRYPTOS"));
            affine.A = 3;
            affine.B = 7;

            string generated = affine.GenerateRandomString();

            string[] str = Utility.StringToStringArray(generated);

            Assert.IsTrue(str.Length > 0);
        }

        [TestMethod]
        public void Utility_Random()
        {
            string number = Utility.Random(1);
            byte[] num = Convert.FromBase64String(number);
            Assert.IsTrue(Convert.FromBase64String(number).Length == 1);

            number = Utility.Random(3);
            num = Convert.FromBase64String(number);
            Assert.IsTrue(Convert.FromBase64String(number).Length == 3);

            number = Utility.Random(654);
            num = Convert.FromBase64String(number);
            Assert.IsTrue(Convert.FromBase64String(number).Length == 654);
        }

        [TestMethod]
        public void Utility_RandomGeneric()
        {
            char[] number = Utility.Random<char>(1);
            byte[] num = Convert.FromBase64String(new string(number));
            Assert.IsTrue(num.Length == 1);

            number = Utility.Random<char>(453);
            num = Convert.FromBase64String(new string(number));
            Assert.IsTrue(num.Length == 453);

            number = Utility.Random<char>(23456);
            num = Convert.FromBase64String(new string(number));
            Assert.IsTrue(num.Length == 23456);
        }

        [TestMethod]
        public void Utility_RandomInt()
        {
            int x = Utility.RandomInt();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void Utility_LoadDictionary()
        {
            Dictionary<char, List<string>> stuff = Utility.LoadDictionary();

            Assert.IsTrue(stuff.Keys.Count > 0);
        }
    }
}
