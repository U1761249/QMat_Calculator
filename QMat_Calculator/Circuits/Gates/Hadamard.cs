using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 3:02:44 PM 
*/

namespace QMat_Calculator.Circuits.Gates
{
    class Hadamard : Gate
    {

        public Hadamard(Qubit q)
        {
            //   1  |1   1 |
            //   √2 |1   -1|

            this.nodeCount = 1;
            this.qubits = new List<Qubit> { q }; // Add the provided Qubit as elenent 0 in the list.


            double[,] data = new double[2, 2];
            data[0, 0] = 1;
            data[0, 1] = 1;
            data[1, 0] = 1;
            data[1, 1] = -1;

            Matrices.Matrix m = new Matrices.Matrix(2, 2, data, 1 / Math.Sqrt(2));
            this.matrix = m;

        }


    }
}
