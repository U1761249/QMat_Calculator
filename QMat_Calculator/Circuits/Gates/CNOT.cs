using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 3:02:44 PM 
*/

namespace QMat_Calculator.Circuits.Gates
{
    class CNOT : Gate
    {
        public CNOT()
        {
            Setup();
        }

        private void Setup()
        {

            //   |1 0 0 0|
            //   |0 1 0 0|
            //   |0 0 0 1|
            //   |0 0 1 0|

            this.setNodeCount(2);


            Complex[,] data = new Complex[4, 4];
            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 3] = 1;
            data[3, 2] = 1;

            Matrix m = new Matrix(4, 4, data);
            this.setMatrix(m);
        }

        public CNOT(Guid guid, int nodeCount, Matrix m)
        {
            this.setGuid(guid);
            this.setNodeCount(nodeCount);
            this.setMatrix(m);
        }
        public string GetGateLabel() { return "NOT"; }
    }
}
