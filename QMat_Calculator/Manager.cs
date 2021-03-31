using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Drawable;
using QMat_Calculator.Interfaces;
using QMat_Calculator.Matrices;
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
        private static int minQubitCount = 1;

        private static CircuitCanvas circuitCanvas;
        private static MatrixCanvas matrixCanvas;
        private static CircuitComponent selectedGate = null;
        private static ControlQubit selectedControl = null;
        private static List<Qubit> qubits = new List<Qubit>();
        private static List<SolutionStep> solutionSteps = new List<SolutionStep>();
        private static Gate heldGate = null;
        private static bool DoDetails = false;
        private static int requiredColumns = 1;
        public static void setDoDetails(bool value)
        {
            DoDetails = value;
        }
        public static bool getDoDetails() { return DoDetails; }

        private static Dictionary<string, Type> GateImage = null;
        private static UserControl[,] finalAlignment = null;

        //Drag and Drop components
        private static UIElement ccDrag = null;
        private static Point offsetDrag;
        public static void setHeldGate(Gate g) { heldGate = g; }
        public static Gate getHeldGate() { return heldGate; }
        public static void setSelectedGate(CircuitComponent g) { selectedGate = g; }
        public static CircuitComponent getSelectedGate() { return selectedGate; }
        public static void setSelectedControl(ControlQubit g) { selectedControl = g; }
        public static ControlQubit getSelectedControl() { return selectedControl; }
        public static UserControl[,] getFinalAlignment() { return finalAlignment; }
        public static void setFinalAlignment(UserControl[,] al) { finalAlignment = al; }
        public static int getRequiredColumns() { return requiredColumns; }
        public static void setRequiredColumns(int rc) { requiredColumns = rc; }
        public static int getMinQubitCount() { return minQubitCount; }


        public static void setMinQubitCount(int val) { minQubitCount = val; }

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

        /// <summary>
        /// Return the QubitComponent closest to the given Y coordinate.
        /// </summary>
        /// <param name="targetY"></param>
        /// <param name="qubits"></param>
        /// <returns></returns>
        public static QubitComponent getClosestQubitComponent(double targetY, List<QubitComponent> qubits)
        {
            double closestY = double.PositiveInfinity;
            QubitComponent chosen = null;
            foreach (QubitComponent qc in qubits)
            {
                if (chosen == null) { chosen = qc; }

                double qcY = Math.Abs(qc.GetPoint().Y - targetY);
                if (qcY < closestY)
                {
                    closestY = qcY;
                    chosen = qc;
                }
            }

            return chosen;
        }

        public static void setCircuitCanvas(CircuitCanvas cc) { circuitCanvas = cc; }
        public static MatrixCanvas getMatrixCanvas() { return matrixCanvas; }

        public static void UpdateHeight(CircuitComponent component, double height)
        {
            Canvas.SetTop(component, height - (component.ActualHeight / 2));
            Point circuitPoint = component.getPoint();
            circuitPoint.Y = height - (component.ActualHeight / 2);
            component.setPoint(circuitPoint);
        }
        public static void UpdateHeight(ControlQubit component, double height)
        {
            Canvas.SetTop(component, height - (component.ActualHeight / 2));
            Point circuitPoint = component.getPoint();
            circuitPoint.Y = height - (component.ActualHeight / 2);
            component.setPoint(circuitPoint);
        }

        public static void setMatrixCanvas(MatrixCanvas mc) { matrixCanvas = mc; }
        public static UIElement getCCDrag() { return ccDrag; }
        public static Point getOffsetDrag() { return offsetDrag; }
        public static void setCCDrag(UIElement cc) { ccDrag = cc; }
        public static void setOffsetDrag(Point p) { offsetDrag = p; }
        public static void setSpacing(double d) { spacing = d; }
        public static double getSpacing() { return spacing; }


        public static List<Qubit> getQubits() { return qubits; }
        public static void setQubits(List<Qubit> q) { qubits = q; }
        public static List<SolutionStep> GetSolutionSteps() { return solutionSteps; }
        public static void setSolutionSteps(List<SolutionStep> ss) { solutionSteps = ss; }
        public static int getQubitCount() { return qubits.Count; }
        public static void addQubit(ref Qubit q) { qubits.Add(q); }
        public static void removeQubit(int index) { qubits.RemoveAt(index); }

        public static void addQubit() { circuitCanvas.AddQubit(); }

        public static void AssignControlBits()
        {
            Gate gate = ((CircuitComponent)ccDrag).getGate();
            List<ControlQubit> controlBits = ((CircuitComponent)ccDrag).getControlQubits();
            List<int> blackListIndexes = new List<int>();

            foreach (ControlQubit bit in controlBits)
            {
                bool assigned = false;

                ControlBit control = bit.getControlBit();
                for (int i = 0; i < qubits.Count; i++)
                {
                    if (qubits[i].hasGate(gate))// Detect the primary gate and blacklist the index.
                    {
                        if (!blackListIndexes.Contains(i)) blackListIndexes.Add(i);

                        if (qubits[i].hasGate(control)) // Detect if the control bit is assigned and invalid.
                        {
                            qubits[i].removeGate(control);
                            assigned = false;
                        }
                    }
                    else if (qubits[i].hasGate(control)) // Detect if the control bit is assigned and valid.
                    {
                        assigned = true;
                        blackListIndexes.Add(i);
                    }
                }

                if (!assigned) // Assign the unassigned control bits to the first available qubit.
                {
                    for (int i = 0; i < qubits.Count; i++)
                    {
                        if (!blackListIndexes.Contains(i))
                        {
                            blackListIndexes.Add(i);
                            qubits[i].addGate(control);
                            break;
                        }
                    }
                }
            }



        }

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

        public static void removeQubit()
        {
            int lastIndex = qubits.Count() - 1;
            if (qubits[lastIndex].getGates().Count == 0 && qubits.Count() > minQubitCount + 1)
            {
                //qubits.RemoveAt(lastIndex);
                circuitCanvas.RemoveLastQubit();
            }
            else
            {
                MessageBox.Show($"Unable to remove the last qubit. \r\nCheck it is empty and no gate uses {minQubitCount} qubits.");
            }
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

            // Update the minQubitCount value;
            List<Gate> gates = new List<Gate>();
            foreach (Qubit q in qubits) gates.AddRange(q.getGates());
            if (gates.Count() > 0)
                minQubitCount = gates.GroupBy(x => x.getNodeCount()).Select(x => x.First().getNodeCount()).ToList().Max(); // Set the minQubitCount to the largest NodeCount of all gates.
            else minQubitCount = 1;


            // Remove the gate from the canvas.
            circuitCanvas.MainCircuitCanvas.Children.Remove(selectedGate);
            for (int i = 0; i < selectedGate.getControlQubits().Count; i++)
            {
                // Remove all control bits associated with the gate
                circuitCanvas.MainCircuitCanvas.Children.Remove(selectedGate.getControlQubits()[i]);
            }
            selectedGate = null;
            circuitCanvas.OrderComponents();

        }

        /// <summary>
        /// Sort the contents of the components list on the X coordinate.
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public static List<UserControl> SortX(List<UserControl> components)
        {
            Dictionary<Point, UserControl> mapping = new Dictionary<Point, UserControl>();
            List<Point> sortedPoints = new List<Point>();
            List<UserControl> sorted = new List<UserControl>();

            // Get the point for each element (of each type) and map the point to the component.
            foreach (UserControl component in components)
            {
                Point p = new Point();
                if (component.GetType() == typeof(CircuitComponent))
                {
                    p = ((CircuitComponent)component).getPoint();
                }
                else if (component.GetType() == typeof(ControlQubit))
                {
                    p = ((ControlQubit)component).getPoint();
                }

                if (p == new Point() || mapping.ContainsKey(p)) continue;
                mapping.Add(p, component);
                sortedPoints.Add(p);
            }

            // Sort the points by X ascending, with Y ascending for each X.
            sortedPoints = sortedPoints.OrderBy(x => x.X).ThenBy(x => x.Y).ToList();
            // Add the mapped controls to the list based on the order of the points.
            for (int i = 0; i < sortedPoints.Count; i++)
            {
                if (mapping.ContainsKey(sortedPoints[i]))
                {
                    sorted.Add(mapping[sortedPoints[i]]);
                }
            }


            return sorted;
        }

        public static int getMostPopulated()
        {
            int mostPopulated = 0;
            foreach (Qubit q in Manager.getQubits())
            {
                if (q.getGates().Count > mostPopulated) mostPopulated = q.getGates().Count;
            }
            return mostPopulated;
        }

        /// <summary>
        /// Solve the circuit as a Matrix (From Right to Left)
        /// </summary>
        public static void Solve()
        {
            //Console.WriteLine("Solving");
            Matrix totalMatrix = null; // Current value of the circuit Matrix.
            solutionSteps = new List<SolutionStep>();
            string solutionEquation = "";

            List<CircuitComponent> components = new List<CircuitComponent>();
            foreach (var element in circuitCanvas.MainCircuitCanvas.Children)
            {
                if (element.GetType() == typeof(CircuitComponent)) { components.Add((CircuitComponent)element); }
            }
            components = components.OrderBy(x => x.getPoint().X).ToList();

            // Get lists of distinct X and Y values
            List<double> xValues = components.GroupBy(x => x.getPoint().X).Select(x => x.First().getPoint().X).ToList();
            List<double> yValues = components.GroupBy(x => x.getPoint().Y).Select(x => x.First().getPoint().Y).ToList();

            if (yValues.Count > qubits.Count || xValues.Count > getMostPopulated()) // Stop solving if there is a problem.
            {
                Console.WriteLine("There was a problem with the circuit. \r\n Try re-sorting it and try again.");
                //MessageBox.Show(PrintGateLayout());

                return;
            }

            for (int x = xValues.Count - 1; x >= 0; x--)
            {
                string tensorCalculations = "";
                // Get a list of all gates at the current X value.
                List<Gate> currentGates = components.Where(c => c.getPoint().X == xValues[x]).Select(c => c.getGate()).ToList();
                // Calculate the tensor product of all matrices of the current gates.
                Matrix stepMatrix = currentGates[0].getMatrix();
                if (tensorCalculations == "") tensorCalculations = currentGates[0].GetGateLabel();
                for (int i = 1; i < currentGates.Count; i++)
                {
                    Matrix result = Matrix.Tensor(stepMatrix, currentGates[i].getMatrix());
                    tensorCalculations = CalculateEquation(tensorCalculations, currentGates[i]);
                    solutionSteps.Add(new SolutionStep(stepMatrix, SolutionStep.MatrixFunction.Tensor, currentGates[i].getMatrix(), result, tensorCalculations));
                    stepMatrix = result;
                }


                if (totalMatrix != null)                // Multiply the overall matrix by the current tensor product.
                {
                    Matrix result = Matrix.Multiply(totalMatrix, stepMatrix);
                    solutionEquation = CalculateEquation(solutionEquation, tensorCalculations);
                    solutionSteps.Add(new SolutionStep(totalMatrix, SolutionStep.MatrixFunction.Multiply, stepMatrix, result, solutionEquation));
                    totalMatrix = result;
                }
                else
                {
                    totalMatrix = stepMatrix;
                    if (tensorCalculations.Contains("⊗"))
                        solutionEquation = $"({tensorCalculations})";
                    else
                        solutionEquation = $"{tensorCalculations}";
                }
            }

            //Console.Write(currentVal.ToString(true));

            if (totalMatrix != null)
                matrixCanvas.DisplaySolution(totalMatrix);
            //MessageBox.Show(PrintGateLayout());
        }

        /// <summary>
        /// Calculate the new multiplication within the equation.
        /// </summary>
        /// <param name="solutionEquation"></param>
        /// <param name="tensorCalculations"></param>
        /// <returns></returns>
        private static string CalculateEquation(string solutionEquation, string tensorCalculations)
        {
            // Add parentheses if there is a ⊗
            if (tensorCalculations.Contains("⊗"))
                return $"({tensorCalculations}) * {solutionEquation}";

            return $"{tensorCalculations} * {solutionEquation}";

        }

        /// <summary>
        /// Calculate the new tensor product within the equation.
        /// </summary>
        /// <param name="tensorCalculations"></param>
        /// <param name="gate"></param>
        /// <returns></returns>
        private static string CalculateEquation(string tensorCalculations, Gate gate)
        {
            return $"{gate.GetGateLabel()} ⊗ {tensorCalculations}";

        }
    }
}

