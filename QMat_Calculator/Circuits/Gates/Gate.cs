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
        int nodeCount;
        List<Qubit> qubits;
        Matrix matrix;

        public void setNodeCount(int nodeCount) { this.nodeCount = nodeCount; }
        public void setQubits(Qubit qubit) { this.qubits = new List<Qubit>() { qubit }; }
        public void setQubits(List<Qubit> qubits) { this.qubits = qubits; }
        public void setMatrix(Matrix matrix) { this.matrix = matrix; }

        public int getNodeCount() { return this.nodeCount; }
        public List<Qubit> getQubits() { return this.qubits; }
        public Matrix getMatrix() { return this.matrix; }


        public void printMatrix(bool preceder = false)
        {
            matrix.printMatrix(preceder);
        }

        public string ToString(bool preceder = false)
        {
            return matrix.ToString(preceder);
        }

        public double GetPreceder()
        {
            return matrix.GetPreceder();
        }



    }


}
