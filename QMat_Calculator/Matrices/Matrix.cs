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
    }
}
