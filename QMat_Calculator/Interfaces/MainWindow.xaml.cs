using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Drawable;
using QMat_Calculator.Interfaces;
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
            DataContext = new CustomCommandContext();

            CircuitCanvas cc = new CircuitCanvas();
            Manager.setCircuitCanvas(cc);
            circuitCanvasBorder.Child = cc;

        }



        //Define the functionality for standard Saving and Opening commands.
        private void CommandBindingOpen_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

        private void CommandBindingOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Open");
        }
        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Save");
        }


    }

    /// <summary>
    /// Define getter methods for each custom command.
    /// </summary>
    public class CustomCommandContext
    {
        public ICommand ExitCommand
        {
            get { return new ExitKey(); }
        }
    }

    /// <summary>
    /// Define the function for the Exit key command (Ctrl+X)
    /// </summary>
    public class ExitKey : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Application.Current.Shutdown();
        }
    }
}
