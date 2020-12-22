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
        public CNOT(Qubit root, Qubit control)
        {
            List<Qubit> q = new List<Qubit>() { root, control };
            Setup(q);
        }
        public CNOT(List<Qubit> q)
        {
            Setup(q);
        }

        private void Setup(List<Qubit> q)
        {

            //   |1 0 0 0|
            //   |0 1 0 0|
            //   |0 0 0 1|
            //   |0 0 1 0|

            this.setNodeCount(2);
            this.setQubits(q);


            Complex[,] data = new Complex[4, 4];
            data[0, 0] = 1;
            data[1, 1] = 1;
            data[2, 3] = 1;
            data[3, 2] = 1;

            Matrices.Matrix m = new Matrices.Matrix(4, 4, data);
            this.setMatrix(m);
        }


    }
}
