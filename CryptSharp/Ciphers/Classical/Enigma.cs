using CryptSharp.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers.Classical
{
    public class Enigma : CipherBase<char>, IClassicalCipher
    {
        public Enigma(char[] Alphabet) : base(Alphabet)
        {
            Default();
        }

        //Rotors would be physically positioned as:
        //Reflector <-> 5 4 3 2 1 <-> Plugboard
        public Rotor Rotor1;
        public Rotor Rotor2;
        public Rotor Rotor3;
        public Rotor Rotor4;
        public char[] Reflector;
        public List<Pair<char, char>> Plugboard;

        public void Default()
        {
            //https://en.wikipedia.org/wiki/Enigma_rotor_details
            Rotor4 = null;
            Rotor3 = new Rotor() { Substitution = "EKMFLGDQVZNTOWYHXUSPAIBRCJ".ToCharArray(), Start = 0, RotatePosition = 17 };
            Rotor2 = new Rotor() { Substitution = "AJDKSIRUXBLHWTMCQGZNPYFVOE".ToCharArray(), Start = 0, RotatePosition = 4 };
            Rotor1 = new Rotor() { Substitution = "BDFHJLCPRTXVZNYEIWGAKMUSQO".ToCharArray(), Start = 0, RotatePosition = 21 };

            Reflector = "YRUHQSLDPXNGOKMIEBFZCWVJAT".ToCharArray();
        }

        public void Reset()
        {
            Rotor1.Reset();
            Rotor2.Reset();
            Rotor3.Reset();
            if (Rotor4 != null) Rotor4.Reset();
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

                //Plugboard Substitution
                if (Plugboard != null)
                {
                    foreach (Pair<char, char> p in Plugboard)
                    {
                        if (p.Value1 == c)
                        {
                            c = p.Value2;
                            break;
                        }
                    }
                }

                //Rotor 1
                c = Rotor1.SubstituteForward(c, alphabet);


                //Rotor 2
                c = Rotor2.SubstituteForward(c, alphabet);

                //Rotor 3
                c = Rotor3.SubstituteForward(c, alphabet);

                //Rotor 4
                if (Rotor4 != null)
                {
                    c = Rotor4.SubstituteForward(c, alphabet);
                }

                //Reflector
                c = Reflector[alphabet.IndexOf(c)];


                //Rotor 4
                if (Rotor4 != null)
                {
                    c = Rotor4.SubstituteBackward(c, alphabet);
                }

                //Rotor 3
                c = Rotor3.SubstituteBackward(c, alphabet);

                //Rotor 2
                c = Rotor2.SubstituteBackward(c, alphabet);


                //Rotor 1
                c = Rotor1.SubstituteBackward(c, alphabet);

                //Plugboard Substitution
                if (Plugboard != null)
                {
                    foreach (Pair<char, char> p in Plugboard)
                    {
                        if (p.Value2 == c)
                        {
                            c = p.Value1;
                            break;
                        }
                    }
                }

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
}
