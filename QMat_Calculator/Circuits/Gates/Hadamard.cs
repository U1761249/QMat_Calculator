﻿using QMat_Calculator.Matrices;
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
    public class Hadamard : Gate
    {

        public Hadamard()
        {
            //   1  |1   1 |
            //   √2 |1   -1|

            this.setNodeCount(1);


            Complex[,] data = new Complex[2, 2];
            data[0, 0] = 1;
            data[0, 1] = 1;
            data[1, 0] = 1;
            data[1, 1] = -1;

            Matrix m = new Matrix(2, 2, data, 1 / Math.Sqrt(2));
            this.setMatrix(m);

        }

        public Hadamard(Guid guid, int nodeCount, Matrix m)
        {
            this.setGuid(guid);
            this.setNodeCount(nodeCount);
            this.setMatrix(m);
        }


        public string GetGateLabel() { return "H"; }

    }
}
