using QMat_Calculator.Drawable;
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
    /// Interaction logic for CircuitCanvas.xaml
    /// </summary>
    public partial class CircuitCanvas : UserControl
    {
        public CircuitCanvas()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Drop the held component to the screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CircuitCanvas_Drop(object sender, DragEventArgs e)
        {
            //MessageBox.Show($"Dropped Gate of type {Manager.getHeldGate().GetType()}");
            CircuitComponent cc = new CircuitComponent();
            cc.setType(Manager.getHeldGate());
            MainCircuitCanvas.Children.Add(cc);

        }

        private void CircuitCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            UIElement ccDrag = Manager.getCCDrag();
            Point offset = Manager.getOffsetDrag();

            if (ccDrag == null) return;

            var position = e.GetPosition(sender as IInputElement);
            Canvas.SetTop(ccDrag, position.Y - offset.Y);
            Canvas.SetLeft(ccDrag, position.X - offset.X);

            Manager.setCCDrag(ccDrag);
        }

        private void CircuitCanvas_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Manager.setCCDrag(null);
            this.MainCircuitCanvas.ReleaseMouseCapture();
        }
    }
}
