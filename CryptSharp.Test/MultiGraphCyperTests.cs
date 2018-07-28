using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CryptSharp.Ciphers.MultiGraph;

namespace CryptSharp.Test
{
    [TestClass]
    public class MultiGraphCyperTests
    {
        string[] cipher;
        string[] clear;
        string[] generated;

        Dictionary<char, List<string>> dictionary = Utility.LoadDictionary();

        [TestMethod]
        public void Multigraph_AffineTest()
        {
            Affine affine = new Affine(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());
            affine.A = 3;
            affine.B = 7;

            for (int i = 0; i < 25; i++)
            {
                generated = affine.GenerateRandomLetters();

                cipher = affine.Encrypt(generated);
                clear = affine.Decrypt(cipher);

                CollectionAssert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Multigraph_AtbashTest()
        {
            Atbash atbash = new Atbash(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());

            for (int i = 0; i < 25; i++)
            {
                generated = atbash.GenerateRandomLetters();

                cipher = atbash.Encrypt(generated);
                clear = atbash.Decrypt(cipher);

                CollectionAssert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Multigraph_BaconianTest()
        {
            Baconian baconian = new Baconian(Utility.EnglishAlphabet().ToStringArray());
            baconian.GenerateGenericSubTable();

            for (int i = 0; i < 25; i++)
            {
                generated = baconian.GenerateRandomLetters();

                cipher = baconian.Encrypt(generated);
                clear = baconian.Decrypt(cipher);

                for (int j = 0; j < generated.Length; j++)
                {
                    if (generated[j] == "J")
                    {
                        generated[j] = "I";
                    }
                    if (generated[j] == "V")
                    {
                        generated[j] = "U";
                    }
                }

                CollectionAssert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Multigraph_PolybiusTest()
        {
            Polybius polybius = new Polybius(Utility.EnglishAlphabet().ToStringArray());
            polybius.RowHeaders = new string[] { "A", "B", "C", "D", "E" };
            polybius.ColumnHeaders = new string[] { "A", "B", "C", "D", "E" };

            for (int i = 0; i < 25; i++)
            {
                List<string> scrambled = new List<string>(polybius.ScrambledAlphabet());
                for (int j = 0; j < scrambled.Count; j++)
                {
                    if (scrambled[j] == "J")
                    {
                        scrambled.RemoveAt(j);
                        break;
                    }
                }
                polybius.Square = scrambled.ToArray();

                //the letter J is not in the Polybius square.  Standard sub is J -> I
                string[] generated = polybius.GenerateRandomLetters();
                for(int j = 0; j< generated.Length; j++)
                {
                    if (generated[j] == "J")
                    {
                        generated[j] = "I";
                    }
                }

                cipher = polybius.Encrypt(generated);
                clear = polybius.Decrypt(cipher);

                CollectionAssert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Multigraph_RailFenceTest()
        {
            RailFence railfence = new RailFence(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());

            byte[] tokenData = new byte[2];
            using (System.Security.Cryptography.RandomNumberGenerator rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenData);

                railfence.Key = (int)(BitConverter.ToUInt16(tokenData, 0) >> 8);

                for (int i = 0; i < 25; i++)
                {
                    generated = railfence.GenerateRandomLetters();

                    cipher = railfence.Encrypt(generated);
                    clear = railfence.Decrypt(cipher);

                    CollectionAssert.AreEqual(generated, clear);
                }
            }
        }

        [TestMethod]
        public void Multigraph_RotationTest()
        {
            Rotation rotation = new Rotation(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());


            byte[] tokenData = new byte[2];
            using (System.Security.Cryptography.RandomNumberGenerator rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenData);

                rotation.Key = (int)(BitConverter.ToUInt16(tokenData, 0) >> 8);


                for (int i = 0; i < 25; i++)
                {
                    generated = rotation.GenerateRandomLetters();

                    cipher = rotation.Encrypt(generated);
                    clear = rotation.Decrypt(cipher);

                    CollectionAssert.AreEqual(generated, clear);
                }
            }
        }

        [TestMethod]
        public void Multigraph_SubstitutionTest()
        {
            Substitution substitution = new Substitution(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());

            for (int i = 0; i < 25; i++)
            {
                substitution.Key = substitution.ScrambledAlphabet();
                generated = substitution.GenerateRandomLetters();

                cipher = substitution.Encrypt(generated);
                clear = substitution.Decrypt(cipher);

                CollectionAssert.AreEqual(generated, clear);
            }
        }
    }
}
