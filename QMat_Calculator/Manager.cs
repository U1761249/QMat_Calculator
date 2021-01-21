using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 1:33:05 PM 
*/

namespace QMat_Calculator
{
    /// <summary>
    /// /// This class stores persistent information throughout the runtime of the program.
    /// </summary>
    static class Manager
    {
        public static List<Qubit> qubits = new List<Qubit>();
        private static Gate heldGate = null;
        private static Dictionary<string, Gate> GateImage = null;


        public static void setHeldGate(Gate g) { heldGate = g; }
        public static Gate getHeldGate() { return heldGate; }

        /// <summary>
        /// Use an image to define the gate to add.
        /// </summary>
        /// <param name="i"></param>
        public static void setHeldGate(Image i)
        {
            getGateImageDictionary();
            if (GateImage.ContainsKey(i.Name))
            {
                setHeldGate(GateImage[i.Name]);
            }
        }

        public static Dictionary<string, Gate> getGateImageDictionary()
        {
            if (GateImage == null) // Create a default dictionary
            {
                Qubit q = new Qubit(); // Empty Qubit for definition
                GateImage = new Dictionary<string, Gate>() {
                    {"imageHadamard", new Hadamard(q) },
                    {"imagePauliX", new Pauli(q, Pauli.PauliType.X) },
                    {"imagePauliY", new Pauli(q, Pauli.PauliType.Y) },
                    {"imagePauliZ", new Pauli(q, Pauli.PauliType.Z) },
                    {"imageCNOT", new CNOT(q, q) },
                    {"imageSqrtNOT", new SqrtNOT(q) },
                    {"imageToffoli", new Toffoli(q,q,q) }
                };
            }
            return GateImage;
        }
    }

    //TODO: Calculate the number of Qubits used and the appropriate Kronecker product for the gates used.
}

