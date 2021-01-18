using QMat_Calculator.Circuits;
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

            //// Tests for the values contained within the gates.
            // CNOT.Content = new CNOT(new Qubit(), new Qubit()).ToString();
            // Hadamard.Content = new Hadamard(new Qubit()).ToString();
            // PauliX.Content = new Pauli(new Qubit(), Pauli.PauliType.X).ToString();
            // PauliY.Content = new Pauli(new Qubit(), Pauli.PauliType.Y).ToString();
            // PauliZ.Content = new Pauli(new Qubit(), Pauli.PauliType.Z).ToString();
            // SqrtNOT.Content = new SqrtNOT(new Qubit()).ToString();
            // Toffoli.Content = new Toffoli(new Qubit(), new Qubit(), new Qubit()).ToString();

            Qubit bit1 = new Qubit(true);
            Qubit bit2 = new Qubit(false);

            Hadamard h = new Hadamard(bit1);
            Hadamard h2 = new Hadamard(bit1);
            Hadamard h3 = new Hadamard(bit2);

            Matrix m = Matrix.Tensor(h, h3);
            Matrix m1 = Matrix.Tensor(bit1, bit2);
            Matrix m2 = Matrix.Multiply(m, m1);
            Matrix m3 = Matrix.Multiply(h2, m2);

            CNOT.Content = m3.ToString(true);
            Hadamard.Content = Matrix.Tensor(bit1, bit2).ToString();

        }
    }
}
