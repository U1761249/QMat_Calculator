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
    public abstract class Gate
    {
        Guid guid = Guid.NewGuid(); // Each gate has a unique ID so two identical gates can be distinguished.
        int nodeCount;
        Matrix matrix;

        public void setNodeCount(int nodeCount) { this.nodeCount = nodeCount; }
        public void setMatrix(Matrix matrix) { this.matrix = matrix; }

        public int getNodeCount() { return this.nodeCount; }
        public Matrix getMatrix() { return this.matrix; }
        public Guid getGuid() { return this.guid; }
        public void newGuid() { this.guid = Guid.NewGuid(); }

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
            return matrix.getPreceder();
        }

    }


}
