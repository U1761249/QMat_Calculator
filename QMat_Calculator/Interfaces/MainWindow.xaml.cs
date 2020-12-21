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

            Circuits.Gates.Hadamard h = new Circuits.Gates.Hadamard(new Circuits.Qubit());

            Matrices.Matrix matrix = Matrices.Matrix.Multiply(h.matrix, 2);
            h.matrix = matrix;

            Console.WriteLine("TEST");
            h.printMatrix();
            label.Content = h.ToString();
        }
    }
}
