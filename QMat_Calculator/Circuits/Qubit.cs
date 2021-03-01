using Newtonsoft.Json;
using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 1:37:01 PM 
*/

namespace QMat_Calculator.Circuits
{
    public class Qubit
    {
        Guid guid = Guid.NewGuid();
        Matrix matrix;
        List<Gate> gates;

        public Guid getID() { return this.guid; }
        public Matrix getMatrix() { return this.matrix; }


        /// <summary>
        /// Create a Qubit.
        /// </summary>
        /// <param name="TrueValue"> Whether the Qubit is |1> or |0>. Default to |0>. </param>
        public Qubit(bool TrueValue = false)
        {
            // Define the matrix size and values.
            this.matrix = new Matrix(2, 1);
            if (TrueValue) { matrix.Update(1, 0, 1); }
            else { matrix.Update(0, 0, 1); }
            gates = new List<Gate>();
        }

        public List<Gate> getGates() { return gates; }
        public void setGates(List<Gate> g) { gates = g; }
        public bool hasGate(Gate g)
        {
            if (gates.Contains(g)) return true;
            return false;
        }

        public void removeGate(Gate gate)
        {
            if (hasGate(gate)) gates.Remove(gate);
        }

        public void addGate(Gate gate)
        {
            if (this.gates == null) this.gates = new List<Gate>();
            this.gates.Add(gate);
        }

    }
}
