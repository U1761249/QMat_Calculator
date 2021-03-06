﻿using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Data;
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

/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 1:30:12 PM 
*/

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
            MatrixCanvas mc = new MatrixCanvas();
            Manager.setMatrixCanvas(mc);
            matrixCanvasBorder.Child = mc;
        }




        //Define the functionality for standard Saving and Opening commands.
        private void CommandBindingOpen_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

        private void CommandBindingOpen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Loading.Load();
        }
        private void CommandBindingSave_CanExecute(object sender, CanExecuteRoutedEventArgs e) { e.CanExecute = true; }

        private void CommandBindingSave_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Saving.Save();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedIndex == 1)
                Manager.Solve();
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
        public ICommand AddQubitCommand
        {
            get { return new AddQubitKey(); }
        }

        public ICommand RemoveLastQubitCommand
        {
            get { return new RemoveLastQubitKey(); }
        }

        public ICommand RemoveGateCommand
        {
            get { return new RemoveGateKey(); }
        }

        public ICommand SolveCommand
        {
            get { return new SolveKey(); }
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

    public class AddQubitKey : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Manager.addQubit();
        }
    }

    public class RemoveLastQubitKey : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Manager.removeQubit();
        }
    }

    public class RemoveGateKey : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Manager.removeGate();
        }
    }

    public class SolveKey : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Manager.Solve();

        }
    }

}
