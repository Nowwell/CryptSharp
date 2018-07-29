using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptSharp
{
    public class Matrix
    {
        public Matrix(int row, int column)
        {
            data = new double[row, column];
            Rows = row;
            Columns = column;
        }
        public Matrix(double[,] matr)
        {
            data = new double[matr.GetLength(0), matr.GetLength(1)];
            Rows = matr.GetLength(0);
            Columns = matr.GetLength(1);

            SetData(matr);
        }
        public Matrix(Matrix matr)
        {
            data = new double[matr.Rows, matr.Columns];
            Rows = matr.Rows;
            Columns = matr.Columns;

            SetData(matr);
        }

        double[,] data;
        //double constMultiplier = 1;
        //double constAdder = 0;

        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public double this[int x, int y]
        {
            get
            {
                return data[x, y];
            }
            set
            {
                data[x, y] = value;
            }
        }

        public void SetData(double [,] matr)
        {
            if(data.GetLength(0)!=Rows || data.GetLength(1)!=Columns)
            {
                throw new Exception("Row and Column counts must match to set data");
            }

            for(int i=0; i<Rows; i++)
            {
                for(int j=0; j<Columns; j++)
                {
                    data[i, j] = matr[i, j];
                }
            }
        }
        public void SetData(Matrix matr)
        {
            if (data.GetLength(0) != Rows || data.GetLength(1) != Columns)
            {
                throw new Exception("Row and Column counts must match to set data");
            }

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    data[i, j] = matr[i, j];
                }
            }
        }

        public void MakeIdentity()
        {
            if (Rows != Columns) throw new Exception("Not a square matrix, cannot make identity");

            data = new double[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                data[i, i] = 1.0;
            }
        }


        public void MakeRandom(Random rand)
        {
            data = new double[Rows, Columns];
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    data[r, c] = (rand.NextDouble() < 0.5 ? -1 : 1);
                }
            }
        }
        public double Determinant()
        {
            if (Rows != Columns) throw new Exception("Not a square matrix, cannot take determinant");

            if (Rows == 2 && Columns == 2)
            {
                return data[0, 0] * data[1, 1] - data[1, 0] * data[0, 1];
            }
            else if (Rows == 3 && Columns == 3)
            {
                return data[0, 0] * data[1, 1] * data[2, 2] + data[0, 1] * data[1, 2] * data[2, 0] + data[0, 2] * data[1, 0] * data[2, 1] - data[2, 0] * data[1, 1] * data[0, 2] - data[2, 1] * data[1, 2] * data[0, 0] - data[2, 2] * data[1, 0] * data[0, 1];
            }
            else
            {
                //need to work with original data and it's transpose... so transpose indicies...

                //something isn't quite right with this...
                double det = 0;
                int e = 1;
                for (int i = 0; i < Columns; i++)
                {
                    Matrix cofactor = new Matrix(Rows - 1, Columns - 1);

                    int a = 0;
                    int b = 0;
                    for (int y = 1; y < Rows; y++)
                    {
                        a = 0;
                        for (int x = 0; x < Columns; x++)
                        {
                            if (x == i) continue;

                            cofactor[b, a] = data[y, x];

                            a++;
                        }
                        b++;
                    }
                    det += data[0, i] * e * cofactor.Determinant();
                    e *= -1;
                }

                return det;
            }
        }

        public Matrix Adjoint()
        {
            Matrix m = new Matrix(Rows, Columns);

            Transpose();
            int e = 1;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Matrix cofactor = new Matrix(Rows - 1, Columns - 1);

                    int a = 0;
                    int b = 0;
                    for (int y = 0; y < Rows; y++)
                    {
                        if (y == j) continue;

                        a = 0;
                        for (int x = 0; x < Columns; x++)
                        {
                            if (x == i) continue;

                            cofactor[a, b] = e * data[x, y];

                            a++;
                        }
                        b++;
                    }
                    m[i, j] = e * cofactor.Determinant();
                    e *= -1;
                }
            }
            Transpose();
            return m;
        }

        //public void AddRow2ToRow1(int row1, int row2)
        //{
        //    for (int i = 0; i < Columns; i++)
        //    {
        //        data[row1, i] += data[row2, i];
        //    }
        //}
        //public void MultiplyRowByConstant(int row, double constant)
        //{
        //    for (int i = 0; i < Columns; i++)
        //    {
        //        data[row, i] *= constant;
        //    }
        //}
        //public void DivideRowByConstant(int row, double constant)
        //{
        //    MultiplyRowByConstant(row, 1.0 / constant);
        //}
        //public void SwitchRows(int row1, int row2)
        //{
        //    for (int i = 0; i < Columns; i++)
        //    {
        //        double temp = data[row1, i];
        //        data[row1, i] = data[row2, i];
        //        data[row2, i] = temp;
        //    }

        //}

        public void Transpose()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (i == j) break;
                    double temp = data[i, j];
                    data[i, j] = data[j, i];
                    data[j, i] = temp;
                }
            }
        }
        public Matrix GetTranspose()
        {
            Matrix ret = new Matrix(Rows, Columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    ret[i, j] = data[j, i];
                }
            }
            return ret;
        }

        public void Invert()
        {
            if (Rows != Columns) throw new Exception("Not a square matrix, cannot take determinant");

            if (Rows == 2 && Columns == 2)
            {
                double det = data[0, 0] * data[1, 1] - data[1, 0] * data[0, 1];
                double temp = data[0, 0] * (1.0 / det);
                data[0, 0] = data[1, 1] * (1.0 / det);
                data[1, 1] = temp;

                //temp = -data[0, 1] * (1.0 / det);
                data[0, 1] = -data[0, 1] * (1.0 / det);
                data[1, 0] = -data[1, 0] * (1.0 / det);// temp;
            }
            else
            {
                double inv = (1.0 / Determinant());

                Matrix adj = Adjoint();
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        data[i, j] = inv * adj[i, j];
                    }
                }

            }
        }

        public void ModInvert(int mod)
        {
            if (Rows != Columns) throw new Exception("Not a square matrix, cannot take determinant");

            if (Rows == 2 && Columns == 2)
            {
                double det = data[0, 0] * data[1, 1] - data[1, 0] * data[0, 1];
                det = Utility.ModInverse((int)det, mod);

                double temp = data[0, 0] * det;
                data[0, 0] = (data[1, 1] * det) % mod;
                data[1, 1] = temp % mod;

                //temp = -data[0, 1] * (1.0 / det);
                data[0, 1] = (-data[0, 1] * det) % mod;
                data[1, 0] = (-data[1, 0] * det) % mod;// temp;
            }
            else
            {
                double inv = Utility.ModInverse((int)Determinant(), mod);

                Matrix adj = Adjoint();
                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        data[i, j] = ((inv * adj[i, j]) % mod + mod) % mod;
                    }
                }

            }
        }

        static int GCD(int a, int b)
        {
            int Remainder;

            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }

        //aka are rows/columns mutually orthogonal
        public bool IsABasis()
        {
            if (Rows == 1 && Columns == 1)
            {
                return true;
            }
            else if (Rows == 2 && Columns == 2)
            {
                return (data[0, 0] * data[0, 1] + data[1, 0] * data[1, 1]) == 0;
            }
            else
            {
                for (int r = 0; r < Rows; r++)
                {
                    for (int s = r + 1; s < Rows; s++)
                    {
                        double sum = 0.0;
                        for (int c = 0; c < Columns; c++)
                        {
                            sum += data[r, c] * data[s, c];
                        }
                        if (sum != 0.0)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        public double Trace()
        {
            double sum = 0.0;
            for (int i = 0; i < Rows; i++)
            {
                sum += data[i, i];
            }
            return sum;
        }

        public void Mod(int mod)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    data[i, j] = data[i, j] % mod;
                }
            }
        }

        public static Matrix operator +(Matrix c1, Matrix c2)
        {
            Matrix ret = new Matrix(c1.Rows, c1.Columns);

            for (int i = 0; i < c1.Rows; i++)
            {
                for (int j = 0; j < c1.Columns; j++)
                {
                    ret[i, j] = c1[i, j] + c2[i, j];
                }
            }
            return ret;
        }
        public static Matrix operator -(Matrix c1, Matrix c2)
        {
            Matrix ret = new Matrix(c1.Rows, c1.Columns);

            for (int i = 0; i < c1.Rows; i++)
            {
                for (int j = 0; j < c1.Columns; j++)
                {
                    ret[i, j] = c1[i, j] - c2[i, j];
                }
            }
            return ret;
        }
        public static Matrix operator *(Matrix c1, Matrix c2)
        {
            Matrix ret = new Matrix(c1.Rows, c1.Columns);

            for (int i = 0; i < c1.Rows; i++)
            {
                for (int j = 0; j < c1.Columns; j++)
                {
                    for (int k = 0; k < c1.Columns; k++)
                    {
                        ret[i, j] += c1[i, k] * c2[k, j];
                    }
                }
            }
            return ret;
        }
        public static Matrix operator /(Matrix c1, Matrix c2)
        {
            c2.Invert();
            Matrix ret = c1 * c2;
            return ret;
        }

        public static Matrix operator +(Matrix c1, double c2)
        {
            Matrix ret = new Matrix(c1.Rows, c1.Columns);

            for (int i = 0; i < c1.Rows; i++)
            {
                for (int j = 0; j < c1.Columns; j++)
                {
                    ret[i, j] = c1[i, j] + c2;
                }
            }
            return ret;
        }
        public static Matrix operator -(Matrix c1, double c2)
        {
            Matrix ret = new Matrix(c1.Rows, c1.Columns);

            for (int i = 0; i < c1.Rows; i++)
            {
                for (int j = 0; j < c1.Columns; j++)
                {
                    ret[i, j] = c1[i, j] - c2;
                }
            }
            return ret;
        }
        public static Matrix operator *(Matrix c1, double c2)
        {
            Matrix ret = new Matrix(c1.Rows, c1.Columns);

            for (int i = 0; i < c1.Rows; i++)
            {
                for (int j = 0; j < c1.Columns; j++)
                {
                    ret[i, j] = c1[i, j] * c2;
                }
            }
            return ret;
        }
        public static Matrix operator /(Matrix c1, double c2)
        {
            Matrix ret = new Matrix(c1.Rows, c1.Columns);

            for (int i = 0; i < c1.Rows; i++)
            {
                for (int j = 0; j < c1.Columns; j++)
                {
                    ret[i, j] = c1[i, j] / c2;
                }
            }
            return ret;
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            int maxLength = 0;
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Columns; c++)
                {
                    if (data[r, c].ToString().Length > maxLength)
                    {
                        maxLength = data[r, c].ToString().Length;
                    }
                }

            }
            //string formatString = "";
            //while (maxLength > 0)
            //{
            //    formatString += "0";
            //    maxLength--;
            //}


            output.Append("[");

            for (int r = 0; r < Rows; r++)
            {
                if (r > 0)
                {
                    output.Append("\r\n ");
                }
                output.Append("[");
                for (int c = 0; c < Columns; c++)
                {
                    if (IsValidCombo(r, c))
                    {
                        output.Append(string.Format("{0," + maxLength + "}", data[r, c]));
                    }
                    else
                    {
                        //output.Append(string.Format(" ", data[r, c]));
                        output.Append(string.Format("{0," + maxLength + "}", data[r, c]));
                    }
                    if (c < Columns - 1)
                    {
                        output.Append(",");
                    }
                }
                output.Append("]");

            }
            output.Append("]");

            return output.ToString();
        }

        public static bool IsValidCombo(int i, int j)
        {
            //int[] values = { 3, 5, 9, 15 };
            int[] values = { 255, 85, 51, 153, 15, 165, 195, 105 };

            if (i == j) return false;

            if (values.Contains(i) && values.Contains(j)) return true;

            return false;
        }
    }
}
