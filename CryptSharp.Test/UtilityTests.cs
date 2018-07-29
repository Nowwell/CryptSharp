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

    }
}
