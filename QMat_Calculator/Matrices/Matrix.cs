using QMat_Calculator.Circuits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 1:42:16 PM 
*/

namespace QMat_Calculator.Matrices
{
    public class Matrix
    {
        int rows;
        int columns;
        double preceder; // Some gates have fractions preceding them - E.G the Hadamard Gate
        Complex[,] values; // [Rows, Columns] of complex numbers (Real + Imaginary * i) where i^2 = -1

        public Matrix() { }
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;

            this.values = new Complex[rows, columns];
            this.preceder = -1;
        }
        public Matrix(int rows, int columns, double preceder, Complex[,] data)
        {
            this.rows = rows;
            this.columns = columns;

            this.values = data;
            this.preceder = preceder;
        }

        public int getRows() { return rows; }
        public int getColumns() { return columns; }
        public Complex[,] getData() { return values; }

        /// <summary>
        /// Update the value in the matrix.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void Update(int row, int column, int value)
        {
            if (!(row <= rows && column <= columns)) { throw new IndexOutOfRangeException("Row or Column specified was not within the matrix dimensions."); }

            this.values[row, column] = value;
        }

        public Matrix(int rows, int columns, Complex[,] data, double preceder = -1)
        {
            this.rows = rows;
            this.columns = columns;

            this.values = data;
            this.preceder = preceder;
        }



        /// <summary>
        /// Create an identity matrix of the given size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Matrix CreateIdentityMatrix(int size)
        {
            Matrix identity = new Matrix(size, size);
            for (int i = 0; i < size; i++)
            {
                identity.values[i, i] = 1;
            }
            return identity;
        }

        /// <summary>
        /// Write the matrix to the console/
        /// </summary>
        public void printMatrix(bool preceder = false)
        {
            Console.Write(ToString(preceder));
        }

        /// <summary>
        /// Write the matrix as a string.
        /// </summary>
        /// <returns> A String of this matrix's data </returns>

