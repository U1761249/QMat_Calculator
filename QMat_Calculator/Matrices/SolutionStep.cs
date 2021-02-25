﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 2/25/2021 10:25:18 AM 
*/

namespace QMat_Calculator.Matrices
{
    /// <summary>
    /// This class stores the steps for the solution to be displayed as the working for the solution.
    /// </summary>
    class SolutionStep
    {
        public enum MatrixFunction { Multiply, Tensor } // enum of different operations that can be used.

        private Matrix input1 = null;   // Value of the first input.
        private MatrixFunction mf;      // Function used on the inputs.
        private Matrix input2 = null;   // Value of the second input.
        private Matrix answer = null;   // Value of the output from the calculation.

        public Matrix getInput1() { return input1; }
        public MatrixFunction getFunction() { return mf; }
        public Matrix getInput2() { return input2; }
        public Matrix getAnswer() { return answer; }

        public SolutionStep(Matrix input1, MatrixFunction mf, Matrix input2, Matrix answer)
        {
            this.input1 = input1;
            this.mf = mf;
            this.input2 = input2;
            this.answer = answer;
        }

        override public string ToString()
        {
            StringBuilder s = new StringBuilder();

            int m1rows = input1.getRows();
            int m2rows = input2.getRows();

            int midrow = Convert.ToInt32(Math.Round(Convert.ToDouble(Math.Max(m1rows, m2rows) / 2))); // The middle row of the matrix with more rows.

            for (int row = 0; row < Math.Max(m1rows, m2rows); row++)
            {
                StringBuilder sb = new StringBuilder();
                // Add the first matrix row.
                if (row < m1rows)
                {
                    sb.Append(GetRowString(input1, row));
                }

                if (row == midrow) sb.Append(String.Format("{0, -2}", $" {FunctionString()} "));
                else sb.Append(String.Format("{0, -3}", "   "));

                // Add the second matrix row
                if (row < m2rows)
                {
                    sb.Append(GetRowString(input2, row));

                }
                s.AppendLine(sb.ToString());
            }

            s.AppendLine(answer.ToString());

            return s.ToString();
        }

        /// <summary>
        /// Return the specified row of the given matrix as a string.
        /// </summary>
        /// <param name="input1"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GetRowString(Matrix matrix, int row)
        {
            StringBuilder s = new StringBuilder();


            for (int c = 0; c < matrix.getColumns(); c++)
            {
                Complex value = matrix.getData()[row, c];
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

                s.Append(sb.ToString());
            }


            return s.ToString();
        }

        private string FunctionString()
        {
            switch (mf)
            {
                case MatrixFunction.Multiply:
                    return "·";

                case MatrixFunction.Tensor:
                    return "⊗";

                default:
                    return "unknown";
            }
        }
    }
}
