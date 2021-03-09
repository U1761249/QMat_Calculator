using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QMat_Calculator.Circuits.Gates
{
    class Pauli : Gate
    {
        public enum PauliType { X, Y, Z };
        private PauliType gateType;
        public Pauli(PauliType type)
        {
            //      X        Y        Z
            //   |0    1| |0   -i| |1    0|
            //   |1    0| |i    0| |0   -1|

            this.setNodeCount(1);
            this.gateType = type;


            Complex[,] data = new Complex[2, 2];

            switch (type)
            {
                case (PauliType.X):
                    data[0, 0] = 0;
                    data[0, 1] = 1;
                    data[1, 0] = 1;
                    data[1, 1] = 0;
                    break;

                case (PauliType.Y):
                    data[0, 0] = 0;
                    data[0, 1] = new Complex(0, -1);
                    data[1, 0] = new Complex(0, 1);
                    data[1, 1] = 0;
                    break;

                case (PauliType.Z):
                    data[0, 0] = 1;
                    data[0, 1] = 0;
                    data[1, 0] = 0;
                    data[1, 1] = -1;
                    break;
            }

            Matrix m = new Matrix(2, 2, data);
            this.setMatrix(m);

        }

        public Pauli(Guid guid, PauliType type, int nodeCount, Matrix m)
        {
            this.setGuid(guid);
            this.gateType = type;
            this.setNodeCount(nodeCount);
            this.setMatrix(m);
        }

        public PauliType GetPauliType() { return this.gateType; }

        public string GetGateLabel()
        {
            switch (gateType)
            {
                case (PauliType.X): return "X";
                case (PauliType.Y): return "Y";
                case (PauliType.Z): return "Z";
                default: return "";
            }
        }


    }
}
