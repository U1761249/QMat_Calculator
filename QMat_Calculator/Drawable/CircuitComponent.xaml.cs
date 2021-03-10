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
        private string source = null;
        private Gate gate = null;
        private Point point;
        List<ControlQubit> controlQubits = new List<ControlQubit>();

        public string getMatrix
        {
            get { if (gate == null) return ""; return gate.getMatrix().ToString(true); }
        } // Create a value to bind in Xaml
        public CircuitComponent(Point p)
        {
            InitializeComponent();
            this.component.Height = 100;
            this.component.Width = 100;

            this.point = p;
            this.DataContext = this;

            Canvas.SetTop(this, p.Y - (this.ActualHeight / 2));
            Canvas.SetLeft(this, p.X - (this.ActualWidth / 2));
        }
        public CircuitComponent() { }
        public CircuitComponent(Gate gate, string imageSource, Point p)
        {
            InitializeComponent();
            this.component.Height = 100;
            this.component.Width = 100;

            this.point = p;
            this.gate = gate;
            this.source = imageSource;
            this.DataContext = this;

            Canvas.SetTop(this, p.Y - (this.ActualHeight / 2));
            Canvas.SetLeft(this, p.X - (this.ActualWidth / 2));

            this.setType(gate);
        }
        public string getImagePath() { return source; }
        public void setImage(string s) { source = s; }
        public ref Gate getGate() { return ref gate; }
        public void setGate(Gate g) { gate = g; }
        public Point getPoint() { return point; }
        public void setPoint(Point p)
        {
            point = p;
            // Update all controlQubits to have the same X coordinate.
            foreach (ControlQubit cq in controlQubits)
            {
                cq.setPoint(
                    new Point(p.X, cq.getPoint().Y));
            }
        }
        public List<ControlQubit> getControlQubits() { return controlQubits; }
        public void setControlQubits(List<ControlQubit> q) { controlQubits = q; }
        public void addControlQubit(ControlQubit q)
        {
            if (controlQubits == null)
                controlQubits = new List<ControlQubit>();
            controlQubits.Add(q);
        }


        /// <summary>
        /// Change the component based on the type of gate it contains.
        /// </summary>
        /// <param name="o"></param>
        public void setType(Object o)
        {
            if (o.GetType().BaseType == typeof(Gate))
            {
                gate = ((Gate)o);
                //string label = "";
                //

                //if (gate.GetType() == typeof(Hadamard)) { label = ((Hadamard)gate).GetGateLabel(); }
                //else if (gate.GetType() == typeof(Pauli)) { label = ((Pauli)gate).GetGateLabel(); }
                //else if (gate.GetType() == typeof(CNOT)) { label = ((CNOT)gate).GetGateLabel(); }
                //else if (gate.GetType() == typeof(SqrtNOT)) { label = ((SqrtNOT)gate).GetGateLabel(); }
                ////else if (gate.GetType() == typeof(Deutsch)) { label = ((Deutsch)gate).GetGateLabel(); }
                //else if (gate.GetType() == typeof(Toffoli)) { label = ((Toffoli)gate).GetGateLabel(); }
                //
                //
                //if (!String.IsNullOrWhiteSpace(label))
                //{
                //    componentLabel.Text = label;
                //    if (label.Length > 1)
                //    {
                //        componentLabel.FontSize = 30;
                //    }
                //}

                if (String.IsNullOrWhiteSpace(source))
                {

                    if (gate.GetType() == typeof(Hadamard)) { source = "/Icons/Hadamard(New).png"; }
                    else if (gate.GetType() == typeof(Pauli))
                    {
                        switch (((Pauli)gate).GetPauliType())
                        {
                            case (Pauli.PauliType.X): source = "/Icons/PauliX.png"; break;
                            case (Pauli.PauliType.Y): source = "/Icons/PauliY.png"; break;
                            case (Pauli.PauliType.Z): source = "/Icons/PauliZ.png"; break;
                        }
                    }
                    else if (gate.GetType() == typeof(CNOT)) { source = "/Icons/Toffoli-CNOT.png"; }
                    else if (gate.GetType() == typeof(SqrtNOT)) { source = "/Icons/RootNOT.png"; }
                    else if (gate.GetType() == typeof(Toffoli)) { source = "/Icons/Toffoli-CNOT.png"; }
                }

                componentImage.Source = new BitmapImage(new Uri(source, UriKind.Relative));
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

            Canvas.SetTop(this, p.Y - (this.ActualHeight / 2));
            Canvas.SetLeft(this, p.X - (this.ActualWidth / 2));

            this.point = p;
        }

        //public string getLabelText() { return componentLabel.Text; }

    }
}
