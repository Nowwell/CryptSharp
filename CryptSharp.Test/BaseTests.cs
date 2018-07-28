using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptSharp.Ciphers;

namespace CryptSharp.Test
{
    /// <summary>
    /// Summary description for BaseTests
    /// </summary>
    [TestClass]
    public class BaseTests
    {
        [TestMethod]
        public void GenerateRandomStringTest()
        {
            Affine affine = new Affine(Utility.KeyedEnglishAlphabet("KRYPTOS"));
            affine.A = 3;
            affine.B = 7;

            Random r = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < 25; i++)
            {
                int len = r.Next(0, 100);
                string x = affine.GenerateRandomString(len);
                Assert.AreEqual(len, x.Length);
            }
        }

        [TestMethod]
        public void ScrambledAlphabetTest()
        {
            char[] alphabet = Utility.KeyedEnglishAlphabet("KRYPTOS");
            Affine affine = new Affine(alphabet);
            affine.A = 3;
            affine.B = 7;

            //scramble the alphabet
            char[] scrambled = affine.ScrambledAlphabet().ToCharArray();
            Dictionary<char, int> counts = new Dictionary<char, int>();

            //ensure the scrambled alphabet contains one and only one of each letter
            foreach (char c in scrambled)
            {
                if (affine.IsInAlphabet(c))
                {
                    if (counts.ContainsKey(c))
                    {
                        counts[c]++;
                    }
                    else
                    {
                        counts.Add(c, 1);
                    }
                }
            }

            //check the counts of each letter to ensure they're unique
            int sum = 0;
            foreach(char key in counts.Keys)
            {
                int value = counts[key];

                if(value > 1)
                {
                    Assert.Fail(string.Format("Scramble failed, {0} included {1} times", key, value));
                }
                sum += value;
            }

            Assert.AreEqual(alphabet.Length, sum);

        }
    }
}
