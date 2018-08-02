using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.MultiGraph
{
    public class Baconian : CipherBase<string>, IMultigraphCipher
    {
        protected Dictionary<string, int> charIndexPositions = new Dictionary<string, int>();
        public Baconian(string[] Alphabet) : base(Alphabet)
        {
            alphabet = Alphabet;

            for (int i = 0; i < alphabet.Length; i++)
            {
                charIndexPositions.Add(alphabet[i], i);
            }

            SubstitutionTable = new Dictionary<string, string>();
        }

        public Dictionary<string, string> SubstitutionTable { get; set; }

        public void GenerateGenericSubTable()
        {
            SubstitutionTable.Add(alphabet[0], "aaaaa");
            SubstitutionTable.Add(alphabet[1], "aaaab");
            SubstitutionTable.Add(alphabet[2], "aaaba");
            SubstitutionTable.Add(alphabet[3], "aaabb");
            SubstitutionTable.Add(alphabet[4], "aabaa");
            SubstitutionTable.Add(alphabet[5], "aabab");
            SubstitutionTable.Add(alphabet[6], "aabba");
            SubstitutionTable.Add(alphabet[7], "aabbb");
            SubstitutionTable.Add(alphabet[8], "abaaa");//I
            SubstitutionTable.Add(alphabet[9], "abaaa");//J
            SubstitutionTable.Add(alphabet[10], "abaab");
            SubstitutionTable.Add(alphabet[11], "ababa");
            SubstitutionTable.Add(alphabet[12], "ababb");
            SubstitutionTable.Add(alphabet[13], "abbaa");
            SubstitutionTable.Add(alphabet[14], "abbab");
            SubstitutionTable.Add(alphabet[15], "abbba");
            SubstitutionTable.Add(alphabet[16], "abbbb");
            SubstitutionTable.Add(alphabet[17], "baaaa");
            SubstitutionTable.Add(alphabet[18], "baaab");
            SubstitutionTable.Add(alphabet[19], "baaba");
            SubstitutionTable.Add(alphabet[20], "baabb");//U
            SubstitutionTable.Add(alphabet[21], "baabb");//V
            SubstitutionTable.Add(alphabet[22], "babaa");
            SubstitutionTable.Add(alphabet[23], "babab");
            SubstitutionTable.Add(alphabet[24], "babba");
            SubstitutionTable.Add(alphabet[25], "babbb");
        }

        public string[] Encrypt(string[] clearText)
        {
            List<string> cipher = new List<string>();

            foreach (string s in clearText)
            {
                cipher.Add(SubstitutionTable[s]);
            }
            return cipher.ToArray();
        }
        public string[] Encrypt(string clearText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public string[] Decrypt(string[] cipherText)
        {
            List<string> output = new List<string>();
            for (int i = 0; i < cipherText.Length; i ++)
            {
                string code = cipherText[i];

                foreach (string c in alphabet)
                {
                    if (SubstitutionTable[c] == code)
                    {
                        output.Add(c);
                        break;
                    }
                }
            }
            return output.ToArray();
        }
        public string[] Decrypt(string cipherText, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename, char wordSeparator, char charSeparator)
        {
            throw new NotImplementedException();
        }
    }
}
