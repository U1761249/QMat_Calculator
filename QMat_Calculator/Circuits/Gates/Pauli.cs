using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMat_Calculator.Circuits.Gates
{
    class Pauli : Gate
    {
        public enum PauliType { X, Y, Z };
        private PauliType gateType;
        public Pauli(Qubit q, PauliType type)
        {
            //      X        Y        Z
            //   |0    1| |0   -i| |1    0|
            //   |1    0| |i    0| |0   -1|

            this.setNodeCount(1);
            this.setQubits(q);
            this.gateType = type;


            double[,] data = new double[2, 2];

            switch (type)
            {
                case (PauliType.X):
                    data[0, 0] = 0;
                    data[0, 1] = 1;
                    data[1, 0] = 1;
                    data[1, 1] = 0;
                    break;

                case (PauliType.Y): // TODO: replace 1 and -1 with the value for the imaginary number "i"
                    data[0, 0] = 0;
                    data[0, 1] = -1;
                    data[1, 0] = 1;
                    data[1, 1] = 0;
                    break;

                case (PauliType.Z):
                    data[0, 0] = 1;
                    data[0, 1] = 0;
                    data[1, 0] = 0;
                    data[1, 1] = -1;
                    break;
            }

            Matrices.Matrix m = new Matrices.Matrix(2, 2, data);
            this.setMatrix(m);

        }

        public PauliType GetPauliType() { return this.gateType; }

    }
}
