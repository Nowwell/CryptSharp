using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using CryptSharp;
using CryptSharp.Ciphers;
using System.Text;
using System.Collections.Generic;

namespace CryptSharp.Test
{
    [TestClass]
    public class UnigraphCyperTests
    {
        string cipher = "";
        string clear = "";
        string generated = "";

        Dictionary<char, List<string>> dictionary = Utility.LoadDictionary();

        [TestMethod]
        public void Unigraph_AffineTest()
        {
            Affine affine = new Affine(Utility.KeyedEnglishAlphabet("KRYPTOS"));
            affine.A = 3;
            affine.B = 7;

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = affine.GenerateRandomString();

                cipher = affine.Encrypt(generated);
                clear = affine.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_AtbashTest()
        {
            Atbash atbash = new Atbash(Utility.KeyedEnglishAlphabet("KRYPTOS"));

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = atbash.GenerateRandomString();

                cipher = atbash.Encrypt(generated);
                clear = atbash.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_BaconianTest()
        {
            Baconian baconian = new Baconian(Utility.EnglishAlphabet());
            baconian.GenerateGenericSubTable();

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = baconian.GenerateRandomString();

                cipher = baconian.Encrypt(generated);
                clear = baconian.Decrypt(cipher);

                Assert.AreEqual(generated.Replace("J", "I").Replace("V", "U"), clear);
            }
        }

        [TestMethod]
        public void Unigraph_BeaufortTest()
        {
            Beaufort beaufort = new Beaufort(Utility.KeyedEnglishAlphabet("KRYPTOS"));

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                beaufort.Key = beaufort.GenerateRandomString(11);
                generated = beaufort.GenerateRandomString();

                cipher = beaufort.Encrypt(generated);
                clear = beaufort.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }
        
        [TestMethod]
        public void Unigraph_BeaufortAutokeyTest()
        {
            Beaufort beaufort = new Beaufort(Utility.KeyedEnglishAlphabet("KRYPTOS"));
            beaufort.AutoKey = true;

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                beaufort.Key = beaufort.GenerateRandomString(11);
                generated = beaufort.GenerateRandomString();

                cipher = beaufort.Encrypt(generated);
                clear = beaufort.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_VigenereTest()
        {
            Vigenere vigenere = new Vigenere(Utility.KeyedEnglishAlphabet("KRYPTOS"));

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                vigenere.Key = vigenere.GenerateRandomString(11);

                generated = vigenere.GenerateRandomString();

                cipher = vigenere.Encrypt(generated);
                clear = vigenere.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_VigenereAutoKeyTest()
        {
            Vigenere vigenere = new Vigenere(Utility.KeyedEnglishAlphabet("KRYPTOS"));
            vigenere.Key = "ABSCISSA";
            vigenere.AutoKey = true;

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                vigenere.Key = vigenere.GenerateRandomString(11);

                generated = vigenere.GenerateRandomString();

                cipher = vigenere.Encrypt(generated);
                clear = vigenere.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_ColumnarTest()
        {
            Columnar columnar = new Columnar(Utility.EnglishAlphabet());

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                System.Collections.Generic.Dictionary<char, int> repeats = new System.Collections.Generic.Dictionary<char, int>();
                foreach (char c in columnar.GenerateRandomString(7))
                {
                    if (!repeats.ContainsKey(c))
                    {
                        repeats.Add(c, 0);
                    }
                }

                StringBuilder sb = new StringBuilder();
                foreach (char c in repeats.Keys)
                {
                    sb.Append(c);
                }

                columnar.Key = sb.ToString();


                generated = columnar.GenerateRandomString();

                cipher = columnar.Encrypt(generated);
                clear = columnar.Decrypt(cipher);

                Assert.AreEqual(generated, clear);//error is missing the last character...
            }
        }

        [TestMethod]
        public void Unigraph_PolybiusTest()
        {
            Polybius polybius = new Polybius(Utility.EnglishAlphabet());
            polybius.RowHeaders = new char[] { 'A', 'B', 'C', 'D', 'E' };
            polybius.ColumnHeaders = new char[] { 'A', 'B', 'C', 'D', 'E' };

            for (int i = 0; i < 25; i++)
            {
                List<char> scrambled = new List<char>(polybius.ScrambledAlphabet());
                for (int j = 0; j < scrambled.Count; j++)
                {
                    if (scrambled[j] == 'J')
                    {
                        scrambled.RemoveAt(j);
                        break;
                    }
                }
                polybius.Square = scrambled.ToArray();
                
                //the letter J is not in the Polybius square.  Standard sub is J -> I
                generated = polybius.GenerateRandomString().Replace("J", "I");

                cipher = polybius.Encrypt(generated);
                clear = polybius.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_RailFenceTest()
        {
            RailFence railfence = new RailFence(Utility.KeyedEnglishAlphabet("KRYPTOS"));

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

                    cipher = railfence.Encrypt(generated);
                    clear = railfence.Decrypt(cipher);

                    Assert.AreEqual(generated, clear);
                }
            }
        }

        [TestMethod]
        public void Unigraph_RotationTest()
        {
            Rotation rotation = new Rotation(Utility.KeyedEnglishAlphabet("KRYPTOS"));
            
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

                    cipher = rotation.Encrypt(generated);
                    clear = rotation.Decrypt(cipher);

                    Assert.AreEqual(generated, clear);
                }
            }
        }

        [TestMethod]
        public void Unigraph_SubstitutionTest()
        {
            Substitution substitution = new Substitution(Utility.KeyedEnglishAlphabet("KRYPTOS"));

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                substitution.Key = substitution.ScrambledAlphabet();
                generated = substitution.GenerateRandomString();

                cipher = substitution.Encrypt(generated);
                clear = substitution.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_PortaTest()
        {
            Porta porta = new Porta(Utility.KeyedEnglishAlphabet("KRYPTOS"));
            porta.Key = "KRYPTOS";

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {

                generated = porta.GenerateRandomString();

                cipher = porta.Encrypt(generated);
                clear = porta.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_HomophonicTest()
        {
            Homophonic homophonic = new Homophonic();
            homophonic.GenerateGenericAlphabet();

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = homophonic.GenerateRandomString();

                cipher = homophonic.Encrypt(generated);
                clear = homophonic.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_AmscoTest()
        {
            Amsco amsco = new Amsco(Utility.KeyedEnglishAlphabet("KRYPTOS"));
            amsco.Key = new int[5] { 3, 0, 2, 1, 4 };// { 2, 4, 0, 1, 3 };

            cipher = "";
            clear = "";
            generated = "";
            for (int j = 1; j < 50; j++)
            {
                for (int i = 0; i < 25; i++)
                {
                    generated = amsco.GenerateRandomString().Substring(0, j);

                    cipher = amsco.Encrypt(generated);
                    clear = amsco.Decrypt(cipher);

                    Assert.AreEqual(generated, clear);
                }
            }
        }

        [TestMethod]
        public void Unigraph_XorTest()
        {
            Xor xor = new Xor(Utility.EnglishAlphabet());// KeyedEnglishAlphabet("KRYPTOS"));

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                xor.Key = xor.GenerateRandomString(11);
                generated = xor.GenerateRandomString();

                cipher = xor.Encrypt(generated);
                clear = xor.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_FourSquareTest()
        {
            char[] ab = Utility.EnglishAlphabet();
            Array.Resize(ref ab, ab.Length - 1);
            FourSquare foursquare = new FourSquare(ab);// KeyedEnglishAlphabet("KRYPTOS"));

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                foursquare.KeySquare1 = new string(foursquare.ScrambledAlphabet());
                foursquare.KeySquare2 = new string(foursquare.ScrambledAlphabet());

                generated = foursquare.GenerateRandomString();

                cipher = foursquare.Encrypt(generated);
                clear = foursquare.Decrypt(cipher);

                if (cipher.Length - 1 == generated.Length)
                {
                    clear = clear.Substring(0, clear.Length - 1);
                }

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_HillTest()
        {
            Hill hill = new Hill(Utility.EnglishAlphabet());// Utility.KeyedEnglishAlphabet("KRYPTOS"));
            hill.Key = new Matrix(new double[,] { { 6, 24, 1 }, { 13, 16, 10 }, { 20, 17, 15 } });
            //hill.Key = new double[,] { { 2, 4, 5 }, { 9, 2, 1 }, { 3, 17, 7 } };
            //hill.Key = new double[,] { { 5, 17 }, { 4, 15 } };

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = hill.GenerateRandomString(552);

                while (generated.Length % hill.Key.Rows != 0)
                {
                    generated += 'Z';
                }

                //generated = "ABCDEFGHI";

                cipher = hill.Encrypt(generated);
                clear = hill.Decrypt(cipher);

                int x = generated.Length;
                int y = cipher.Length;
                int z = clear.Length;

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_PlayfairTest()
        {
            char[] ch = new string(Utility.KeyedEnglishAlphabet("KRYPTOS")).Replace("J","").ToCharArray();
            Playfair playfair = new Playfair(ch);

            cipher = "";
            clear = "";
            generated = "";
            for (int i = 0; i < 25; i++)
            {
                generated = playfair.GenerateRandomString().Replace("J", "I").Substring(0, 26);

                cipher = playfair.Encrypt(generated);
                clear = playfair.Decrypt(cipher);

                //TODO: Write changing X to be the letter that came before it
                //Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_ADFGVXTest()
        {
            ADFGVX adfgvx = new ADFGVX(Utility.EnglishAlphabet());
            adfgvx.Square = "phqg0iu7me4ay5lno8jfd9xk6rc2vs4tz1wb3".ToUpper().ToCharArray();
            adfgvx.Key = "GERMAN";

            for (int i = 0; i < 25; i++)
            {
                generated = adfgvx.GenerateRandomString();

                cipher = adfgvx.Encrypt(generated);
                clear = adfgvx.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_ADFGXTest()
        {
            char[] ch = new string(Utility.KeyedEnglishAlphabet("KRYPTOS")).Replace("J", "").ToCharArray();
            ADFGX adfgx = new ADFGX(ch);
            adfgx.Square = "phqgiumeaylnofdxkrcvstzwb".ToUpper().ToCharArray();
            adfgx.Key = "GERMAN";

            for (int i = 0; i < 25; i++)
            {
                generated = adfgx.GenerateRandomString().Replace("J", "I");

                cipher = adfgx.Encrypt(generated);
                clear = adfgx.Decrypt(cipher);

                Assert.AreEqual(generated, clear);
            }
        }

        [TestMethod]
        public void Unigraph_BifidTest()
        {
            char[] ch = new string(Utility.EnglishAlphabet()).Replace("J", "").ToCharArray();
            Bifid bifid = new Bifid(ch);

            for (bifid.Group = 1; bifid.Group < 25; bifid.Group++)
            {

                for (int i = 0; i < 25; i++)
                {
                    bifid.Square = bifid.ScrambledAlphabet();
                    generated = bifid.GenerateRandomString().Replace("J", "I");

                    cipher = bifid.Encrypt(generated);
                    clear = bifid.Decrypt(cipher);

                    Assert.AreEqual(generated, clear);
                }
            }
        }

        [TestMethod]
        public void Unigraph_TrifidTest()
        {
            char[] ch = (new string(Utility.EnglishAlphabet()) + '.').ToCharArray();
            Trifid trifid = new Trifid(ch);

            for (trifid.Group = 1; trifid.Group < 25; trifid.Group++)
            {
                for (int i = 0; i < 25; i++)
                {
                    trifid.Squares = trifid.ScrambledAlphabet();
                    generated = trifid.GenerateRandomString();

                    cipher = trifid.Encrypt(generated);
                    clear = trifid.Decrypt(cipher);

                    Assert.AreEqual(generated, clear);
                }
            }
        }
        
        [TestMethod]
        public void Advanced_TestDES()
        {
            byte[] key = BitConverter.GetBytes(0x133457799BBCDFF1);
            byte[] clear = BitConverter.GetBytes(0x0123456789ABCDEF);

            DES d = new DES();
            d.Mode = Mode.ElectronicCodeBook;
            d.Key = key;
            d.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            byte[] cipherText = d.Encrypt(clear);

            ulong output = BitConverter.ToUInt64(cipherText, 0);

            Assert.AreEqual(0x85E813540F0AB405, output);

            output = BitConverter.ToUInt64(d.Decrypt(cipherText), 0);
            Assert.AreEqual((ulong)0x0123456789ABCDEF, output);

            d.Mode = Mode.ChainBlockCoding;
            d.Key = key;
            d.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
            cipherText = d.Encrypt(clear);

            output = BitConverter.ToUInt64(cipherText, 0);

            Assert.AreEqual(0x85E813540F0AB405, output);

            d.Mode = Mode.ChainBlockCoding;
            d.Key = key;
            d.IV = clear;
            cipherText = d.Encrypt(clear);

            output = BitConverter.ToUInt64(cipherText, 0);

            Assert.AreEqual(0x948A43F98A834F7E, output);

            output = BitConverter.ToUInt64(d.Decrypt(cipherText), 0);
            Assert.AreEqual((ulong)0x0123456789ABCDEF, output);
        }

        [TestMethod]
        public void TestDES_Kryptos()
        {
            byte[] key = new byte[8];

            ulong baseKey = (ulong)'S' | (ulong)'O' << 8 | (ulong)'T' << 16 | (ulong)'P' << 24 | (ulong)'Y' << 32 | (ulong)'R' << 40 | (ulong)'K' << 48;
            //ulong baseKey = (ulong)'K' | (ulong)'R' << 8 | (ulong)'Y' << 16 | (ulong)'P' << 24 | (ulong)'T' << 32 | (ulong)'O' << 40 | (ulong)'S' << 48;
            ulong expandedKey = baseKey & 0x7F;
            expandedKey |= ((baseKey >> 7) & 0x7F) << 8;
            expandedKey |= ((baseKey >> 14) & 0x7F) << 16;
            expandedKey |= ((baseKey >> 21) & 0x7F) << 24;
            expandedKey |= ((baseKey >> 28) & 0x7F) << 32;
            expandedKey |= ((baseKey >> 35) & 0x7F) << 40;
            expandedKey |= ((baseKey >> 42) & 0x7F) << 48;
            expandedKey |= ((baseKey >> 49) & 0x7F) << 56;

            System.Security.Cryptography.DES des = System.Security.Cryptography.DES.Create();

            des.Key = BitConverter.GetBytes(expandedKey);
            des.Mode = System.Security.Cryptography.CipherMode.ECB;

            System.Security.Cryptography.ICryptoTransform xform = des.CreateDecryptor();

            byte[] data =new byte[8];
            data[0] = (byte)'O';
            data[1] = (byte)'K';
            data[2] = (byte)'R';
            data[3] = (byte)'U';
            data[4] = (byte)'O';
            data[5] = (byte)'X';
            data[6] = (byte)'O';

            byte[] output = new byte[8];
            xform.TransformBlock(data, 0, 8, output, 0);

            for (int i = 0; i < 8; i++)
            {
                output[i] = (byte)(output[i] % 26);
            }


        }
    }
}
