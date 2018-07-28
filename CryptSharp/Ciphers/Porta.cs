using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Porta : CipherBase<char>, ICipher
    {
        public Porta(char[] Alphabet) : base(Alphabet)
        {
            Key = alphabet[0].ToString();
        }
        public Porta(char[] Alphabet, string key, bool autoKey = false) : base(Alphabet)
        {
            Key = key;
        }

        public string Key { get; set; }

        public string Encrypt(string clearText)
        {
            StringBuilder front = new StringBuilder(new string(alphabet).Substring(0, alphabet.Length / 2));
            StringBuilder back = new StringBuilder(new string(alphabet).Substring(alphabet.Length / 2, alphabet.Length / 2));

            string[] table = new string[13];

            //build decryption table
            for (int i = 0; i < 13; i++)
            {
                table[i] = back.ToString() + front.ToString();

                char last = front[front.Length - 1];
                front.Remove(front.Length - 1, 1);
                front.Insert(0, last);

                last = back[0];
                back.Remove(0, 1);
                back.Append(last);
            }
            int l = 0;

            //decrypt
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < clearText.Length; i++)
            {
                int j = i % Key.Length;

                //get index appropriate to the alphabet
                for (int k = 0; k < alphabet.Length; k++)
                {
                    if (clearText[i] == alphabet[k])
                    {
                        l = k;
                        break;
                    }
                }

                if ((Key[j] == alphabet[24]) || (Key[j] == alphabet[25])) { output.Append(table[12][l]); }
                if ((Key[j] == alphabet[22]) || (Key[j] == alphabet[23])) { output.Append(table[11][l]); }
                if ((Key[j] == alphabet[20]) || (Key[j] == alphabet[21])) { output.Append(table[10][l]); }
                if ((Key[j] == alphabet[18]) || (Key[j] == alphabet[19])) { output.Append(table[9][l]); }
                if ((Key[j] == alphabet[16]) || (Key[j] == alphabet[17])) { output.Append(table[8][l]); }
                if ((Key[j] == alphabet[14]) || (Key[j] == alphabet[15])) { output.Append(table[7][l]); }
                if ((Key[j] == alphabet[12]) || (Key[j] == alphabet[13])) { output.Append(table[6][l]); }
                if ((Key[j] == alphabet[10]) || (Key[j] == alphabet[11])) { output.Append(table[5][l]); }
                if ((Key[j] == alphabet[8]) || (Key[j] == alphabet[9])) { output.Append(table[4][l]); }
                if ((Key[j] == alphabet[6]) || (Key[j] == alphabet[7])) { output.Append(table[3][l]); }
                if ((Key[j] == alphabet[4]) || (Key[j] == alphabet[5])) { output.Append(table[2][l]); }
                if ((Key[j] == alphabet[2]) || (Key[j] == alphabet[3])) { output.Append(table[1][l]); }
                if ((Key[j] == alphabet[0]) || (Key[j] == alphabet[1])) { output.Append(table[0][l]); }

            }

            return output.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText)
        {
            StringBuilder front = new StringBuilder(new string(alphabet).Substring(0, alphabet.Length / 2));
            StringBuilder back = new StringBuilder(new string(alphabet).Substring(alphabet.Length / 2, alphabet.Length / 2));

            string[] table = new string[13];

            //build decryption table
            for (int i = 0; i < 13; i++)
            {
                table[i] = back.ToString() + front.ToString();

                char last = front[front.Length - 1];
                front.Remove(front.Length - 1, 1);
                front.Insert(0, last);

                last = back[0];
                back.Remove(0, 1);
                back.Append(last);
            }
            int l = 0;

            //decrypt
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < cipherText.Length; i++)
            {
                int j = i % Key.Length;

                //get index appropriate to the alphabet
                for (int k = 0; k < alphabet.Length; k++)
                {
                    if (cipherText[i] == alphabet[k])
                    {
                        l = k;
                        break;
                    }
                }

                if ((Key[j] == alphabet[24]) || (Key[j] == alphabet[25])) { output.Append(table[12][l]); }
                if ((Key[j] == alphabet[22]) || (Key[j] == alphabet[23])) { output.Append(table[11][l]); }
                if ((Key[j] == alphabet[20]) || (Key[j] == alphabet[21])) { output.Append(table[10][l]); }
                if ((Key[j] == alphabet[18]) || (Key[j] == alphabet[19])) { output.Append(table[9][l]); }
                if ((Key[j] == alphabet[16]) || (Key[j] == alphabet[17])) { output.Append(table[8][l]); }
                if ((Key[j] == alphabet[14]) || (Key[j] == alphabet[15])) { output.Append(table[7][l]); }
                if ((Key[j] == alphabet[12]) || (Key[j] == alphabet[13])) { output.Append(table[6][l]); }
                if ((Key[j] == alphabet[10]) || (Key[j] == alphabet[11])) { output.Append(table[5][l]); }
                if ((Key[j] == alphabet[8]) || (Key[j] == alphabet[9])) { output.Append(table[4][l]); }
                if ((Key[j] == alphabet[6]) || (Key[j] == alphabet[7])) { output.Append(table[3][l]); }
                if ((Key[j] == alphabet[4]) || (Key[j] == alphabet[5])) { output.Append(table[2][l]); }
                if ((Key[j] == alphabet[2]) || (Key[j] == alphabet[3])) { output.Append(table[1][l]); }
                if ((Key[j] == alphabet[0]) || (Key[j] == alphabet[1])) { output.Append(table[0][l]); }

            }

            return output.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
