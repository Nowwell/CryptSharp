using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Generic
{
    public class GenericBlockCipher
    {
        public delegate byte[] Diffuse(byte[] block, GenericBlockCipher cipher);
        public delegate byte[] Confuse(byte[] block, GenericBlockCipher cipher);

        public Diffuse DiffuseFunction { get; set; }
        public Confuse ConfuseFunction { get; set; }

        public delegate byte[] InverseDiffuse(byte[] block, GenericBlockCipher cipher);
        public delegate byte[] InverseConfuse(byte[] block, GenericBlockCipher cipher);

        public InverseDiffuse InverseDiffuseFunction { get; set; }
        public InverseConfuse InverseConfuseFunction { get; set; }

        public byte[] Key { get; set; }
        public byte[] Key2 { get; set; }
        public byte[] IV { get; set; }
        public int BlockSize { get; set; } = 128;

        public byte[] Encrypt(byte[] clearBlock)
        {
            int bytesPerBlock = BlockSize / 8;
            int blocks = clearBlock.Length / bytesPerBlock;
            if (clearBlock.Length > bytesPerBlock && clearBlock.Length % bytesPerBlock != 0) blocks++;
            if (blocks == 0) blocks = 1;

            byte[] cipher = new byte[blocks * bytesPerBlock];
            for (int i = 0; i < blocks; i++)
            {
                byte[] block = new byte[bytesPerBlock];

                for (int j = 0; j < block.Length; j++)
                {
                    if (i * bytesPerBlock + j < clearBlock.Length)
                    {
                        block[j] = clearBlock[i * bytesPerBlock + j];
                    }
                    else
                    {
                        block[j] = 0;
                    }

                }



                if (DiffuseFunction != null && ConfuseFunction != null)
                {
                    block = DiffuseFunction(block, this);
                    block = ConfuseFunction(block, this);
                }
                else if (DiffuseFunction != null && ConfuseFunction == null)
                {
                    block = DiffuseFunction(block, this);
                }
                else if (DiffuseFunction == null && ConfuseFunction != null)
                {
                    block = ConfuseFunction(block, this);
                }




                for (int j = 0; j < block.Length; j++)
                {
                    cipher[i * bytesPerBlock + j] = block[j];
                }

            }

            return cipher;
        }

        public byte[] Decrypt(byte[] encryptedBlock)
        {
            int bytesPerBlock = BlockSize / 8;
            int blocks = encryptedBlock.Length / bytesPerBlock;
            if (encryptedBlock.Length > bytesPerBlock && encryptedBlock.Length % bytesPerBlock != 0) blocks++;
            if (blocks == 0) blocks = 1;

            byte[] clear = new byte[blocks * bytesPerBlock];
            for (int i = 0; i < blocks; i++)
            {
                byte[] block = new byte[bytesPerBlock];

                for (int j = 0; j < block.Length; j++)
                {
                    if (i * bytesPerBlock + j < encryptedBlock.Length)
                    {
                        block[j] = encryptedBlock[i * bytesPerBlock + j];
                    }
                    else
                    {
                        block[j] = 0;
                    }
                }



                if (InverseDiffuseFunction != null && InverseConfuseFunction != null)
                {
                    block = InverseConfuseFunction(block, this);
                    block = InverseDiffuseFunction(block, this);
                }
                else if (InverseDiffuseFunction != null && InverseConfuseFunction == null)
                {
                    block = InverseDiffuseFunction(block, this);
                }
                else if (InverseDiffuseFunction == null && InverseConfuseFunction != null)
                {
                    block = InverseConfuseFunction(block, this);
                }




                for (int j = 0; j < block.Length; j++)
                {
                    clear[i * bytesPerBlock + j] = block[j];
                }

            }

            return clear;
        }
    }
}
