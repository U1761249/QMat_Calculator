using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Interfaces;
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
* @date - 1/24/2021 12:23:11 AM
*/

namespace QMat_Calculator.Drawable
{
    /// <summary>
    /// Interaction logic for CircuitComponent.xaml
    /// </summary>
    public partial class CircuitComponent : UserControl
    {
        private Gate gate = null;
        private Point point;
        public CircuitComponent(Point p)
        {
            InitializeComponent();

            Canvas.SetTop(this, p.Y - (this.ActualHeight / 2));
            Canvas.SetLeft(this, p.X - (this.ActualWidth / 2));
            this.point = p;
        }
        public ref Gate getGate() { return ref gate; }
        public void setGate(Gate g) { gate = g; }
        public Point getPoint() { return point; }
        public void setPoint(Point p) { point = p; }

        /// <summary>
        /// Change the component based on the type of gate it contains.
        /// </summary>
        /// <param name="o"></param>
        public void setType(Object o)
        {
            if (o.GetType().BaseType == typeof(Gate))
            {
                string label = "";

                gate = ((Gate)o);
                if (gate.GetType() == typeof(Hadamard)) { label = ((Hadamard)gate).GetGateLabel(); }
                else if (gate.GetType() == typeof(Pauli)) { label = ((Pauli)gate).GetGateLabel(); }
                else if (gate.GetType() == typeof(CNOT)) { label = ((CNOT)gate).GetGateLabel(); }
                else if (gate.GetType() == typeof(SqrtNOT)) { label = ((SqrtNOT)gate).GetGateLabel(); }
                //else if (gate.GetType() == typeof(Deutsch)) { label = ((Deutsch)gate).GetGateLabel(); }
                else if (gate.GetType() == typeof(Toffoli)) { label = ((Toffoli)gate).GetGateLabel(); }


                if (!String.IsNullOrWhiteSpace(label))
                {
                    componentLabel.Text = label;
                    if (label.Length > 1)
                    {
                        componentLabel.FontSize = 30;
                    }
                }

            }
        }

        /// <summary>
        /// Set this object as the drag component for moving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void component_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CircuitCanvas circuitCanvas = Manager.getCircuitCanvas();
            Manager.setSelectedGate(this);
            Manager.setCCDrag(this);
            Point offset = e.GetPosition(circuitCanvas.MainCircuitCanvas);
            offset.Y -= Canvas.GetTop(this);
            offset.X -= Canvas.GetLeft(this);
            Manager.setOffsetDrag(offset);
            circuitCanvas.MainCircuitCanvas.CaptureMouse();
        }

        /// <summary>
        /// Move this to the specified point.
        /// </summary>
        /// <param name="p"></param>
        public void Move(Point p)
        {
            Canvas.SetTop(this, 0);
            Canvas.SetLeft(this, 0);

            Canvas.SetTop(this, (p.Y / 2) - (this.ActualHeight / 2));
            Canvas.SetLeft(this, (p.X / 2) - (this.ActualHeight / 2));

            this.point = p;
        }

        public string getLabelText() { return componentLabel.Text; }

    }
}
