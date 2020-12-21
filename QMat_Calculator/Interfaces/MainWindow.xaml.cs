using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Matrices;

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
using Matrix = QMat_Calculator.Matrices.Matrix;

namespace QMat_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Hadamard h = new Hadamard(new Circuits.Qubit());

            Matrix matrix = Matrix.Tensor(h.getMatrix(), h.getMatrix());
            h.setMatrix(matrix);

            Console.WriteLine("TEST");
            h.printMatrix();
            label.Content = h.ToString();
        }
    }
}
