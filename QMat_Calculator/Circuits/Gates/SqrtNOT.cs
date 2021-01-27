using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QMat_Calculator.Circuits.Gates
{
    class SqrtNOT : Gate
    {
        public SqrtNOT()
        {
            //   1  |1   -1 |
            //   √2 |1    1 |

            this.setNodeCount(1);


            Complex[,] data = new Complex[2, 2];
            data[0, 0] = 1;
            data[0, 1] = -1;
            data[1, 0] = 1;
            data[1, 1] = 1;

            Matrices.Matrix m = new Matrices.Matrix(2, 2, data, 1 / Math.Sqrt(2));
            this.setMatrix(m);
        }

        public string GetGateLabel() { return "√NOT"; }

    }
}
