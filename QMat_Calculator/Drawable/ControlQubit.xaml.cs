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
* @date - 2/15/2020 1:12:36 PM 
*/

namespace QMat_Calculator.Drawable
{
    /// <summary>
    /// Interaction logic for ControlQubit.xaml
    /// </summary>
    public partial class ControlQubit : UserControl
    {
        private ControlBit control;
        private Point point;

        public void setControlBit(ControlBit b) { control = b; }
        public ControlBit getControlBit() { return control; }
        public Point getPoint() { return point; }
        public void setPoint(Point p) { point = p; }
        public ControlQubit(Point p)
        {
            InitializeComponent();
            Canvas.SetTop(this, p.Y - (this.ActualHeight / 2));
            Canvas.SetLeft(this, p.X - (this.ActualWidth / 2));
            this.point = p;
            this.control = new ControlBit();
        }

        private void Dot_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            CircuitCanvas circuitCanvas = Manager.getCircuitCanvas();
            Manager.setSelectedControl(this);
            Manager.setCCDrag(this);
            Point offset = e.GetPosition(circuitCanvas.MainCircuitCanvas);
            offset.Y -= Canvas.GetTop(this);
            offset.X -= Canvas.GetLeft(this);
            Manager.setOffsetDrag(offset);
            circuitCanvas.MainCircuitCanvas.CaptureMouse();
        }

        public void Move(Point p)
        {
            Canvas.SetTop(this, 0);
            Canvas.SetLeft(this, 0);

            Canvas.SetTop(this, p.Y - (this.ActualHeight / 2));
            Canvas.SetLeft(this, p.X - (this.ActualWidth / 2));
        }


    }
}
