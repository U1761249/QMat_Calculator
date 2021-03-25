using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
* @date - 12/18/2020 12:12:07 PM 
*/

namespace QMat_Calculator.Interfaces
{
    /// <summary>
    /// Interaction logic for MatrixCanvas.xaml
    /// </summary>
    public partial class MatrixCanvas : UserControl
    {
        public MatrixCanvas()
        {
            InitializeComponent();
        }

        public void DisplaySolution(Matrices.Matrix m)
        {
            dataGrid.Children.Clear();
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());

            DisplayMatrix(m);
            for (int i = 0; i < Manager.GetSolutionSteps().Count; i++)
            {
                SolutionStep step = Manager.GetSolutionSteps()[i];
                //MessageBox.Show(step.ToString());
                DisplayStep(step, i + 1);
            }

        }

        private void DisplayMatrix(Matrices.Matrix m)
        {
            dataGrid.RowDefinitions.Add(new RowDefinition());

            int row = dataGrid.RowDefinitions.Count - 1;

            AddLabel($"Solution: ", dataGrid, row, 0);
            AddLabel(m.ToString(), dataGrid, row, 1);


            Border b = new Border();
            b.BorderThickness = new Thickness(2);
            b.BorderBrush = Brushes.Black;

            Grid.SetRow(b, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(b, 1);

            dataGrid.Children.Add(b);

        }

        private void DisplayStep(SolutionStep step, int stepNumber)
        {
            dataGrid.RowDefinitions.Add(new RowDefinition());
            int row = dataGrid.RowDefinitions.Count - 1;

            AddLabel($"Step {stepNumber}: ", dataGrid, row, 0);
            AddLabel(step.getEquation(), dataGrid, row, 1);
            AddLabel(step.getInput2().ToString(), dataGrid, row, 2);
            AddLabel(step.FunctionString(), dataGrid, row, 3);
            AddLabel(step.getInput1().ToString(), dataGrid, row, 4);
            AddLabel("=", dataGrid, row, 5);
            AddLabel(step.getAnswer().ToString(), dataGrid, row, 6);

        }

        private void AddLabel(string content, Grid grid, int row, int column)
        {
            Label label = new Label();
            label.Content = content;

            Grid.SetRow(label, row);
            Grid.SetColumn(label, column);

            grid.Children.Add(label);
        }

    }
}
