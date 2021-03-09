using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QMat_Calculator.Circuits.Gates
{
    class Toffoli : Gate
    {
        public Toffoli()
        {
            Setup();
        }

        private void Setup()
        {

            //   |1 0 0 0 0 0 0 0|
            //   |0 1 0 0 0 0 0 0|
            //   |0 0 1 0 0 0 0 0|
            //   |0 0 0 1 0 0 0 0|
            //   |0 0 0 0 1 0 0 0|
            //   |0 0 0 0 0 1 0 0|
            //   |0 0 0 0 0 0 0 1|
            //   |0 0 0 0 0 0 1 0|

            this.setNodeCount(3);


            Complex[,] data = new Complex[8, 8];
            // All elements are initialised to 0
            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 2] = 1;
            data[3, 3] = 1;
            data[4, 4] = 1;
            data[5, 5] = 1;
            data[6, 7] = 1;
            data[7, 6] = 1;

            Matrix m = new Matrix(4, 4, data);
            this.setMatrix(m);
        }

        public Toffoli(Guid guid, int nodeCount, Matrix m)
        {
            this.setGuid(guid);
            this.setNodeCount(nodeCount);
            this.setMatrix(m);
        }

        public string GetGateLabel() { return "T"; }

    }
}
