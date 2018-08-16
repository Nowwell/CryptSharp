using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Generic
{
    public class GenericClassicalCipher<K/*type of key 1*/, L/*Type of key 2*/, M/*Type of key 3*/, T/*Type of letter, string or char*/> where T: new()
    {

        public delegate T[] Diffuse(T[] block, GenericClassicalCipher<K, L, M, T> cipher);
        public delegate T[] Confuse(T[] block, GenericClassicalCipher<K, L, M, T> cipher);

        public Diffuse DiffuseFunction { get; set; }
        public Confuse ConfuseFunction { get; set; }

        public delegate T[] InverseDiffuse(T[] block, GenericClassicalCipher<K, L, M, T> cipher);
        public delegate T[] InverseConfuse(T[] block, GenericClassicalCipher<K, L, M, T> cipher);

        public InverseDiffuse InverseDiffuseFunction { get; set; }
        public InverseConfuse InverseConfuseFunction { get; set; }

        public K Key { get; set; }
        public L Key2 { get; set; }
        public M Key3 { get; set; }
        public K IV1 { get; set; }
        public L IV2 { get; set; }
        public M IV3 { get; set; }

        public T[] Alphabet { get; set; }

        public T[] Encrypt(T[] text)
        {
            T[] block = new T[text.Length];
            if (DiffuseFunction != null && ConfuseFunction != null)
            {
                block = DiffuseFunction(text, this);
                block = ConfuseFunction(text, this);
            }
            else if (DiffuseFunction != null && ConfuseFunction == null)
            {
                block = DiffuseFunction(text, this);
            }
            else if (DiffuseFunction == null && ConfuseFunction != null)
            {
                block = ConfuseFunction(text, this);
            }

            return block;
        }

        public T[] Decrypt(T[] text)
        {
            T[] block = new T[text.Length];

            if (InverseDiffuseFunction != null && InverseConfuseFunction != null)
            {
                block = InverseConfuseFunction(text, this);
                block = InverseDiffuseFunction(text, this);
            }
            else if (InverseDiffuseFunction != null && InverseConfuseFunction == null)
            {
                block = InverseDiffuseFunction(text, this);
            }
            else if (InverseDiffuseFunction == null && InverseConfuseFunction != null)
            {
                block = InverseConfuseFunction(text, this);
            }

            return block;
        }
    }
}
