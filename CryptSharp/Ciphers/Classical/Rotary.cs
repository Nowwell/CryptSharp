using CryptSharp.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Rotary : CipherBase<char>, IClassicalCipher
    {
        public Rotary(char[] Alphabet) : base(Alphabet)
        {
            Default();
        }
        //Rotors would be physically positioned as:
        //3 2 1
        public K1 Rotor1;
        public K2 Rotor2;
        public K3 Rotor3;
        //public char[] Reflector;
        //public List<Pair<char, char>> Plugboard;

        public void Default()
        {
            //Rotor3 = new K3() { Substitution = "CDEFGHIJLMNQUVWXZKRYPTOSAB".ToCharArray(), Start = 0, RotatePosition = 17 };
            //Rotor2 = new K2() { Substitution = "ABCDEFGHIJLMNQUVWXZKRYPTOS".ToCharArray(), Start = 0, RotatePosition = 4 };
            //Rotor1 = new K1() { Substitution = "PTOSABCDEFGHIJLMNQUVWXZKRY".ToCharArray(), Start = 0, RotatePosition = 21 };
            //Rotor3 = new K3() { Substitution = "KRYPTOSABCDEFGHIJLMNQUVWXZ".ToCharArray(), Start = 0, RotatePosition = 17 };
            //Rotor2 = new K2() { Substitution = "KRYPTOSABCDEFGHIJLMNQUVWXZ".ToCharArray(), Start = 0, RotatePosition = 4 };
            //Rotor1 = new K1() { Substitution = "KRYPTOSABCDEFGHIJLMNQUVWXZ".ToCharArray(), Start = 0, RotatePosition = 21 };
            Rotor3 = new K3() { Substitution = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(), Start = 0, RotatePosition = 17 };
            Rotor2 = new K2() { Substitution = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(), Start = 0, RotatePosition = 4 };
            Rotor1 = new K1() { Substitution = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(), Start = 0, RotatePosition = 21 };

            //Reflector = "YRUHQSLDPXNGOKMIEBFZCWVJAT".ToCharArray();
        }

        public void Reset()
        {
            Rotor1.Reset();
            Rotor2.Reset();
            Rotor3.Reset();
        }


        public string Decrypt(string cipherText)
        {
            return Encrypt(cipherText);
        }

        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            StringBuilder cipherText = new StringBuilder(clearText.ToUpper().Replace(" ", ""));

            for (int i = 0; i < clearText.Length; i++)
            {
                IncrementPositions();
                char c = clearText[i];

                //Rotor 1
                c = Rotor1.SubstituteForward(c, alphabet);

                //Rotor 2
                c = Rotor2.SubstituteForward(c, alphabet);

                //Rotor 3
                c = Rotor3.SubstituteForward(c, alphabet);

                cipherText[i] = c;
            }
            return cipherText.ToString();
        }

        public void IncrementPositions()
        {
            Rotor1.Rotate();
            if (Rotor1.IsInRotateNextPosition())
            {
                Rotor2.Rotate();

                if (Rotor2.IsInRotateNextPosition())
                {
                    Rotor3.Rotate();
                }
            }
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }

    public class K1 : Rotor
    {
        private int RotationIndex;

        public override void Reset()
        {
            base.Reset();
            RotationIndex = 0;
        }

        public bool IsInRotateNextPosition()
        {
            return true;
        }
    }

    public class K2 : Rotor
    {
        private int RotationIndex = 0;

        public override void Reset()
        {
            base.Reset();
            RotationIndex = 0;
        }

        public override bool IsInRotateNextPosition()
        {
            RotationIndex++;
            return (RotationIndex % 4) == 0;
        }
    }

    public class K3 : Rotor
    {
        private int RotationIndex = 0;

        public override void Reset()
        {
            base.Reset();
            RotationIndex = 0;
        }

        public override bool IsInRotateNextPosition()
        {
            RotationIndex++;
            return (RotationIndex % 4) == 0;
        }
    }



}
