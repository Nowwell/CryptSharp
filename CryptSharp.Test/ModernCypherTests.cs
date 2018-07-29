using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CryptSharp.Ciphers;

namespace CryptSharp.Test
{
    [TestClass]
    public class ModernCypherTests
    {

        [TestMethod]
        public void Modern_DESTest()
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

    }
}
