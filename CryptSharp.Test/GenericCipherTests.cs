using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptSharp.Ciphers.Generic;
using System.Text;

namespace CryptSharp.Test
{
    [TestClass]
    public class GenericCipherTests
    {
        [TestMethod]
        public void Generic_GenericBlock()
        {
            GenericBlockCipher gbc = new GenericBlockCipher();
            gbc.DiffuseFunction = diff;
            gbc.ConfuseFunction = conf;

            gbc.InverseDiffuseFunction = diff;
            gbc.InverseConfuseFunction = conf;

            gbc.Key = BitConverter.GetBytes(0x133457799BBCDFF1);
            byte[] clear = BitConverter.GetBytes(0x0123456789ABCDEF);
            gbc.BlockSize = gbc.Key.Length * 8;//this is to get this working without padding

            byte[] cipher = gbc.Encrypt(clear);

            CollectionAssert.AreNotEqual(clear, cipher);

            byte[] plain = gbc.Decrypt(cipher);

            CollectionAssert.AreEqual(clear, plain);
        }

        public byte[] diff(byte[] block, GenericBlockCipher cipher)
        {
            for (int j = 0; j < block.Length; j++)
            {
                block[j] = (byte)(block[j] ^ cipher.Key[j % cipher.Key.Length]);
            }

            return block;
        }

        public byte[] conf(byte[] block, GenericBlockCipher cipher)
        {
            Array.Reverse(block);

            return block;
        }

        [TestMethod]
        public void Generic_GenericClassical()
        {
            GenericClassicalCipher<int, int, int, char> cipher = new GenericClassicalCipher<int, int, int, char>();
            cipher.Alphabet = Utility.EnglishAlphabet();

            cipher.DiffuseFunction = diffuse;
            cipher.InverseDiffuseFunction = diffuse;


            char[] encrypted = cipher.Encrypt("this is a test".ToUpper().ToCharArray());

            string clear = new string(cipher.Decrypt(encrypted));

            Assert.AreEqual("THISISATEST", clear);
        }

        public char[] diffuse(char[] block, GenericClassicalCipher<int, int, int, char> cipher)
        {
            int alphabetLength = cipher.Alphabet.Length - 1;

            StringBuilder encrypted = new StringBuilder();
            foreach (char c in block)
            {
                if (c == ' ') continue;
                encrypted.Append(cipher.Alphabet[alphabetLength - cipher.Alphabet.IndexOf(c)]);
            }

            return encrypted.ToString().ToCharArray();
        }

        [TestMethod]
        public void Generic_GenericStream()
        {
            Assert.Fail();
        }
    }
}
