﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Generic
{
    class GenericStreamCipher
    {
        public delegate byte[] GenerateKeyStream(uint numBytes, GenericStreamCipher cipher);

        public GenerateKeyStream KeystreamFunction { get; set; }

        public byte[] state = new byte[36];
        public byte[] IV { get; private set; }
        public byte[] Key { get; private set; }

        public byte[] Encrypt(byte[] clearText)
        {
            byte[] streamKey = KeystreamFunction((uint)(8 * clearText.Length), this);

            byte[] cipher = new byte[clearText.Length];
            for (int i = 0; i < clearText.Length; i++)
            {
                cipher[i] = (byte)(clearText[i] ^ streamKey[i]);
            }

            return cipher;
        }

        public byte[] Decrypt(byte[] cipherText)
        {
            byte[] streamKey = KeystreamFunction((uint)(8 * cipherText.Length), this);

            byte[] clear = new byte[cipherText.Length];
            for (int i = 0; i < cipherText.Length; i++)
            {
                clear[i] = (byte)(cipherText[i] ^ streamKey[i]);
            }

            return clear;
        }

    }
}
