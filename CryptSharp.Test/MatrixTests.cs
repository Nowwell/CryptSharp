using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CryptSharp.Test
{
    [TestClass]
    public class MatrixTests
    {
        [TestMethod]
        public void Matrix_MakeIdentityTest()
        {
            Matrix m = new Matrix(5, 5);
            m.MakeIdentity();

            for(int i=0; i<m.Rows; i++)
            {
                for (int j = 0; j < m.Columns; j++)
                {
                    if(i == j)
                    {
                        Assert.AreEqual(1, m[i, j]);
                    }
                    else
                    {
                        Assert.AreEqual(0, m[i, j]);
                    }
                }
            }
        }

        [TestMethod]
        public void Matrix_DeterminantTest()
        {
            Matrix m = new Matrix(5, 5);
            m.MakeIdentity();

            Assert.AreEqual(1, m.Determinant());

            m = new Matrix(new double[,] { { 3, 8 }, { 4, 6 } });
            Assert.AreEqual(-14, m.Determinant());

            m = new Matrix(new double[,] { { 6, 1, 1 }, { 4, -2, 5}, { 2, 8, 7} });
            Assert.AreEqual(-306, m.Determinant());

            m = new Matrix(new double[,] { { 3, 2, 0, 1 }, { 4, 0, 1, 2 }, { 3, 0, 2, 1 }, { 9, 2, 3, 1 } });
            Assert.AreEqual(24, m.Determinant());

        }

        [TestMethod]
        public void Matrix_InversionTest()
        {
            Matrix m = new Matrix(new double[,] { { 1, 2 }, { 3, 4 } });
            m.Invert();

            Assert.AreEqual(-2, m[0, 0]);
            Assert.AreEqual(1, m[0, 1]);
            Assert.AreEqual(3.0/2.0, m[1, 0]);
            Assert.AreEqual(-1.0/2.0, m[1, 1]);

            m = new Matrix(new double[,] { { 1, 2, 3 }, { 0, 4, 5 }, { 1, 0, 6 } });
            m.Invert();

            Assert.AreEqual(12.0 / 11.0, m[0, 0], 0.0000001);
            Assert.AreEqual(-6.0 / 11.0, m[0, 1], 0.0000001);
            Assert.AreEqual(-1.0 / 11.0, m[0, 2], 0.0000001);
            Assert.AreEqual(5.0 / 22.0, m[1, 0], 0.0000001);
            Assert.AreEqual(3.0 / 22.0, m[1, 1], 0.0000001);
            Assert.AreEqual(-5.0 / 22.0, m[1, 2], 0.0000001);
            Assert.AreEqual(-2.0 / 11.0, m[2, 0], 0.0000001);
            Assert.AreEqual(1.0 / 11.0, m[2, 1], 0.0000001);
            Assert.AreEqual(2.0 / 11.0, m[2, 2], 0.0000001);
        }

        [TestMethod]
        public void Matrix_ModInversionTest()
        {
            Matrix m = new Matrix(new double[,] { { 6, 24, 1 }, { 13, 16, 10 }, { 20, 17, 15 } });
            m.ModInvert(26);

            Assert.AreEqual(8, m[0, 0], 0.0000001);
            Assert.AreEqual(5, m[0, 1], 0.0000001);
            Assert.AreEqual(10, m[0, 2], 0.0000001);
            Assert.AreEqual(21, m[1, 0], 0.0000001);
            Assert.AreEqual(8, m[1, 1], 0.0000001);
            Assert.AreEqual(21, m[1, 2], 0.0000001);
            Assert.AreEqual(21, m[2, 0], 0.0000001);
            Assert.AreEqual(12, m[2, 1], 0.0000001);
            Assert.AreEqual(8, m[2, 2], 0.0000001);
        }

        [TestMethod]
        public void Matrix_TraceTest()
        {
            Matrix m = new Matrix(5, 5);
            m.MakeIdentity();

            Assert.AreEqual(5, m.Trace());
        }
    }
}
