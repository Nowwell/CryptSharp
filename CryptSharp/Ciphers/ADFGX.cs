using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class ADFGX : CipherBase<char>, ICipher
    {
        public ADFGX(char[] Alphabet) : base(Alphabet)
        {
            Key = alphabet[0].ToString();
        }
        public ADFGX(char[] Alphabet, string key, char[] square) : base(Alphabet)
        {
            Key = key;
            Square = square;
        }

        public string Key { get; set; }
        public char[] Square { get; set; }

        public string Decrypt(string cipherText)
        {
            Polybius poly = new Polybius(alphabet);
            poly.RowHeaders = new char[] { 'A', 'D', 'F', 'G', 'X' };
            poly.ColumnHeaders = new char[] { 'A', 'D', 'F', 'G', 'X' };
            poly.Square = Square;

            Columnar column = new Columnar(alphabet);
            column.Key = Key;

            return poly.Decrypt(column.Decrypt(cipherText));
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            Polybius poly = new Polybius(alphabet);
            poly.RowHeaders = new char[] { 'A', 'D', 'F', 'G', 'X' };
            poly.ColumnHeaders = new char[] { 'A', 'D', 'F', 'G', 'X' };
            poly.Square = Square;

            Columnar column = new Columnar(alphabet);
            column.Key = Key;

            return column.Encrypt(poly.Encrypt(clearText));
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
