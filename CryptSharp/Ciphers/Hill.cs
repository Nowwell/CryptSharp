using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp.Ciphers
{
    public class Hill : CipherBase, ICipher
    {
        public Hill(char[] Alphabet) : base(Alphabet)
        {
            Key = new double[,] { { 2, 4, 5 }, { 9, 2, 1 }, { 3, 17, 7 } };
        }
        public Hill(char[] Alphabet, double[,] key) : base(Alphabet)
        {
            Key = key;
        }

        public double[,] Key { get; set; }

        public string Decrypt(string cipherText)
        {
            double[,] inv = InvertKey();

            StringBuilder clear = new StringBuilder();

            for (int i = 0; i < cipherText.Length; i += inv.GetLength(1))
            {
                for (int k = 0; k < inv.GetLength(0); k++)
                {
                    double outchar = 0;
                    for (int j = 0; j < inv.GetLength(1); j++)
                    {
                        outchar += (double)alphabet.IndexOf(cipherText[i + j]) * inv[k, j];
                    }
                    clear.Append(alphabet[(((int)outchar % alphabet.Length) + alphabet.Length) % alphabet.Length]);
                }
            }

            while (clear[clear.Length - 1] == alphabet[alphabet.Length - 1])
            {
                clear.Length--;
            }

            return clear.ToString();

        }
        private double[,] InvertKey()
        {
            int n = Key.Length;
            double[,] result = null;
            InvertMatrix(Key, out result);
            double d = det((double[,])Key.Clone());
            int modinv = Utility.ModInverse((int)d, alphabet.Length);
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = Math.Round((((result[i, j] * d) % alphabet.Length) * modinv) % alphabet.Length, 0);
                    while (result[i, j] < 0)
                    {
                        result[i, j] += alphabet.Length;
                    }
                }
            }

            return result;
        }
        static double det(double[,] a)
        {
            int i, j, k;
            int n = a.GetLength(0);
            double det = 0;
            for (i = 0; i < n - 1; i++)
            {
                for (j = i + 1; j < n; j++)
                {
                    det = a[j, i] / a[i, i];
                    for (k = i; k < n; k++)
                        a[j, k] = a[j, k] - det * a[i, k];
                }
            }
            det = 1;
            for (i = 0; i < n; i++)
                det = det * a[i, i];
            return det;
        }
        // The double matrix inversion routine:
        static Boolean InvertMatrix(double[,] A, out double[,] invA)
        {
            // There are faster ways to do this, but for simplicity
            // and to get something working quickly, I'll just write a 
            // simple Gaussian-elimination matrix inverter here, and look 
            // at speeding things up later.

            // Keep a record to see if there is a sensible inverse:
            Boolean bNotIllConditioned = true;

            // If the matrix is not square, there is no inverse:
            if (A.GetLength(0) != A.GetLength(1))
            {
                invA = A; // Have to assign something to invA before returning
                return false;
            }

            // This routine destroys the matrix it's working on, so I'll first 
            // make a copy of it, as well as setting up the output matrix invA 
            // as a unit matrix of the appropriate size:
            int dimension = A.GetLength(0);
            double[,] working = new double[dimension, dimension];
            Array.Copy(A, working, A.Length);
            double[,] inverse = new double[dimension, dimension];
            // C# will set the initial values to zero, so to create a unit
            // matrix, I just need to fill in the diagonal elements:
            for (int loop = 0; loop < dimension; loop++) inverse[loop, loop] = 1.0;

            // OK, first convert working to upper triangular form:
            for (int loop = 0; loop < dimension; loop++) // for each row
            {
                int currentRow = loop;

                // First step is pivoting: make sure the biggest element
                // remaining in any column is on the next row.  First, find
                // the biggest element remaining in the current column:
                double biggestSoFar = 0.0; int biggestRow = currentRow;
                for (int x = currentRow; x < dimension; x++)
                {
                    double sizeOfThis = working[x, currentRow];//.Magnitude;
                    if (sizeOfThis > biggestSoFar)
                    {
                        biggestSoFar = sizeOfThis;
                        biggestRow = x;
                    }
                }

                // and if this is not at the top, swop the rows of working
                // and inverse around until it is:
                if (biggestRow != currentRow)
                {
                    double temp;
                    for (int lop = currentRow; lop < dimension; lop++)
                    {
                        temp = working[currentRow, lop];
                        working[currentRow, lop] = working[biggestRow, lop];
                        working[biggestRow, lop] = temp;
                    }
                    for (int lop = 0; lop < dimension; lop++)
                    {
                        temp = inverse[currentRow, lop];
                        inverse[currentRow, lop] = inverse[biggestRow, lop];
                        inverse[biggestRow, lop] = temp;
                    }
                }

                // Then, go down the matrix subtracting as necessary
                // to get rid of the lower-triangular elements:
                for (int lop = currentRow + 1; lop < dimension; lop++)
                {
                    // Matrix might be ill-conditioned.  I should check:
                    if (working[currentRow, currentRow] == 0)
                    {
                        bNotIllConditioned = false;
                        working[currentRow, currentRow] = 1e-60;
                    }
                    double factor = working[lop, currentRow] / working[currentRow, currentRow];

                    // If the matrix is fairly sparse (quite common for this
                    // application), it might make sense to check that the 
                    // lower elements are not already zero before doing all
                    // the scaling and replacing:
                    if (factor != 0.0)
                    {
                        // Only have to do from current row on in working, but due
                        // to pivoting, might have to do the entire row in inverse:
                        for (int lp = currentRow; lp < dimension; lp++)
                            working[lop, lp] -= factor * working[currentRow, lp];
                        for (int lp = 0; lp < dimension; lp++)
                            inverse[lop, lp] -= factor * inverse[currentRow, lp];
                    }
                }
                // That's it for this row, now on to the next one...
            }

            // Now with the working matrix in upper-triangular form, continue the same
            // process amongst the upper-triangular elements to convert working into
            // diagonal form:
            for (int loop = dimension - 1; loop >= 0; loop--) // for each row
            {
                int currentRow = loop;

                // Matrix might be ill-conditioned.  I should check:
                if (working[currentRow, currentRow] == 0)
                {
                    bNotIllConditioned = false;
                    working[currentRow, currentRow] = 1e-60;
                }

                // Then, go up the matrix subtracting as necessary to get 
                // rid of the remaining upper-triangular elements:
                for (int lop = currentRow - 1; lop >= 0; lop--)
                {
                    double factor = working[lop, currentRow] / working[currentRow, currentRow];

                    // There's only one element in working to change (the other elements
                    // in the row of working are all zero), and that will always be set
                    // to zero; but you might have to do the entire row in inverse:
                    working[lop, currentRow] = 0.0;

                    if (factor != 0.0)
                    {
                        for (int lp = 0; lp < dimension; lp++)
                        {
                            inverse[lop, lp] -= factor * inverse[currentRow, lp];
                        }
                    }
                }
                // That's it for this row, now on to the next one...
            }

            // Should now have working as a diagonal matrix.  Final thing is 
            // to scale all the rows:
            for (int loop = 0; loop < dimension; loop++)
            {
                double scale = working[loop, loop];
                for (int lop = 0; lop < dimension; lop++) inverse[loop, lop] /= scale;
            }

            // That's it.  inverse should now be the inverse of the original matrix.
            invA = inverse;
            return bNotIllConditioned;
        }
        public void DecryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string clearText)
        {
            StringBuilder cipher = new StringBuilder();

            while (clearText.Length % Key.GetLength(0) != 0)
            {
                clearText += alphabet[alphabet.Length - 1];
            }

            for (int i = 0; i < clearText.Length; i += Key.GetLength(1))
            {
                for (int k = 0; k < Key.GetLength(0); k++)
                {
                    double outchar = 0;
                    for (int j = 0; j < Key.GetLength(1); j++)
                    {
                        outchar += (double)alphabet.IndexOf(clearText[i + j]) * Key[k, j];
                    }
                    cipher.Append(alphabet[(int)outchar % alphabet.Length]);
                } 
            }

            return cipher.ToString();
        }

        public void EncryptFile(string clearTextFilename, string cipherTextFilename)
        {
            throw new NotImplementedException();
        }
    }
}
