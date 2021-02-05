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
        private static CircuitComponent selectedGate = null;
        private static List<Qubit> qubits = new List<Qubit>();
        private static Gate heldGate = null;
        private static Dictionary<string, Type> GateImage = null;

        //Drag and Drop components
        private static UIElement ccDrag = null;
        private static Point offsetDrag;
        public static void setHeldGate(Gate g) { heldGate = g; }
        public static Gate getHeldGate() { return heldGate; }
        public static void setSelectedGate(CircuitComponent g) { selectedGate = g; }
        public static CircuitComponent getSelectedGate() { return selectedGate; }

        private static double spacing;

        /// <summary>
        /// Use an image to define the gate to add.
        /// </summary>
        /// <param name="i"></param>
        public static void setHeldGate(Image i)
        {
            getGateImageDictionary();
            if (GateImage.ContainsKey(i.Name))
            {
                Gate g = CreateGate(GateImage[i.Name], i.Name);
                g.newGuid();
                setHeldGate(g);
                //GateImage = null;
            }
        }

        private static Gate CreateGate(Type t, string imageName)
        {
            if (t == typeof(Hadamard))
            {
                return new Hadamard();
            }
            else if (t == typeof(Pauli))
            {
                switch (imageName.Last()) // Use the last letter of the name to get the Pauli Type.
                {
                    case ('X'): return new Pauli(Pauli.PauliType.X);
                    case ('Y'): return new Pauli(Pauli.PauliType.Y);
                    case ('Z'): return new Pauli(Pauli.PauliType.Z);
                }
            }
            else if (t == typeof(CNOT))
            {
                return new CNOT();
            }
            else if (t == typeof(SqrtNOT))
            {
                return new SqrtNOT();
            }
            //else if (t == typeof(Deutsch)) 
            //{

            //}
            else if (t == typeof(Toffoli))
            {
                return new Toffoli();
            }
            return null;
        }

        public static Dictionary<string, Type> getGateImageDictionary()
        {
            if (GateImage == null) // Create a default dictionary
            {
                GateImage = new Dictionary<string, Type>() {
                    {"imageHadamard", typeof(Hadamard) },
                    {"imagePauliX", typeof(Pauli) },
                    {"imagePauliY", typeof(Pauli) },
                    {"imagePauliZ", typeof(Pauli) },
                    {"imageCNOT", typeof(CNOT) },
                    {"imageSqrtNOT", typeof(SqrtNOT) },
                    {"imageToffoli", typeof(Toffoli) }
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
        public static void setSpacing(double d) { spacing = d; }
        public static double getSpacing() { return spacing; }


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

        /// <summary>
        /// Sort the gates on all qubits based on the position of the gate.
        /// </summary>
        /// <param name="components"></param>
        public static void SortComponents(List<CircuitComponent> components)
        {
            // Get each unique Y value from the list of components
            List<double> qubitHeightValues = components.GroupBy(x => x.getPoint().Y).Select(x => x.First().getPoint().Y).ToList();

            for (int currentQubitIndex = 0; currentQubitIndex < qubitHeightValues.Count; currentQubitIndex++)
            {
                for (int i = 0; i < qubits.Count; i++)
                {
                    // Get a list of all components with the Y value of the current element in qubitHeightValues
                    List<CircuitComponent> qubitComponents = components.Where(x => x.getPoint().Y == qubitHeightValues[currentQubitIndex]).Select(x => (CircuitComponent)x).ToList();
                    if (qubitComponents.Count != qubits[i].getGates().Count) { continue; } // Skip this iteration.

                    // Sort the list by X position (Ascending)
                    qubitComponents = qubitComponents.OrderBy(x => x.getPoint().X).ToList();
                    // Get a list of gates;
                    List<Gate> qubitGates = qubitComponents.Select(x => x.getGate()).ToList();
                    qubits[i].setGates(qubitGates);

                }
            }

        }


        public static string PrintGateLayout()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < qubits.Count; i++)
            {
                Qubit q = qubits[i];
                sb.Append($"Qubit: {q.getGates().Count} gates\r\n");
                List<Gate> gates = q.getGates();
                for (int j = 0; j < gates.Count; j++)
                {
                    Gate g = gates[j];
                    sb.Append(g.GetType());
                    if (g.GetType() == typeof(Pauli))
                    {
                        sb.Append(((Pauli)g).GetPauliType().ToString());
                    }
                    sb.Append("\r\n");
                }
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public static void removeGate()
        {
            if (selectedGate == null)
            {
                MessageBox.Show("No gate is selected.");
                return;
            }

            // Remove the gate from the Qubit
            for (int i = 0; i < qubits.Count; i++)
            {
                if (qubits[i].getGates().Contains(selectedGate.getGate()))
                {
                    qubits[i].removeGate(selectedGate.getGate());
                    break;
                }
            }
            // Remove the gate from the canvas.
            circuitCanvas.MainCircuitCanvas.Children.Remove(selectedGate);
            selectedGate = null;
            circuitCanvas.OrderComponents();

        }

        //TODO: Calculate the number of Qubits used and the appropriate Kronecker product for the gates used.
    }
}

