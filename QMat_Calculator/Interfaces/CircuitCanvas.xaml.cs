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

namespace QMat_Calculator.Interfaces
{
    /// <summary>
    /// Interaction logic for CircuitCanvas.xaml
    /// </summary>
    public partial class CircuitCanvas : UserControl
    {
        double actualWidth = 0;
        double actualHeight = 0;

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
            Manager.setCCDrag(null);
            this.MainCircuitCanvas.ReleaseMouseCapture();
        }
        private void MainCircuitCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeQubits();
            MoveGates();

            actualHeight = MainCircuitCanvas.ActualHeight;
            actualWidth = MainCircuitCanvas.ActualWidth;
        }

        private void AddQubit()
        {
            int width = Convert.ToInt32(MainCircuitCanvas.ActualWidth);
            int height = Convert.ToInt32(MainCircuitCanvas.ActualHeight / 2);

            QubitComponent q = new QubitComponent(width, height);
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
                        double spacing = (height / count) / 2;
                        height = Convert.ToInt32(spacing * qubitIndex);

                        Point start = new Point(0, height);
                        Point end = new Point(width, height);

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
    }
}
