using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Drawable;
using QMat_Calculator.Interfaces;
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
        private static CircuitCanvas circuitCanvas;

        private static List<Qubit> qubits = new List<Qubit>();
        private static Gate heldGate = null;
        private static Dictionary<string, Gate> GateImage = null;

        //Drag and Drop components
        private static UIElement ccDrag = null;
        private static Point offsetDrag;
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
                    {"imageHadamard", new Hadamard() },
                    {"imagePauliX", new Pauli(Pauli.PauliType.X) },
                    {"imagePauliY", new Pauli(Pauli.PauliType.Y) },
                    {"imagePauliZ", new Pauli(Pauli.PauliType.Z) },
                    {"imageCNOT", new CNOT() },
                    {"imageSqrtNOT", new SqrtNOT() },
                    {"imageToffoli", new Toffoli() }
                };
            }
            return GateImage;
        }

        public static CircuitCanvas getCircuitCanvas() { return circuitCanvas; }
        public static void setCircuitCanvas(CircuitCanvas cc) { circuitCanvas = cc; }
        public static UIElement getCCDrag() { return ccDrag; }
        public static Point getOffsetDrag() { return offsetDrag; }
        public static void setCCDrag(UIElement cc) { ccDrag = cc; }
        public static void setOffsetDrag(Point p) { offsetDrag = p; }



        public static List<Qubit> getQubits() { return qubits; }
        public static void setQubits(List<Qubit> q) { qubits = q; }
        public static int getQubitCount() { return qubits.Count; }
        public static void addQubit(ref Qubit q) { qubits.Add(q); }
        public static void removeQubit(Qubit q) { qubits.Remove(q); }
        public static void removeQubit(int index) { qubits.RemoveAt(index); }

        public static void addQubit() { circuitCanvas.AddQubit(); }

        /// <summary>
        /// Remove the given gate from all qubits.
        /// </summary>
        /// <param name="gate"></param>
        public static void Decouple(Gate gate)
        {
            for (int i = 0; i < qubits.Count; i++)
            {
                if (qubits[i].hasGate(gate))
                {
                    qubits[i].removeGate(gate);
                }
            }
        }
    }

    //TODO: Calculate the number of Qubits used and the appropriate Kronecker product for the gates used.
}

