using CryptSharp.Ciphers.Classical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Lorentz : IClassicalCipher
    {
        public Lorentz()
        {
            Alphabet = new Dictionary<char, int>();

            Alphabet.Add('A', 0b11000);
            Alphabet.Add('B', 0b10011);
            Alphabet.Add('C', 0b01110);
            Alphabet.Add('D', 0b10010);
            Alphabet.Add('E', 0b10000);
            Alphabet.Add('F', 0b10110);
            Alphabet.Add('G', 0b01011);
            Alphabet.Add('H', 0b00101);
            Alphabet.Add('I', 0b01100);
            Alphabet.Add('J', 0b11010);
            Alphabet.Add('K', 0b11110);
            Alphabet.Add('L', 0b01001);
            Alphabet.Add('M', 0b00111);
            Alphabet.Add('N', 0b00110);
            Alphabet.Add('O', 0b00011);
            Alphabet.Add('P', 0b01101);
            Alphabet.Add('Q', 0b11101);
            Alphabet.Add('R', 0b01010);
            Alphabet.Add('S', 0b10100);
            Alphabet.Add('T', 0b00001);
            Alphabet.Add('U', 0b11100);
            Alphabet.Add('V', 0b01111);
            Alphabet.Add('W', 0b11001);
            Alphabet.Add('X', 0b10111);
            Alphabet.Add('Y', 0b10101);
            Alphabet.Add('Z', 0b10001);

            Alphabet.Add('3', 0b00010);
            Alphabet.Add('4', 0b01000);
            Alphabet.Add('8', 0b11111);
            Alphabet.Add('9', 0b00100);
            Alphabet.Add('+', 0b11011);
            Alphabet.Add('/', 0b00000);
        }

        private Dictionary<char, int> Alphabet;
        private int[] KWheel = new int[] { 0b11000, 0b10011, 0b01110, 0b10010, 0b10000, 0b10110, 0b01011, 0b00101, 0b01100, 0b11010, 0b11110, 0b01001, 0b00111, 0b00110 };
        private int[] SWheel = new int[] { 0b11000, 0b11000, 0b10011, 0b10011 };

        public int K;
        public int S;

        public string Decrypt(string cipherText)
        {
            StringBuilder clearText = new StringBuilder(cipherText);

            int k = K;
            int s = S;

            for (int i = 0; i < clearText.Length; i++)
            {
                int keyValue = KWheel[k] ^ SWheel[s];
                int cipherValue = Alphabet[cipherText[i]];

                clearText[i] = Alphabet.FirstOrDefault(x => x.Value == (keyValue ^ cipherValue)).Key;

                k++;
                k %= KWheel.Length;
                s++;
                s %= SWheel.Length;
            }

            return clearText.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            StringBuilder cipherText = new StringBuilder(clearText);

            int k = K;
            int s = S;

            for (int i = 0; i < cipherText.Length; i++)
            {
                int keyValue = KWheel[k] ^ SWheel[s];
                int clearValue = Alphabet[cipherText[i]];

                cipherText[i] = Alphabet.FirstOrDefault(x => x.Value == (keyValue ^ clearValue)).Key;

                k++;
                k %= KWheel.Length;
                s++;
                s %= SWheel.Length;
            }

            return cipherText.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
