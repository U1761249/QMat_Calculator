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

            var position = e.GetPosition(sender as IInputElement);
            Canvas.SetTop(ccDrag, position.Y - offset.Y);
            Canvas.SetLeft(ccDrag, position.X - offset.X);

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

                // Update the Qubits with the new association.
                if (chosen != null)
                {
                    Gate held = ((CircuitComponent)Manager.getCCDrag()).getGate();
                    if (!(chosen.getQubit().hasGate(held))) // If the chosen qubit doesn't contain the held gate.
                    {
                        Manager.Decouple(held);
                    }

                    chosen.AddGate(held);

                    // Set the height of the gate based on the qubit it is associated with.
                    CircuitComponent cc = (CircuitComponent)Manager.getCCDrag();
                    Point position = chosen.GetPoint();


                    Canvas.SetTop(cc, position.Y - (cc.ActualHeight / 2));
                    Point circuitPoint = cc.getPoint();
                    circuitPoint.Y = position.Y - (cc.ActualHeight / 2);
                    cc.setPoint(circuitPoint);

                    cc.setGate(held);
                    Manager.setCCDrag(cc);
                }

                Manager.setCCDrag(null);
                this.MainCircuitCanvas.ReleaseMouseCapture();

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
            MoveGates();

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

            ResizeQubits();
            //MessageBox.Show($"Width: {width}");
            //MessageBox.Show($"Height: {height}");
        }

        /// <summary>
        /// Change the size of the Qubits when the screen size changes.
        /// </summary>
        private void ResizeQubits()
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
        /// Move the gates to the correct places when the screen size or number of Qubits changes.
        /// </summary>
        private void MoveGates()
        {
            Point p = new Point(MainCircuitCanvas.ActualWidth, MainCircuitCanvas.ActualHeight);

            for (int i = 0; i < MainCircuitCanvas.Children.Count; i++)
            {
                if (MainCircuitCanvas.Children[i].GetType() == typeof(CircuitComponent))
                {
                    ((CircuitComponent)MainCircuitCanvas.Children[i]).Move(p);
                }
            }

        }

        /// <summary>
        /// Move the gates on the screen and within the Qubit's list so that they are in the correct order.
        /// </summary>
        private void OrderComponents()
        {
            double spacing = 0;
            int mostPopulated = 0;
            // Find how many gates are in the most populated Qubit.
            foreach (Qubit q in Manager.getQubits())
            {
                int population = q.getGates().Count();
                if (population > mostPopulated) mostPopulated = population;
            }
            if (mostPopulated <= 0) { return; } // Exit if the population is 0.

            spacing = MainCircuitCanvas.ActualWidth / (mostPopulated + 1);

            ComponentSorter components = new ComponentSorter(); // Create a 2D list of components

            // Insert the CircuitComponetns into the components list based on it's Point using an insert sort.
            for (int i = 0; i < MainCircuitCanvas.Children.Count; i++)
            {
                if (MainCircuitCanvas.Children[i].GetType() == typeof(CircuitComponent))
                {
                    components.addComponent((CircuitComponent)MainCircuitCanvas.Children[i]);
                }
            }
            //MessageBox.Show(components.ToString());
        }

        private class ComponentSorter
        {
            int rows;
            List<List<CircuitComponent>> components;
            public ComponentSorter()
            {
                components = new List<List<CircuitComponent>>();
                rows = 0;
            }

            /// <summary>
            /// Add the given component into the 2D list based on its point.
            /// </summary>
            /// <param name="c"></param>
            public void addComponent(CircuitComponent c)
            {
                Point p = c.getPoint();
                bool added = false;
                for (int i = 0; i < rows; i++)
                {
                    if (components[i][0].getPoint().Y == p.Y) // If the current row is where the component should be
                    {
                        for (int j = 0; j < components[i].Count; j++)
                        {
                            if (components[i][j].getPoint().X > p.X)
                            {
                                components[i].Insert(j, c);
                                added = true;
                            }
                        }
                        if (!added)
                        {
                            components[i].Add(c);
                            added = true;
                        }
                    }
                    else //if (components[i][0].getPoint().Y < p.Y)
                    {
                        addRow(i);
                        components[i].Add(c);
                        added = true;
                    }

                    if (added) break; // Exit the loop if it has been added.
                }
                if (!added)
                {
                    addRow();
                    components[rows - 1].Add(c);
                }
            }

            /// <summary>
            /// Add a new row (Qubit) at the specified index
            /// </summary>
            /// <param name="index"></param>
            private void addRow(int index = -1)
            {
                if (index == -1) components.Add(new List<CircuitComponent>());
                else components.Insert(index, new List<CircuitComponent>());

                rows += 1;
            }

            public String ToString()
            {
                StringBuilder s = new StringBuilder();

                foreach (List<CircuitComponent> row in components)
                {

                    foreach (CircuitComponent c in row)
                    {
                        Point p = c.getPoint();
                        string point = $"({p.X}, {p.Y})";
                        string value = $"{c.getLabelText()}-{point} ";
                        s.Append(value);
                    }
                    s.Append(Environment.NewLine);
                }

                return s.ToString();
            }
        }
    }


}
