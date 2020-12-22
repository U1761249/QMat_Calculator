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

            // CNOT.Content = new CNOT(new Qubit(), new Qubit()).ToString();
            // Hadamard.Content = new Hadamard(new Qubit()).ToString();
            // PauliX.Content = new Pauli(new Qubit(), Pauli.PauliType.X).ToString();
            // PauliY.Content = new Pauli(new Qubit(), Pauli.PauliType.Y).ToString();
            // PauliZ.Content = new Pauli(new Qubit(), Pauli.PauliType.Z).ToString();
            // SqrtNOT.Content = new SqrtNOT(new Qubit()).ToString();
            // Toffoli.Content = new Toffoli(new Qubit(), new Qubit(), new Qubit()).ToString();

            Pauli pauli = new Pauli(new Qubit(), Pauli.PauliType.Y);
            Hadamard h = new Hadamard(new Qubit());
            Matrix m = Matrix.Tensor(pauli.getMatrix(), h.getMatrix());
            CNOT.Content = pauli.ToString();
            Hadamard.Content = h.ToString();
            PauliX.Content = m.ToString();

        }
    }
}
