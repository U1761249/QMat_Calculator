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

namespace QMat_Calculator.Interfaces
{
    /// <summary>
    /// Interaction logic for CompoentRibbon.xaml
    /// </summary>
    public partial class CompoentRibbon : UserControl
    {
        public CompoentRibbon()
        {
            InitializeComponent();
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Image i = (Image)sender;
                Manager.setHeldGate(i);
                DragDrop.DoDragDrop(i, Manager.getHeldGate(), DragDropEffects.Move);
            }
            catch (InvalidCastException exception) { }
        }
    }
}
