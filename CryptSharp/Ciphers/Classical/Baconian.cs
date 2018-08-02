using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Baconian : CipherBase<char>, IClassicalCipher
    {
        public Baconian(char[] Alphabet) : base(Alphabet)
        {
            SubstitutionTable = new Dictionary<char, string>();
        }

        public Dictionary<char, string> SubstitutionTable { get; set; }

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


        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();

            foreach (char s in clearText)
            {
                cipher.Append(SubstitutionTable[s]);
            }
            return cipher.ToString();
        }

        public string Decrypt(string cipherText)
        {
            int len = SubstitutionTable[alphabet[0]].Length;

            StringBuilder output = new StringBuilder();
            for (int i = 0; i < cipherText.Length; i += len)
            {
                string code = cipherText.Substring(i, len);

                foreach (char c in alphabet)
                {
                    if (SubstitutionTable[c] == code)
                    {
                        output.Append(c);
                        break;
                    }
                }
            }
            return output.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
