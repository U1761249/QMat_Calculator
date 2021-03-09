using QMat_Calculator.Circuits;
using QMat_Calculator.Drawable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/**
* @author Adam Birch - U1761249
*
* @date - 1/25/2021 10:14:54 AM 
*/

namespace QMat_Calculator.Interfaces
{
    /// <summary>
    /// Interaction logic for CircuitCanvas.xaml
    /// </summary>
    public partial class CircuitCanvas : UserControl
    {

        private List<QubitComponent> qubitComponents = new List<QubitComponent>();


        public void setQubitComponents(List<QubitComponent> qubits) { qubitComponents = qubits; }
        public void addQubitComponents(QubitComponent qubit) { qubitComponents.Add(qubit); }
        public List<QubitComponent> getQubitComponents() { return qubitComponents; }

        public CircuitCanvas()
        {
            InitializeComponent();
            AddQubit();

        }

        /// <summary>
        /// Drop the held component to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircuitCanvas_Drop(object sender, DragEventArgs e)
        {
            //MessageBox.Show($"Dropped Gate of type {Manager.getHeldGate().GetType()}");
            Point p = e.GetPosition(MainCircuitCanvas);
            CircuitComponent cc = new CircuitComponent(p);
            cc.setType(Manager.getHeldGate());

            MainCircuitCanvas.Children.Add(cc);
            int nodes = cc.getGate().getNodeCount();

            for (int i = 1; i < nodes; i++)
            {
                ControlQubit cq = new ControlQubit(p);
                cc.addControlQubit(cq);
                MainCircuitCanvas.Children.Add(cq);
            }

            if (nodes > Manager.getMinQubitCount())
            {
                Manager.setMinQubitCount(nodes);
            }
            if (nodes > Manager.getQubitCount())
            {
                while (nodes > Manager.getQubitCount()) // Add a qubit for each node.
                {
                    Manager.addQubit();
                }
            }
        }

        /// <summary>
        /// Change the location of the held item to the current position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircuitCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UIElement ccDrag = Manager.getCCDrag();
            Point offset = Manager.getOffsetDrag();

            if (ccDrag == null) return;

            Point position = e.GetPosition(sender as IInputElement);
            position.X -= offset.X;
            position.Y -= offset.Y;

            Canvas.SetTop(ccDrag, position.Y);
            Canvas.SetLeft(ccDrag, position.X);

            if (ccDrag.GetType() == typeof(CircuitComponent))
            {
                ((CircuitComponent)ccDrag).setPoint(position);
            }

            else if (ccDrag.GetType() == typeof(ControlQubit))
            {
                ((ControlQubit)ccDrag).setPoint(position);
            }



            Manager.setCCDrag(ccDrag);
        }

