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

        /// <summary>
        /// Control the display matrix functionality.
        /// </summary>
        /// <param name="m"></param>
        public void DisplaySolution(Matrices.Matrix m)
        {
            dataGrid.Children.Clear();
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());

            DisplayMatrix(m);
            for (int i = 0; i < Manager.GetSolutionSteps().Count; i++)
            {
                SolutionStep step = Manager.GetSolutionSteps()[i];
                //MessageBox.Show(step.ToString());
                DisplayStep(step, i + 1);
            }

        }

        /// <summary>
        /// Display the solution matrix on the screen.
        /// </summary>
        /// <param name="m"></param>
        private void DisplayMatrix(Matrices.Matrix m)
        {
            dataGrid.RowDefinitions.Add(new RowDefinition());
            int row = dataGrid.RowDefinitions.Count - 1;
            int rowCount = 2;
            if (m.getPreceder() != -1) rowCount++;

            Grid grid = NewGrid(dataGrid, row, rowCount);

            Border b = new Border();
            b.BorderThickness = new Thickness(2);
            b.BorderBrush = Brushes.Black;
            Grid.SetRow(b, 0);


            AddLabel($"Solution: ", grid, row, 0);
            if (m.getPreceder() != -1)
            {
                AddLabel(FractionConverter.Convert(m.getPreceder()), grid, row, 1);
                AddLabel(m.ToString(), grid, row, 2);
                Grid.SetColumn(b, 2);
            }
            else
            {
                AddLabel(m.ToString(), grid, row, 1);
                Grid.SetColumn(b, 1);
            }

            grid.Children.Add(b);
        }

        /// <summary>
        /// Show the specified step on the screen.
        /// </summary>
        /// <param name="step"></param>
        /// <param name="stepNumber"></param>
        private void DisplayStep(SolutionStep step, int stepNumber)
        {
            dataGrid.RowDefinitions.Add(new RowDefinition());
            int row = dataGrid.RowDefinitions.Count - 1;
            int rowCount = 7 + +step.getPreceederCount();
            Grid grid = NewGrid(dataGrid, row, rowCount);
            int colOffset = 0;

            AddLabel($"Step {stepNumber}: ", grid, 0, 0);
            AddLabel(step.getEquation(), grid, 0, 1);

            if (step.getInput2().getPreceder() != -1)
            {
                AddLabel(FractionConverter.Convert(step.getInput2().getPreceder()), grid, 0, 2 + colOffset);
                colOffset++;
            }
            AddLabel(step.getInput2().ToString(), grid, 0, 2 + colOffset);
            AddLabel(step.FunctionString(), grid, 0, 3 + colOffset);

            if (step.getInput1().getPreceder() != -1)
            {
                AddLabel(FractionConverter.Convert(step.getInput1().getPreceder()), grid, 0, 4 + colOffset);
                colOffset++;
            }

            AddLabel(step.getInput1().ToString(), grid, 0, 4 + colOffset);
            AddLabel("=", grid, row, 5 + colOffset);

            if (step.getAnswer().getPreceder() != -1)
            {
                AddLabel(FractionConverter.Convert(step.getAnswer().getPreceder()), grid, 0, 6 + colOffset);
                colOffset++;
            }

            AddLabel(step.getAnswer().ToString(), grid, 0, 6 + colOffset);

        }

        /// <summary>
        /// Add a new label to the specified grid location.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="grid"></param>
        /// <param name="row"></param>
        /// <param name="column"></param>
        private void AddLabel(string content, Grid grid, int row, int column)
        {
            Label label = new Label();
            label.Content = content;

            Grid.SetRow(label, row);
            Grid.SetColumn(label, column);

            grid.Children.Add(label);
        }

        private Grid NewGrid(Grid grid, int row, int rowCount = 7)
        {
            Grid g = new Grid();
            for (int i = 0; i < rowCount; i++)
            {
                g.ColumnDefinitions.Add(new ColumnDefinition());
            }

            Grid.SetRow(g, row);
            Grid.SetColumn(g, 0);
            grid.Children.Add(g);

            return g;
        }
    }
}
