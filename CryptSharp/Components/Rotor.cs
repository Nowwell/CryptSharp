using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Components
{
    public class Rotor
    {
        public char[] Substitution;
        public int Start;
        public int RotatePosition;
        public int RotorPosition;

        public virtual void Reset()
        {
            RotorPosition = Start;
        }

        public virtual char SubstituteForward(char c, char[] alphabet)
        {
            int pos = (alphabet.IndexOf(c) + RotorPosition) % alphabet.Length;
            pos = (alphabet.IndexOf(Substitution[pos]) - RotorPosition + alphabet.Length) % alphabet.Length;
            return alphabet[pos];
        }

        public virtual char SubstituteBackward(char c, char[] alphabet)
        {
            int pos = (alphabet.IndexOf(c) + RotorPosition) % alphabet.Length;
            pos = (Substitution.IndexOf(alphabet[pos]) - RotorPosition + alphabet.Length) % alphabet.Length;
            return alphabet[pos];
        }

        public virtual void Rotate()
        {
            RotorPosition = (RotorPosition + 1) % Substitution.Length;
        }

        public virtual bool IsInRotateNextPosition()
        {
            return RotatePosition == RotorPosition;
        }
    }
}