        /// <summary>
        /// Drop the held object into the correct place on the qubit, and add it to the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircuitCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Manager.getCCDrag() != null)
            {
                // Create a list of all QubitComponents on the screen.
                List<QubitComponent> qubits = new List<QubitComponent>();
                foreach (var line in MainCircuitCanvas.Children)
                {
                    if (line.GetType() == typeof(QubitComponent))
                    {
                        qubits.Add(((QubitComponent)line));
                    }
                }

                // Find the closest QubitComponent to where the user dropped the item.
                double targetY = e.GetPosition(MainCircuitCanvas).Y;

                QubitComponent chosen = Manager.getClosestQubitComponent(targetY, qubits);

                // Update the Qubits with the new association.
                if (chosen != null && Manager.getCCDrag().GetType() == typeof(CircuitComponent))
                {
                    Gate held = ((CircuitComponent)Manager.getCCDrag()).getGate();  // Get the gate
                    Manager.Decouple(held);                                         // Remove the gate from any qubits
                    chosen.AddGate(held);                                           // Add the gate to the chosen qubit

                    if (held.getNodeCount() > 1) { Manager.AssignControlBits(); }


                    // Set the height of the gate based on the qubit it is associated with.
                    CircuitComponent component = (CircuitComponent)Manager.getCCDrag();
                    double height = chosen.GetPoint().Y;
                    Manager.UpdateHeight(component, height);

                    if (component.getControlQubits().Count > 0)
                    {
                        // Update the height of the control bits
                        for (int i = 0; i < component.getControlQubits().Count; i++)
                        {
                            ControlQubit control = component.getControlQubits()[i];

                            foreach (QubitComponent qc in qubits)
                            {
                                if (qc.getQubit().getGates().Contains(control.getControlBit()))
                                {
                                    double targetHeight = qc.GetPoint().Y;
                                    Manager.UpdateHeight(control, targetHeight);
                                    break;
                                }
                            }
                        }
                    }

                    component.setGate(held);
                    Manager.setCCDrag(component);
                }

                Manager.setCCDrag(null);
                this.MainCircuitCanvas.ReleaseMouseCapture();

                //MessageBox.Show($"{Manager.getQubits()[0].getGates().Count} gates on the first Qubit");

                OrderComponents();
            }
        }

        /// <summary>
        /// Perform the changes when the canvas size changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainCircuitCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeQubits();
            OrderComponents();

        }

        /// <summary>
        /// Add a qubit into the system
        /// </summary>
        /// <param name="value"></param>
        public void AddQubit(bool value = false)
        {
            QubitComponent q = new QubitComponent(0, 0, value);
            MainCircuitCanvas.Children.Add(q);
            q.AddToManager();
            qubitComponents.Add(q);
            ResizeQubits();
            //MessageBox.Show($"Width: {width}");
            //MessageBox.Show($"Height: {height}");
        }

        public void RemoveLastQubit()
        {
            if (qubitComponents.Count() > 1)
            {
                MainCircuitCanvas.Children.Remove(qubitComponents.Last());
                qubitComponents.Remove(qubitComponents.Last());

                ResizeQubits();
            }
        }

        /// <summary>
        /// Change the size of the Qubits when the screen size changes.
        /// </summary>
        public void ResizeQubits()
        {
            int width = Convert.ToInt32(MainCircuitCanvas.ActualWidth);

            int count = Manager.getQubitCount();
            int qubitIndex = 0;

            if (count > 0)
            {
                for (int i = 0; i < MainCircuitCanvas.Children.Count; i++)
                {
                    if (MainCircuitCanvas.Children[i].GetType() == typeof(QubitComponent))
                    {
                        qubitIndex += 1;
                        int height = Convert.ToInt32(MainCircuitCanvas.ActualHeight);
                        double spacing = height / (count + 1);
                        height = Convert.ToInt32(spacing * qubitIndex);

                        Point point = new Point(width, height);

                        ((QubitComponent)MainCircuitCanvas.Children[i]).setPoint(point);

                        ((QubitComponent)MainCircuitCanvas.Children[i]).qubitChannel.X1 = 0;
                        ((QubitComponent)MainCircuitCanvas.Children[i]).qubitChannel.Y1 = point.Y;
                        ((QubitComponent)MainCircuitCanvas.Children[i]).qubitChannel.X2 = point.X;
                        ((QubitComponent)MainCircuitCanvas.Children[i]).qubitChannel.Y2 = point.Y;
                    }
                }
            }
        }

        /// <summary>
        /// Return the point value within new user controls.
        /// Works with CircuitComponent and ControlQubit
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        private Point getComponentPoint(UserControl component)
        {
            if (component.GetType() == typeof(CircuitComponent)) return ((CircuitComponent)component).getPoint();
            else if (component.GetType() == typeof(ControlQubit)) return ((ControlQubit)component).getPoint();

            return new Point(-1, -1);
        }

        /// <summary>
        /// Return the closest height value from the list to the height of the User Control.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="qubitHeightValues"></param>
        /// <returns></returns>
        private double ClosestQubit(UserControl component, List<double> qubitHeightValues)
        {
            Point p = getComponentPoint(component);
            double target = p.Y + (component.ActualHeight / 2);


            if (qubitHeightValues.Count == 0) return -1;
            else if (qubitHeightValues.Contains(target)) return target;

            double closest = double.PositiveInfinity;
            double closestDistance = double.PositiveInfinity;
            for (int i = 0; i < qubitHeightValues.Count; i++)
            {
                double distance = Math.Abs(target - qubitHeightValues[i]);
                if (distance < closestDistance)
                {
                    closest = qubitHeightValues[i];
                    closestDistance = distance;
                }
            }

            return closest;
        }

        /// <summary>
        /// Move the gates on screen to even columns within the Qubit.
        /// Gates with control bits reserve their own column per gate.
        /// </summary>
        public void OrderComponents()
        {
            int mostPopulated = Manager.getMostPopulated();

            List<UserControl> components = new List<UserControl>();
            List<QubitComponent> qubits = new List<QubitComponent>();

            for (int i = 0; i < MainCircuitCanvas.Children.Count; i++)
            {
                if (MainCircuitCanvas.Children[i].GetType() == typeof(CircuitComponent)) { components.Add((CircuitComponent)MainCircuitCanvas.Children[i]); }
                else if (MainCircuitCanvas.Children[i].GetType() == typeof(ControlQubit)) { components.Add((ControlQubit)MainCircuitCanvas.Children[i]); }
                else if (MainCircuitCanvas.Children[i].GetType() == typeof(QubitComponent)) { qubits.Add((QubitComponent)MainCircuitCanvas.Children[i]); }
            }

            List<double> qubitHeightValues = qubits.GroupBy(x => x.GetPoint().Y).Select(x => x.First().GetPoint().Y).ToList(); // Get a list of all unique Qubit heights

            UserControl[,] alignment = new UserControl[qubitHeightValues.Count, mostPopulated]; // Create an array based on the order of each component on each qubit.
            List<UserControl> OrderedComponents = Manager.SortX(components);
            int currentComponent = 0;
            if (components.Count == 0) return;

            for (int col = 0; col < mostPopulated; col++)
            {
                for (int row = 0; row < qubitHeightValues.Count; row++)
                {
                    if (ClosestQubit(OrderedComponents[currentComponent], qubitHeightValues) != qubitHeightValues[row]) continue;

                    alignment[row, col] = OrderedComponents[currentComponent];
                    currentComponent++;
                    if (currentComponent == OrderedComponents.Count) break;
                }
            }


            int requiredColumns = 1;

            // Calculate the number of additional columns needed.
            // Add a column if a column:
            //      Has controlled and non-controlled gates on the same column
            //      Has multiple controlled gates on a column

            for (int col = 0; col < Manager.getMostPopulated(); col++)
            {
                int controlledGates = 0;
                int nonControlledGates = 0;
                for (int row = 0; row < qubitHeightValues.Count; row++)
                {
                    if (alignment[row, col] == null) continue;
                    if (alignment[row, col].GetType() == typeof(CircuitComponent))
                    {
                        if (((CircuitComponent)alignment[row, col]).getGate().getNodeCount() > 1)
                        {
                            controlledGates++;
                        }
                        else nonControlledGates++;
                    }
                }

                if (controlledGates > 0 && nonControlledGates > 0) { requiredColumns += controlledGates; }
                else if (controlledGates > 0 && nonControlledGates == 0)
                {
                    if (controlledGates > 1)
                    {
                        requiredColumns += controlledGates - 1;
                    }
                }
                else if (controlledGates == 0 && nonControlledGates == 0) { } // Do nothing
                else { requiredColumns += 1; }
            }


            // Create a final array with a new column for each 
            UserControl[,] finalAlignment = new UserControl[qubitHeightValues.Count, requiredColumns];
            if (finalAlignment.Length == alignment.Length) { finalAlignment = alignment; }
            else
            {
                int colOffset = 0;
                for (int col = 0; col < Manager.getMostPopulated(); col++)
                {
                    int controlledGates = 0;
                    int nonControlledGates = 0;
                    bool isFirst = true;
                    int placedControlledIndex = 1;

                    for (int row = 0; row < qubitHeightValues.Count; row++) // Analyse the content of the column
                    {
                        if (alignment[row, col] == null) continue;
                        if (alignment[row, col].GetType() == typeof(CircuitComponent))
                        {
                            if (((CircuitComponent)alignment[row, col]).getGate().getNodeCount() > 1)
                            {
                                controlledGates++;
                            }
                            else nonControlledGates++;
                        }
                    }

                    for (int row = 0; row < qubitHeightValues.Count; row++) // Order the alignment column into the final column(s)
                    {
                        if (alignment[row, col] == null) continue;
                        if (alignment[row, col].GetType() == typeof(CircuitComponent))
                        {
                            CircuitComponent value = (CircuitComponent)alignment[row, col];
                            bool isControlled = value.getGate().getNodeCount() > 1;

                            if (!isControlled) // The gate goes to it's default column
                            {
                                finalAlignment[row, col + colOffset] = alignment[row, col];
                            }

                            else // The gate needs to be moved
                            {
                                if (controlledGates > 0 && nonControlledGates == 0 && isFirst) // All bar the first controlled gates are in a new column.
                                {
                                    if (colOffset > 0)
                                    { colOffset -= 1; }
                                    isFirst = false;
                                }

                                while (placedControlledIndex >= requiredColumns)
                                {
                                    placedControlledIndex--;
                                }

                                finalAlignment[row, col + colOffset + placedControlledIndex] = alignment[row, col];
                                placedControlledIndex += 1;
                            }
                        }
                    }
                    colOffset += controlledGates;

                }

                finalAlignment = MigrateControlBits(alignment, finalAlignment, qubitHeightValues);
            }

            Manager.setFinalAlignment(finalAlignment);
            MoveAll(ref finalAlignment, qubitHeightValues, qubitHeightValues.Count, requiredColumns);
        }

        /// <summary>
        /// Move the control bits from the alignment array to the finalAlignment array.
        /// </summary>
        /// <param name="alignment"></param>
        /// <param name="finalAlignment"></param>
        private UserControl[,] MigrateControlBits(UserControl[,] alignment, UserControl[,] finalAlignment, List<double> qubitHeightValues)
        {
            // Create a dictionary mapping the locations of control bits in the alignment array.
            Dictionary<ControlQubit, Point> mapping = new Dictionary<ControlQubit, Point>(); // Point X, Y = Row, Col of the control Qubit.
            for (int row = 0; row < qubitHeightValues.Count; row++)
            {
                for (int col = 0; col < Manager.getMostPopulated(); col++)
                {
                    UserControl component = alignment[row, col];
                    if (component == null) continue;

                    else if (component.GetType() == typeof(ControlQubit))
                    {
                        Point p = new Point(row, col);
                        mapping.Add((ControlQubit)component, p);
                    }
                }
            }

            // Move the mapped control bits to the same column as their parent gate.
            if (mapping.Count > 0)
            {
                for (int row = 0; row < qubitHeightValues.Count; row++)
                {
                    for (int col = 0; col < Manager.getMostPopulated(); col++)
                    {
                        UserControl component = alignment[row, col];
                        if (component == null) continue;

                        else if (component.GetType() == typeof(CircuitComponent))
                        {
                            List<ControlQubit> controlled = ((CircuitComponent)component).getControlQubits();
                            foreach (ControlQubit cont in controlled)
                            {
                                if (mapping.ContainsKey(cont))
                                {
                                    Point p = mapping[cont];
                                    int r = Convert.ToInt32(p.X);
                                    int c = Convert.ToInt32(p.Y);

                                    if (finalAlignment[r, col] == null)
                                        finalAlignment[r, col] = alignment[r, c]; // Copy the value directly from alignment into finalAlignment if the cell is null.
                                }
                            }
                        }
                    }
                }
            }

            return finalAlignment;
        }

        /// <summary>
        /// Move the components into the structure defined in OrderComponents();
        /// </summary>
        /// <param name="alignment"></param>
        private void MoveAll(ref UserControl[,] alignment, List<double> qubitHeightValues, double rowCount, double colCount)
        {
            double spacing = MainCircuitCanvas.ActualWidth / (colCount + 1);
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    Point point = new Point(spacing * (col + 1), qubitHeightValues[row]);

                    UserControl component = alignment[row, col];
                    if (component == null) continue; // Skip this elemet if it is null.
                    else if (component.GetType() == typeof(CircuitComponent)) { ((CircuitComponent)component).Move(point); }
                    else if (component.GetType() == typeof(ControlQubit)) { ((ControlQubit)component).Move(point); }
                }
            }
        }

        /// <summary>
        /// Move the gates on the screen and within the Qubit's list so that they are in the correct order.
        /// Does not account for control bits for gates like CNOT and Toffoli
        /// </summary>
        public void OldOrderComponents()
        {

            List<CircuitComponent> components = new List<CircuitComponent>();
            List<ControlQubit> controlQubits = new List<ControlQubit>();


            for (int i = 0; i < MainCircuitCanvas.Children.Count; i++)
            {
                if (MainCircuitCanvas.Children[i].GetType() == typeof(CircuitComponent))
                {
                    components.Add((CircuitComponent)MainCircuitCanvas.Children[i]);
                }
                else if (MainCircuitCanvas.Children[i].GetType() == typeof(ControlQubit))
                {
                    controlQubits.Add((ControlQubit)MainCircuitCanvas.Children[i]);
                }
            }

            if (components.Count > 0)
            {
                // Get a list of all unique heights (each different Qubit Y value)
                List<double> qubitHeightValues = components.GroupBy(x => x.getPoint().Y).Select(x => x.First().getPoint().Y).ToList();
                int mostPopulated = Manager.getMostPopulated();

                double spacing = MainCircuitCanvas.ActualWidth / (mostPopulated + 1);

                // Create a list of all spaced indexes.
                List<double> spaceIndexes = new List<double>();
                for (int i = 0; i < mostPopulated; i++)
                {
                    spaceIndexes.Add(spacing * (i + 1));
                }

                for (int qubitIndex = 0; qubitIndex < qubitHeightValues.Count; qubitIndex++) // Assign the X values for all gates on the current Qubit height.
                {
                    // Get a list of all components with a Y position in the current qubit (index i) ordered by x value (ASC)
                    List<CircuitComponent> qubitComponents = components.Where(x => x.getPoint().Y == qubitHeightValues[qubitIndex]).OrderBy(x => x.getPoint().X).ToList();

                    Dictionary<CircuitComponent, int> componentSpacing = new Dictionary<CircuitComponent, int>(); // Store the component and the index of the nearest spaceIndex

                    // Create a dictionary matching a component to an X position index.
                    for (int i = 0; i < spaceIndexes.Count; i++)
                    {
                        double target = spaceIndexes[i];
                        double closestDistance = double.PositiveInfinity;
                        CircuitComponent closest = null;

                        foreach (CircuitComponent qc in qubitComponents) // Calculate the component closest to a point.
                        {
                            double x = qc.getPoint().X;
                            double distance = Math.Abs(target - x);
                            if (distance < closestDistance)
                            {
                                closestDistance = distance;
                                closest = qc;
                            }
                        }
                        if (closest != null) // Add or update the component in the dictionary.
                        {
                            if (componentSpacing.ContainsKey(closest))
                            {
                                double x = closest.getPoint().X;
                                double originalDistance = Math.Abs(spaceIndexes[componentSpacing[closest]] - x);
                                if (Math.Abs(target - x) < originalDistance)
                                {
                                    componentSpacing[closest] = i;
                                }
                            }
                            else
                            {
                                componentSpacing.Add(closest, i);
                            }
                        }

                    }

                    // Update the points for each of the components.
                    for (int i = 0; i < qubitComponents.Count; i++)
                    {
                        if (componentSpacing.ContainsKey(qubitComponents[i]))
                        {
                            Point p = qubitComponents[i].getPoint();
                            p.X = spaceIndexes[componentSpacing[qubitComponents[i]]];
                            qubitComponents[i].setPoint(p);

                            Canvas.SetTop(qubitComponents[i], p.Y);
                            Canvas.SetLeft(qubitComponents[i], p.X);
                        }
                    }



                    //for (int x = 0; x < qubitComponents.Count; x++)
                    //{
                    //    Point p = qubitComponents[x].getPoint();
                    //    p.X = spacing * (x + 1) - (qubitComponents[x].ActualWidth / 2);

                    //    Canvas.SetTop(qubitComponents[x], p.Y);
                    //    Canvas.SetLeft(qubitComponents[x], p.X);
                    //    qubitComponents[x].setPoint(p);
                    //}


                    for (int j = components.Count - 1; j >= 0; j--) // From rear to start, remove all components with the current Y value
                    {
                        if (components[j].getPoint().Y == qubitHeightValues[qubitIndex]) components.RemoveAt(j);
                    }
                    components.AddRange(qubitComponents); // Add the modified components to the list
                }



                Manager.SortComponents(components);
            }

            //MessageBox.Show(Manager.PrintGateLayout());
        }

    }
}



