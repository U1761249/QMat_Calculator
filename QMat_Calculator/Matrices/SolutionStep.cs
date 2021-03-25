using System;
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
        private string equation = null;  // String value for the step

        public Matrix getInput1() { return input1; }
        public MatrixFunction getFunction() { return mf; }
        public Matrix getInput2() { return input2; }
        public Matrix getAnswer() { return answer; }
        public string getEquation() { return equation; }

        public SolutionStep(Matrix input1, MatrixFunction mf, Matrix input2, Matrix answer, string equation = "unknown")
        {
            this.input1 = input1;
            this.mf = mf;
            this.input2 = input2;
            this.answer = answer;
            this.equation = equation;
        }

        /// <summary>
        /// Get the number of preceders within the current step.
        /// </summary>
        /// <returns></returns>
        public int getPreceederCount()
        {
            int preceders = 0;

            if (input1.getPreceder() != -1) preceders++;
            if (input2.getPreceder() != -1) preceders++;
            if (answer.getPreceder() != -1) preceders++;

            return preceders;
        }
        public string FunctionString()
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
