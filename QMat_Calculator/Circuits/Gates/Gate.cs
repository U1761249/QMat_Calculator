using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 1:34:41 PM 
*/

namespace QMat_Calculator.Circuits
{
    /// <summary>
    /// Superclass type for all gates.
    /// No "Gate" can ever be created - they must always be a subclass.
    /// </summary>
    abstract class Gate
    {
        public int nodeCount;
        public List<Qubit> qubits;
        public Matrix matrix;

        public void printMatrix()
        {
            matrix.printMatrix();
        }

        override
        public string ToString()
        {
            return matrix.ToString();
        }
    }


}
