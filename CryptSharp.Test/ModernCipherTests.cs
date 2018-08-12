using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptSharp.Ciphers.Modern;
using System.Security.Cryptography;

namespace CryptSharp.Test
{
    [TestClass]
    public class ModernCipherTests
    {
        [TestMethod]
        public void Modern_DESTest()
        {
            byte[] key = BitConverter.GetBytes(0x133457799BBCDFF1);
            byte[] clear = BitConverter.GetBytes(0x0123456789ABCDEF);

            Ciphers.Modern.DES d = new Ciphers.Modern.DES();
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
        public void Modern_DESTest_Kryptos()
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

            byte[] data = new byte[8];
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

        [TestMethod]
        public void Modern_StreamTest_LFSR()
        {
            Stream str = new Stream(Utility.EnglishAlphabetAsStrings());

            str.Registers = new Components.LinearFeedbackShiftRegister(0xACE1, 16);//key is effectively 0xACE1
            string[] cipher = str.Encrypt("A B C D E F".Split(' '));

            CollectionAssert.AreNotEqual("A B C D E F".Split(' '), cipher);

            str.Registers = new Components.LinearFeedbackShiftRegister(0xACE1, 16);//key is effectively 0xACE1
            string[] clear = str.Decrypt(cipher);

            CollectionAssert.AreEqual("A B C D E F".Split(' '), clear);
        }

        [TestMethod]
        public void Modern_StreamTest_Key()
        {
            Stream str = new Stream(Utility.EnglishAlphabetAsStrings());
            str.Key = new byte[] { 0xC1, 0xF2, 0x03, 0xA4, 0x05, 0x06, 0xB7, 0x08 };

            string[] cipher = str.Encrypt("A B C D E F".Split(' '));

            CollectionAssert.AreNotEqual("A B C D E F".Split(' '), cipher);

            string[] clear = str.Encrypt(cipher);

            CollectionAssert.AreEqual("A B C D E F".Split(' '), clear);
        }

        [TestMethod]
        public void Modern_Trivium()
        {
            byte[] Key = new byte[10];
            byte[] IV = new byte[10];
            for(int i=0; i<10; i++)
            {
                Key[i] = 0xEF;
                IV[i] = 0xEF;
            }
            Trivium tri = new Trivium(Key, IV);

            byte[] cipher = tri.Encrypt(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 });

            CollectionAssert.AreNotEqual(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, cipher);

            tri.ResetState();
            byte[] clear = tri.Decrypt(cipher);

            CollectionAssert.AreEqual(new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06 }, clear);
        }

        [TestMethod]
        public void Modern_AES_128()
        {
            //a.Key = new byte[] { 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c };
            //byte[] key = new byte[] { 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA };
            byte[] key = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            byte[] iv = new byte[] { 0x01, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };

            AES a = new AES();
            a.Key=key;
            a.IV = iv;

            //byte[] test = new byte[16] {0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88, 0x99, 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF};
            //byte[] test = new byte[16] {0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34};
            byte[] test = new byte[48] { 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA, 0xAA,
                                         0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55, 0x55,
                                         0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66, 0x66};

            byte[] cipher = a.Encrypt(test);
            byte[] checkcipher = new byte[test.Length];

            using (Aes check = Aes.Create())
            {
                check.Key = key;
                check.IV = iv;

                ICryptoTransform encryptor = check.CreateEncryptor(check.Key, check.IV);

                encryptor.TransformBlock(test, 0, test.Length, checkcipher, 0);
            }

            CollectionAssert.AreEqual(checkcipher, cipher);

            byte[] clear = a.Decrypt(cipher);

            CollectionAssert.AreEqual(test, clear);
        }

        [TestMethod]
        public void Modern_AES_192()
        {
            byte[] key = new byte[] { 0x8e, 0x73, 0xb0, 0xf7, 0xda, 0x0e, 0x64, 0x52, 0xc8, 0x10, 0xf3, 0x2b,
                                      0x80, 0x90, 0x79, 0xe5, 0x62, 0xf8, 0xea, 0xd2, 0x52, 0x2c, 0x6b, 0x7b};
            byte[] iv = new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, //0x0, 0x0, 0x0, 0x0, 0x0, 0x0 
                                     0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };//, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0};

            AES a = new AES();
            a.Key = key;
            a.IV = iv;
            //byte[] test = new byte[32] { 0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34,
            //                             0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34};

            byte[] test = new byte[48] { 0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34,
                                         0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34,
                                         0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34};


            byte[] cipher = a.Encrypt(test);
            byte[] checkcipher = new byte[test.Length];

            using (Aes check = Aes.Create())
            {
                check.Key = a.Key;
                check.IV = iv;

                ICryptoTransform encryptor = check.CreateEncryptor(check.Key, check.IV);

                encryptor.TransformBlock(test, 0, test.Length, checkcipher, 0);
            }

            CollectionAssert.AreEqual(checkcipher, cipher);

            byte[] clear = a.Decrypt(cipher);

            CollectionAssert.AreEqual(test, clear);
        }

        [TestMethod]
        public void Modern_AES_256()
        {
            byte[] key = new byte[] { 0x60, 0x3d, 0xeb, 0x10, 0x15, 0xca, 0x71, 0xbe, 0x2b, 0x73, 0xae,
                                      0xf0, 0x85, 0x7d, 0x77, 0x81, 0x1f, 0x35, 0x2c, 0x07, 0x3b, 0x61,
                                      0x08, 0xd7, 0x2d, 0x98, 0x10, 0xa3, 0x09, 0x14, 0xdf, 0xf4};
            byte[] iv = new byte[] { 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, //0x0, 0x0, 0x0, 0x0, 0x0, 0x0 
                                     0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0 };//, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0};

            AES a = new AES();
            a.Key = key;
            a.IV = iv;
            byte[] test = new byte[48] { 0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34,
                                         0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34,
                                         0x32, 0x43, 0xf6, 0xa8, 0x88, 0x5a, 0x30, 0x8d, 0x31, 0x31, 0x98, 0xa2, 0xe0, 0x37, 0x07, 0x34};

            byte[] cipher = a.Encrypt(test);
            byte[] checkcipher = new byte[test.Length];

            using (Aes check = Aes.Create())
            {
                check.Key = a.Key;
                check.IV = iv;

                ICryptoTransform encryptor = check.CreateEncryptor(check.Key, check.IV);

                encryptor.TransformBlock(test, 0, test.Length, checkcipher, 0);
            }

            CollectionAssert.AreEqual(checkcipher, cipher);

            byte[] clear = a.Decrypt(cipher);

            CollectionAssert.AreEqual(test, clear);
        }
    }
}
