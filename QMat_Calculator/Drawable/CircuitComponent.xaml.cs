using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
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

namespace QMat_Calculator.Drawable
{
    /// <summary>
    /// Interaction logic for CircuitComponent.xaml
    /// </summary>
    public partial class CircuitComponent : UserControl
    {
        private Gate gate = null;
        public CircuitComponent()
        {
            InitializeComponent();
        }

        public void setType(Object o)
        {
            if (o.GetType().BaseType == typeof(Gate))
            {
                string label = "";

                gate = ((Gate)o);
                if (gate.GetType() == typeof(Hadamard)) { label = ((Hadamard)gate).GetGateLabel(); }
                else if (gate.GetType() == typeof(Pauli)) { label = ((Pauli)gate).GetGateLabel(); }
                else if (gate.GetType() == typeof(CNOT)) { label = ((CNOT)gate).GetGateLabel(); }
                else if (gate.GetType() == typeof(SqrtNOT)) { label = ((SqrtNOT)gate).GetGateLabel(); }
                //else if (gate.GetType() == typeof(Deutsch)) { label = ((Deutsch)gate).GetGateLabel(); }
                else if (gate.GetType() == typeof(Toffoli)) { label = ((Toffoli)gate).GetGateLabel(); }


                if (!String.IsNullOrWhiteSpace(label)) { componentLabel.Text = label; }


            }
        }
    }
}
