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


            //string preceeder = FractionConverter.Convert(Convert.ToDouble(m.GetPreceder()));
            //if (!String.IsNullOrWhiteSpace(preceeder))
            //{
            //    //if (preceeder.Contains('/')) { }
            //    int midRow = Convert.ToInt32(Math.Round(Convert.ToDouble(m.getRows() / 2)));

            //    Label l = new Label();
            //    l.Content = preceeder;

            //    Grid.SetRow(l, midRow);
            //    Grid.SetColumn(l, 0);
            //}
        }

        private void DisplayMatrix(Matrices.Matrix m)
        {
            dataGrid.RowDefinitions.Add(new RowDefinition());

            Label l = new Label();
            l.Content = ($"Solution: ");

            Label s = new Label();
            s.Content = m.ToString();

            Grid.SetRow(l, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(l, 0);

            Grid.SetRow(s, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(s, 1);

            dataGrid.Children.Add(l);
            dataGrid.Children.Add(s);

            Border b = new Border();
            b.BorderThickness = new Thickness(2);
            b.BorderBrush = Brushes.Black;

            Grid.SetRow(b, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(b, 1);

            dataGrid.Children.Add(b);


            //Complex[,] data = m.getData();

            //for (int i = 0; i < m.getRows(); i++) { dataGrid.RowDefinitions.Add(new RowDefinition()); }
            //for (int i = 0; i < m.getColumns(); i++) { dataGrid.ColumnDefinitions.Add(new ColumnDefinition()); }

            //for (int c = 0; c < m.getColumns(); c++)
            //{
            //    for (int r = 0; r < m.getRows(); r++)
            //    {
            //        Label l = new Label();
            //        Complex value = data[r, c];

            //        StringBuilder sb = new StringBuilder();
            //        sb.Append(String.Format("{0, -2}", value.Real));

            //        if (value.Real == 0 && value.Imaginary != 0)
            //        {
            //            sb.Clear();
            //        }
            //        if (value.Imaginary != 0)
            //        {
            //            if (value.Imaginary == 1) { sb.Append(String.Format("{0, -2}", "+i")); }
            //            else if (value.Imaginary == -1) { sb.Append(String.Format("{0, -2}", "-i")); }
            //            else
            //                sb.Append(String.Format("{0, -2}", value.Imaginary + "i"));
            //        }
            //        sb.Append(String.Format("{0, -2}", " "));

            //        l.Content = sb.ToString();

            //        Grid.SetRow(l, r);
            //        Grid.SetColumn(l, c + 1);

            //        dataGrid.Children.Add(l);
            //    }
            //}
        }

        private void DisplayStep(SolutionStep step, int stepNumber)
        {
            dataGrid.RowDefinitions.Add(new RowDefinition());

            Label l = new Label();
            l.Content = ($"Step {stepNumber}: ");

            Label e = new Label();
            e.Content = step.getEquation();

            Label i1 = new Label();
            i1.Content = step.getInput1().ToString();

            Label f = new Label();
            f.Content = step.FunctionString();

            Label i2 = new Label();
            i2.Content = step.getInput2().ToString();

            Label eq = new Label();
            eq.Content = "=";

            Label s = new Label();
            s.Content = step.getAnswer().ToString();

            Grid.SetRow(l, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(l, 0);

            Grid.SetRow(e, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(e, 1);

            Grid.SetRow(i1, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(i1, 4);

            Grid.SetRow(f, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(f, 3);

            Grid.SetRow(i2, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(i2, 2);

            Grid.SetRow(eq, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(eq, 5);

            Grid.SetRow(s, dataGrid.RowDefinitions.Count - 1);
            Grid.SetColumn(s, 6);

            dataGrid.Children.Add(l);
            dataGrid.Children.Add(e);
            dataGrid.Children.Add(i1);
            dataGrid.Children.Add(f);
            dataGrid.Children.Add(i2);
            dataGrid.Children.Add(eq);
            dataGrid.Children.Add(s);
        }

    }
}
