using QMat_Calculator.Circuits;
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
* @date - 1/14/2021 3:05:12 PM 
*/

namespace QMat_Calculator.Drawable
{
    /// <summary>
    /// Interaction logic for QubitComponent.xaml
    /// </summary>
    public partial class QubitComponent : UserControl
    {
        private Qubit qubit;
        Point point;
        public QubitComponent(int width, int height, bool value = false)
        {
            InitializeComponent();
            qubit = new Qubit(value);

            point = new Point(width, height);

            UpdateSize();

        }

        private void UpdateSize()
        {
            qubitChannel.X1 = 0;
            qubitChannel.Y1 = point.Y;
            qubitChannel.X2 = point.X;
            qubitChannel.Y2 = point.Y;
        }

        public void AddToManager() { Manager.addQubit(qubit); }

        public Point GetPoint() { return point; }
        public void setPoint(Point p) { point = p; }

        public Qubit getQubit() { return qubit; }

        public void AddGate(Gate gate)
        {
            qubit.addGate(gate);
        }

    }
}
