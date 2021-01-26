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

namespace QMat_Calculator.Drawable
{
    /// <summary>
    /// Interaction logic for QubitComponent.xaml
    /// </summary>
    public partial class QubitComponent : UserControl
    {
        private Qubit qubit;
        Point start;
        Point end;
        public QubitComponent(int width, int height, bool value = false)
        {
            InitializeComponent();
            qubit = new Qubit(value);

            start = new Point(0, height);
            end = new Point(width, height);

            UpdateSize();

        }

        private void UpdateSize()
        {
            qubitChannel.X1 = start.X;
            qubitChannel.Y1 = start.Y;
            qubitChannel.X2 = end.X;
            qubitChannel.Y2 = end.Y;
        }

        public void AddToManager() { Manager.addQubit(qubit); }
    }
}
