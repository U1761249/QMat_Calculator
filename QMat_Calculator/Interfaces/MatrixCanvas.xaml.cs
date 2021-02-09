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

        public void DisplayMatrix(Matrix m)
        {
            dataGrid.Children.Clear();
            dataGrid.ColumnDefinitions.Add(new ColumnDefinition());
            Complex[,] data = m.getData();

            for (int i = 0; i < m.getRows(); i++) { dataGrid.RowDefinitions.Add(new RowDefinition()); }
            for (int i = 0; i < m.getColumns(); i++) { dataGrid.ColumnDefinitions.Add(new ColumnDefinition()); }

            for (int c = 0; c < m.getColumns(); c++)
            {
                for (int r = 0; r < m.getRows(); r++)
                {
                    Label l = new Label();
                    l.Content = data[r, c].ToString();

                    Grid.SetRow(l, r);
                    Grid.SetColumn(l, c + 1);

                    dataGrid.Children.Add(l);
                }
            }

            string preceeder = FractionConverter.Convert(Convert.ToDouble(m.GetPreceder()));
            if (!String.IsNullOrWhiteSpace(preceeder))
            {
                //if (preceeder.Contains('/')) { }
                int midRow = Convert.ToInt32(Math.Round(Convert.ToDouble(m.getRows() / 2)));

                Label l = new Label();
                l.Content = preceeder;

                Grid.SetRow(l, midRow);
                Grid.SetColumn(l, 0);
            }
        }
    }
}
