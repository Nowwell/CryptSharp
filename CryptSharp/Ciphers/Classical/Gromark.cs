using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Gromark : IClassicalCipher
    {
        public Gromark()
        {
        }

        public int[] Primer { get; set; }
        private int[] primer { get; set; }
        public char[] StraightAlphabet { get; set; }
        public char[] KeyedAlphabet { get; set; }
        private int primerIndex = 0;
        private bool hasPassedPrimerInit = false;

        public string Decrypt(string cipherText)
        {
            primerIndex = 0;
            hasPassedPrimerInit = false;
            primer = new int[Primer.Length];
            Array.Copy(Primer, primer, Primer.Length);

            StringBuilder clearText = new StringBuilder();
            foreach (char s in cipherText)
            {
                int lookupChar = (KeyedAlphabet.IndexOf(s) - GetPrimer() + StraightAlphabet.Length) % StraightAlphabet.Length;
                clearText.Append(StraightAlphabet[lookupChar]);
            }
            return clearText.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            primerIndex = 0;
            hasPassedPrimerInit = false;
            primer = new int[Primer.Length];
            Array.Copy(Primer, primer, Primer.Length);

            StringBuilder cipherText = new StringBuilder();
            foreach (char s in clearText)
            {
                int lookupChar = (StraightAlphabet.IndexOf(s) + GetPrimer()) % StraightAlphabet.Length;
                cipherText.Append(KeyedAlphabet[lookupChar]);
            }
            return cipherText.ToString();
        }

        private int GetPrimer()
        {
            if(primerIndex == primer.Length) hasPassedPrimerInit = true;

            primerIndex = primerIndex % primer.Length;

            if (hasPassedPrimerInit)
            {
                primer[primerIndex] = (primer[primerIndex] + primer[(primerIndex + 1) % primer.Length]) % 10;
            }

            return primer[primerIndex++];
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
