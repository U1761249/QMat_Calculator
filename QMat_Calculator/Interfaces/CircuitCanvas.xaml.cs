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

        private void CircuitCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (Manager.getCCDrag() != null)
            {
                List<QubitComponent> qubits = new List<QubitComponent>();
                foreach (var line in MainCircuitCanvas.Children)
                {
                    if (line.GetType() == typeof(QubitComponent))
                    {
                        qubits.Add(((QubitComponent)line));
                    }
                }

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

                if (chosen != null)
                {
                    Gate held = ((CircuitComponent)Manager.getCCDrag()).getGate();
                    if (!(chosen.getQubit().hasGate(held))) // If the chosen qubit doesn't contain the held gate
                    {
                        Manager.Decouple(held);
                    }

                    chosen.AddGate(held);

                    CircuitComponent cc = (CircuitComponent)Manager.getCCDrag();
                    Point position = chosen.GetPoint();

                    Canvas.SetTop(cc, position.Y - (cc.ActualHeight / 2));


                    cc.setGate(held);
                    Manager.setCCDrag(cc);
                }

                Manager.setCCDrag(null);
                this.MainCircuitCanvas.ReleaseMouseCapture();
                OrderComponents();
            }
        }
        private void MainCircuitCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeQubits();
            MoveGates();

        }

        public void AddQubit(bool value = false)
        {
            QubitComponent q = new QubitComponent(0, 0, value);
            MainCircuitCanvas.Children.Add(q);
            q.AddToManager();

            ResizeQubits();
            //MessageBox.Show($"Width: {width}");
            //MessageBox.Show($"Height: {height}");
        }

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

                        Point start = new Point(0, height);
                        Point end = new Point(width, height);

                        ((QubitComponent)MainCircuitCanvas.Children[i]).setPoint(end);

                        ((QubitComponent)MainCircuitCanvas.Children[i]).qubitChannel.X1 = start.X;
                        ((QubitComponent)MainCircuitCanvas.Children[i]).qubitChannel.Y1 = start.Y;
                        ((QubitComponent)MainCircuitCanvas.Children[i]).qubitChannel.X2 = end.X;
                        ((QubitComponent)MainCircuitCanvas.Children[i]).qubitChannel.Y2 = end.Y;
                    }
                }
            }
        }


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

        private void OrderComponents()
        {

        }
    }
}
