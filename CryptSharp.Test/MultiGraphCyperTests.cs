using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using CryptSharp.Ciphers.MultiGraph;

namespace CryptSharp.Test
{
    [TestClass]
    public class MultiGraphCyperTests
    {
        string cipher = "";
        string clear = "";
        string generated = "";

        Dictionary<char, List<string>> dictionary = Utility.LoadDictionary();

        [TestMethod]
        public void Multigraph_AffineTest()
        {
            Affine affine = new Affine(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());
            affine.A = 3;
            affine.B = 7;

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = affine.GenerateRandomString();

                cipher = affine.Encrypt(new string[] { generated });
                clear = affine.Decrypt(new string[] { cipher });

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Multigraph_AtbashTest()
        {
            Atbash atbash = new Atbash(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = atbash.GenerateRandomString();

                cipher = atbash.Encrypt(new string[] { generated });
                clear = atbash.Decrypt(new string[] { cipher });

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Multigraph_BaconianTest()
        {
            Baconian baconian = new Baconian(Utility.EnglishAlphabet().ToStringArray());
            baconian.GenerateGenericSubTable();

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = baconian.GenerateRandomString();

                cipher = baconian.Encrypt(new string[] { generated });
                clear = baconian.Decrypt(new string[] { cipher });

                Assert.AreEqual(generated.Replace("J", "I").Replace("V", "U"), clear);
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
                generated = polybius.GenerateRandomString().Replace("J", "I");

                cipher = polybius.Encrypt(new string[] { generated });
                clear = polybius.Decrypt(new string[] { cipher });

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Multigraph_RailFenceTest()
        {
            RailFence railfence = new RailFence(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());

            cipher = "";
            clear = "";
            generated = "";

            byte[] tokenData = new byte[2];
            using (System.Security.Cryptography.RandomNumberGenerator rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenData);

                railfence.Key = (int)(BitConverter.ToUInt16(tokenData, 0) >> 8);

                for (int i = 0; i < 25; i++)
                {
                    generated = railfence.GenerateRandomString();

                    cipher = railfence.Encrypt(new string[] { generated });
                    clear = railfence.Decrypt(new string[] { cipher });

                    Assert.AreEqual(generated, clear);
                }
            }
        }

        [TestMethod]
        public void Multigraph_RotationTest()
        {
            Rotation rotation = new Rotation(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());

            cipher = "";
            clear = "";
            generated = "";

            byte[] tokenData = new byte[2];
            using (System.Security.Cryptography.RandomNumberGenerator rng = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                rng.GetBytes(tokenData);

                rotation.Key = (int)(BitConverter.ToUInt16(tokenData, 0) >> 8);


                for (int i = 0; i < 25; i++)
                {
                    generated = rotation.GenerateRandomString();

                    cipher = rotation.Encrypt(new string[] { generated });
                    clear = rotation.Decrypt(new string[] { cipher });

                    Assert.AreEqual(generated, clear);
                }
            }
        }

        [TestMethod]
        public void Multigraph_SubstitutionTest()
        {
            Substitution substitution = new Substitution(Utility.KeyedEnglishAlphabet("KRYPTOS").ToStringArray());

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                substitution.Key = substitution.ScrambledAlphabet();
                generated = substitution.GenerateRandomString();

                cipher = substitution.Encrypt(new string[] { generated });
                clear = substitution.Decrypt(new string[] { cipher });

                Assert.AreEqual(generated, clear);
            }
        }
    }
}