        public String ToString(bool showPreceder = false)
        {
            bool hasPreceder = false;
            string p = preceder.ToString();
            string space = "";
            double precederRow = Math.Floor((double)rows / 2);

            if (preceder != -1)
            {
                hasPreceder = true;
                foreach (char c in p) { space += " "; } // Aim to make the space the same length as the preceder.
            }

            if (!showPreceder) { hasPreceder = false; } // Don't show the preceder if it is not wanted.

            StringBuilder s = new StringBuilder();

            for (int r = 0; r < rows; r++)
            {
                // Add the preceder or space if there is a preceder.
                if (hasPreceder && r == precederRow) s.Append(String.Format("{0, -5}", FractionConverter.Convert(Convert.ToDouble(p))));
                else if (hasPreceder && r != precederRow) s.Append(String.Format("{0, -5}", space));

                for (int c = 0; c < columns; c++)
                {
                    Complex value = values[r, c];
                    StringBuilder sb = new StringBuilder();
                    sb.Append(String.Format("{0, -2}", value.Real));

                    if (value.Real == 0 && value.Imaginary != 0)
                    {
                        sb.Clear();
                    }
                    if (value.Imaginary != 0)
                    {
                        if (value.Imaginary == 1) { sb.Append(String.Format("{0, -2}", "+i")); }
                        else if (value.Imaginary == -1) { sb.Append(String.Format("{0, -2}", "-i")); }
                        else
                            sb.Append(String.Format("{0, -2}", value.Imaginary + "i"));
                    }
                    sb.Append(String.Format("{0, -2}", " "));

                    s.Append(String.Format("{0, -2}", sb.ToString()));
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        public double getPreceder()
        {
            return this.preceder;
        }

        /// <summary>
        /// Take the matrix from the gate and multiply it by C.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Matrix Multiply(Gate g, Complex c)
        {
            Matrix x = g.getMatrix();
            return Multiply(x, c);
        }


        /// <summary>
        /// Multiply a Matrix by a Constant
        /// </summary>
        /// <param name="x"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix x, Complex c)
        {
            //for (int i = 0; i < x.data.Length; i++)
            //{
            //    int row = i / x.rows;
            //    int col = i % x.rows;
            //    x.data[row, col] *= c;
            //}
            Matrix m = new Matrix(x.rows, x.columns, new Complex[x.rows, x.columns]);
            for (int r = 0; r < x.rows; r++)
            {
                for (int col = 0; col < x.columns; col++)
                {
                    Complex value = x.values[r, col];
                    value = value * c;
                    m.values[r, col] = value;
                }
            }

            return m;
        }

        /// <summary>
        /// Take the matrix from the gate and multiply it by another matrix.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Multiply(Gate g, Matrix y)
        {
            Matrix x = g.getMatrix();
            return Multiply(x, y);
        }
        /// <summary>
        /// Take the matrix from the gate and multiply it by another matrix.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix x, Gate g)
        {
            Matrix y = g.getMatrix();
            return Multiply(x, y);
        }
        /// <summary>
        /// Take the matrix from the gate and multiply it by the matrix of another gate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Multiply(Gate x, Gate y)
        {
            Matrix a = x.getMatrix();
            Matrix b = y.getMatrix();
            return Multiply(a, b);
        }
        /// <summary>
        /// Take the matrix from the qubit and multiply it by the matrix of another qubit.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Multiply(Qubit x, Qubit y)
        {
            Matrix a = x.getMatrix();
            Matrix b = y.getMatrix();
            return Multiply(a, b);
        }


        /// <summary>
        /// Multiply two Matrices and their preceders.
        /// The Number of Columns in X MUST be the same as the number of Rows in Y.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix x, Matrix y)
        {
            Matrix m = new Matrix();

            if (x.columns != y.rows) // Use an identity matrix to make the sizes compatible
            {
                Matrix[] resized = IdentityMatrix(x, y);
                x = resized[0];
                y = resized[1];
            }

            m.rows = x.rows;
            m.columns = y.columns;
            m.values = new Complex[m.rows, m.columns];

            // Calculate the preceder for M.
            if (x.preceder != -1 && y.preceder != -1) { m.preceder = x.preceder * y.preceder; }
            else if (x.preceder != -1) { m.preceder = x.preceder; }
            else if (y.preceder != -1) { m.preceder = y.preceder; }
            else { m.preceder = -1; }

            //Calculate the data for M.
            for (int r = 0; r < x.rows; r++)
            {
                for (int c = 0; c < y.columns; c++)
                {
                    Complex total = 0;
                    for (int i = 0; i < x.columns; i++)
                    {
                        total += x.values[r, i] * y.values[i, c];
                    }
                    m.values[r, c] = total;
                }
            }
            return m;
        }

        /// <summary>
        /// Resize the smaller matrix using an identity matrix.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private static Matrix[] IdentityMatrix(Matrix x, Matrix y)
        {
            Matrix smaller = y;
            Matrix larger = x;
            bool smallerX = false;

            if (x.columns < y.rows)
            {
                smaller = x;
                larger = y;
                smallerX = true;
            }


            // Assuming X < Y and the order is X*Y, X must have the same number of columns as Y has rows.

            if (larger.rows % smaller.columns == 0) // Check if there is a multiple for the number of rows
            {
                int size = larger.rows / smaller.columns;
                Matrix identity = CreateIdentityMatrix(size);
                smaller = Tensor(smaller, identity); // Increase the size of the Smaller
            }

            if (smallerX) { x = smaller; }
            else { y = smaller; }

            Matrix[] output = new Matrix[2] { x, y };
            return output;
        }



        /// <summary>
        /// Take the matrix from the gate and calculate the Tensor product with another matrix.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Tensor(Gate g, Matrix y)
        {
            Matrix x = g.getMatrix();
            return Tensor(x, y);
        }
        /// <summary>
        /// Take the matrix from the gate and calculate the Tensor product with another matrix.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static Matrix Tensor(Matrix x, Gate g)
        {
            Matrix y = g.getMatrix();
            return Tensor(x, y);
        }
        /// <summary>
        /// Take the matrix from the gate and calculate the Tensor product with the matrix of another gate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Tensor(Gate x, Gate y)
        {
            Matrix a = x.getMatrix();
            Matrix b = y.getMatrix();
            return Tensor(a, b);
        }
        /// <summary>
        /// Take the matrix from the qubit and calculate the Tensor product with the matrix of another qubit.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Tensor(Qubit x, Qubit y)
        {
            Matrix a = x.getMatrix();
            Matrix b = y.getMatrix();
            return Tensor(a, b);
        }

        /// <summary>
        /// Calculate the Tensor product of two Matrices
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Tensor(Matrix x, Matrix y)
        {
            Matrix m = new Matrix(x.rows * y.rows, x.columns * y.columns);

            // Calculate the preceder for M.
            if (x.preceder != -1 && y.preceder != -1) { m.preceder = x.preceder * y.preceder; }
            else if (x.preceder != -1) { m.preceder = x.preceder; }
            else if (y.preceder != -1) { m.preceder = y.preceder; }
            else { m.preceder = -1; }

            //Calculate the Tensor product
            for (int r = 0; r < x.rows; r++)
            {
                for (int c = 0; c < x.columns; c++)
                {
                    Matrix d = Multiply(y, x.values[r, c]); // Calculate the data as Y * the cell value in X
                    for (int i = 0; i < y.rows; i++) // Place the data within m.data
                    {
                        for (int j = 0; j < y.columns; j++)
                        {
                            int row = (r * y.rows) + i;
                            int col = (c * y.columns) + j;

                            m.values[row, col] = d.values[i, j];
                        }
                    }
                }
            }

            return m;
        }
    }
}
