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

            Point position = e.GetPosition(sender as IInputElement);
            position.X -= offset.X;
            position.Y -= offset.Y;

            Canvas.SetTop(ccDrag, position.Y);
            Canvas.SetLeft(ccDrag, position.X);
            ((CircuitComponent)ccDrag).setPoint(position);

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
                    Gate held = ((CircuitComponent)Manager.getCCDrag()).getGate();  // Get the gate
                    Manager.Decouple(held);                                         // Remove the gate from any qubits
                    chosen.AddGate(held);                                           // Add the gate to the chosen qubit

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
        /// Move the gates on the screen and within the Qubit's list so that they are in the correct order.
        /// </summary>
        private void OrderComponents()
        {

            List<CircuitComponent> components = new List<CircuitComponent>();

            for (int i = 0; i < MainCircuitCanvas.Children.Count; i++)
            {
                if (MainCircuitCanvas.Children[i].GetType() == typeof(CircuitComponent))
                {
                    components.Add((CircuitComponent)MainCircuitCanvas.Children[i]);
                }
            }

            if (components.Count > 0)
            {
                // Get a list of all Qubit Heights
                List<double> qubitHeightValues = components.GroupBy(x => x.getPoint().Y).Select(x => x.First().getPoint().Y).ToList();
                int mostPopulated = 0;
                foreach (Qubit q in Manager.getQubits())
                {
                    if (q.getGates().Count > mostPopulated) mostPopulated = q.getGates().Count;
                }
                double spacing = MainCircuitCanvas.ActualWidth / (mostPopulated + 1);
                for (int i = 0; i < qubitHeightValues.Count; i++)
                {
                    // Get a list of all components with a Y position in the current qubit (index i) ordered by x value (ASC)
                    List<CircuitComponent> qubitComponents = components.Where(x => x.getPoint().Y == qubitHeightValues[i]).OrderBy(x => x.getPoint().X).ToList();
                    for (int x = 0; x < qubitComponents.Count; x++)
                    {
                        Point p = qubitComponents[x].getPoint();
                        p.X = spacing * (x + 1) - (qubitComponents[x].ActualWidth / 2);

                        Canvas.SetTop(qubitComponents[x], p.Y);
                        Canvas.SetLeft(qubitComponents[x], p.X);
                        qubitComponents[x].setPoint(p);
                    }
                    for (int j = components.Count - 1; j >= 0; j--) // From rear to start, remove all components with the current Y value
                    {
                        if (components[j].getPoint().Y == qubitHeightValues[i]) components.RemoveAt(j);
                    }
                    components.AddRange(qubitComponents); // Add the modified components to the list
                }



                Manager.SortComponents(components);
            }

            //MessageBox.Show(Manager.PrintGateLayout());
        }

    }
}



