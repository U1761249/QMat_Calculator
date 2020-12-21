using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 1:42:16 PM 
*/

namespace QMat_Calculator.Matrices
{
    class Matrix
    {
        int rows;
        int columns;
        double preceder; // Some gates have fractions preceding them - E.G the Hadamard Gate
        double[,] data; // [Rows, Columns]

        public Matrix() { }
        public Matrix(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;

            this.data = new double[rows, columns];
            this.preceder = -1;
        }
        public Matrix(int rows, int columns, double[,] data, double preceder = -1)
        {
            this.rows = rows;
            this.columns = columns;

            this.data = data;
            this.preceder = preceder;
        }

        /// <summary>
        /// Write the matrix to the console/
        /// </summary>
        public void printMatrix()
        {
            Console.Write(ToString());
        }

        /// <summary>
        /// Write the matrix as a string.
        /// </summary>
        /// <returns> A String of this matrix's data </returns>
        override
        public String ToString()
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

            StringBuilder s = new StringBuilder();

            for (int r = 0; r < rows; r++)
            {
                // Add the preceder or space if there is a preceder.
                if (hasPreceder && r == precederRow) s.Append(p);
                else if (hasPreceder && r != precederRow) s.Append(space);

                for (int c = 0; c < columns; c++)
                {
                    s.Append($" {data[r, c]}");
                }
                s.Append(Environment.NewLine);
            }

            return s.ToString();
        }

        /// <summary>
        /// Multiply a Matrix by a Constant
        /// </summary>
        /// <param name="x"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix x, double c)
        {
            for (int i = 0; i < x.data.Length; i++)
            {
                int row = i / x.rows;
                int col = i % x.rows;
                x.data[row, col] *= c;
            }

            return x;
        }

        /// <summary>
        /// Multiply two Matrices and their preceders.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Matrix Multiply(Matrix x, Matrix y)
        {
            Matrix m = new Matrix();
            if (x.rows == y.rows && x.columns == y.columns // If the matrices are the correct sixe to multiply
                || x.rows == y.columns)
            {
                m.rows = x.rows;
                m.columns = y.columns;
                m.data = new double[m.rows, m.columns];
            }
            else // Use an identity matrix to make the sizes equal
            {

            }

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
                    double total = 0;
                    for (int i = 0; i < x.columns; i++)
                    {
                        total += x.data[r, i] * y.data[i, c];
                    }
                    m.data[r, c] = total;
                }
            }
            return m;
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
                    Matrix d = Multiply(y, x.data[r, c]); // Calculate the data as Y * the cell value in X
                    for (int i = 0; i < y.rows; i++)
                    {
                        for (int j = 0; j < y.columns; j++)
                        {
                            int row = (r * x.rows) + i;
                            int col = (c * x.columns) + j;

                            m.data[row, col] = d.data[i, j];
                        }
                    }
                }
            }

            return m;
        }
    }
}
