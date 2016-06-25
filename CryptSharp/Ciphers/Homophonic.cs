using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Homophonic : ICipher
    {
        public Homophonic()
        {
            Alphabet = new Dictionary<char, List<char>>();
        }

        public Dictionary<char, List<char>> Alphabet { get; set; }

        public void GenerateGenericAlphabet()
        {
            Alphabet.Add('A', new List<char> { 'D', '9' });
            Alphabet.Add('B', new List<char> { 'X' });
            Alphabet.Add('C', new List<char> { 'S' });
            Alphabet.Add('D', new List<char> { 'F' });
            Alphabet.Add('E', new List<char> { 'Z' });
            Alphabet.Add('F', new List<char> { 'E', '7', '2', '1' });
            Alphabet.Add('G', new List<char> { 'H' });
            Alphabet.Add('H', new List<char> { 'C' });
            Alphabet.Add('I', new List<char> { 'V', '3' });
            Alphabet.Add('J', new List<char> { 'I' });
            Alphabet.Add('K', new List<char> { 'T' });
            Alphabet.Add('L', new List<char> { 'P' });
            Alphabet.Add('M', new List<char> { 'G' });
            Alphabet.Add('N', new List<char> { 'A' });
            Alphabet.Add('O', new List<char> { 'Q' });
            Alphabet.Add('P', new List<char> { 'L' });
            Alphabet.Add('Q', new List<char> { 'K' });
            Alphabet.Add('R', new List<char> { 'J' });
            Alphabet.Add('S', new List<char> { 'R', '4' });
            Alphabet.Add('T', new List<char> { 'U', '6' });
            Alphabet.Add('U', new List<char> { 'O' });
            Alphabet.Add('V', new List<char> { 'W' });
            Alphabet.Add('W', new List<char> { 'M' });
            Alphabet.Add('X', new List<char> { 'Y' });
            Alphabet.Add('Y', new List<char> { 'B' });
            Alphabet.Add('Z', new List<char> { 'N' });
        }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();

            //TODO: Better RNG?
            Random random = new Random((int)DateTime.Now.Ticks);

            foreach(char c in clearText)
            {
                if (Alphabet[c].Count > 1)
                {
                    cipher.Append(Alphabet[c][random.Next(0, Alphabet[c].Count - 1)]);
                }
                else
                {
                    cipher.Append(Alphabet[c][0]);
                }
            }
            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Decrypt(string cipherText)
        {
            StringBuilder clear = new StringBuilder();

            Dictionary<char, char> lookup = new Dictionary<char, char>();

            foreach (char c in Alphabet.Keys)
            {
                for (int j = 0; j < Alphabet[c].Count; j++)
                {
                    lookup.Add(Alphabet[c][j], c); 
                }
            }

            foreach (char c in cipherText)
            {
                clear.Append(lookup[c]);
            }

            return clear.ToString();
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string GenerateRandomString(int length = 0)
        {
            string generated = Utility.Random(1024).ToUpper();
            int i = length;

            StringBuilder toEncrypt = new StringBuilder();
            foreach (char c in generated)
            {
                if (IsInAlphabet(c))
                {
                    toEncrypt.Append(c);
                }
                if (length != 0 && i == 0)
                {
                    break;
                }
                i--;
            }

            return toEncrypt.ToString();
        }

        public bool IsInAlphabet(char c)
        {
            foreach (char ch in Alphabet.Keys)
            {
                if (ch == c) return true;
                //if (Alphabet[ch].Contains(c)) return true;
            }
            return false;
        }
    }
}
