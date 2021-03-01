using QMat_Calculator.Circuits;
using QMat_Calculator.Drawable;
using QMat_Calculator.Interfaces;
using QMat_Calculator.Matrices;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

/**
* @author Adam Birch - U1761249
*
* @date - 3/1/2021 9:46:36 AM 
*/

namespace QMat_Calculator.Data
{
    static class Saving
    {
        private static List<QubitComponent> qubits;
        private static List<CircuitComponent> components;
        private static List<ControlQubit> controlBits;

        /// <summary>
        /// Perform the main saving functionality.
        /// </summary>
        public static void Save()
        {
            string testDir = "F:\\Users\\Adam\\Desktop\\JsonText.txt";

            qubits = new List<QubitComponent>();
            components = new List<CircuitComponent>();
            controlBits = new List<ControlQubit>();


            foreach (UserControl component in ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.Children)
            {
                if (component == null) continue;

                if (component.GetType() == typeof(CircuitComponent)) { components.Add((CircuitComponent)component); }
                if (component.GetType() == typeof(ControlQubit)) { controlBits.Add((ControlQubit)component); }
                if (component.GetType() == typeof(QubitComponent)) { qubits.Add((QubitComponent)component); }
            }

            string jsonText = CreateJSON();

            WriteFile(testDir, jsonText);

            MessageBox.Show("Saved");
        }

        /// <summary>
        /// Parse the lists into distinct Json entries.
        /// </summary>
        /// <returns></returns>
        private static string CreateJSON()
        {
            StringBuilder s = new StringBuilder();

            s.AppendLine("{\n\"QubitComponents\":[");
            s.AppendLine(QubitComponentJson());
            s.AppendLine("],\n\"CircuitComponents\":[");
            s.AppendLine(CircuitComponentJson());
            s.AppendLine("],\n\"ControlQubits\":[");
            s.AppendLine(ControlQubitJson());
            s.AppendLine("]}\n");

            return s.ToString();
        }

        private static string QubitComponentJson()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("{");

            for (int i = 0; i < qubits.Count; i++)
            {
                s.AppendLine($"\"Qubit{i}\":[{{");

                QubitComponent q = qubits[i];

                s.AppendLine($"\"Qubit\":[{QubitJson(qubits[i].getQubit())}],");
                s.AppendLine($"\n\"Point\":[{PointJson(q.GetPoint())}]");

                s.AppendLine("}]");
                if (i < qubits.Count - 1) { s.Append(","); }
            }

            s.AppendLine("}");
            return s.ToString();
        }
        // TODO:
        private static string CircuitComponentJson()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("{}");
            return s.ToString();
        }
        // TODO:
        private static string ControlQubitJson()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("{}");
            return s.ToString();
        }
        private static string QubitJson(Qubit q)
        {
            StringBuilder s = new StringBuilder();
            List<Gate> gates = q.getGates();
            s.AppendLine("{");

            s.AppendLine($"\n\"Matrix\":[{MatrixJson(q.getMatrix())}]");
            if (gates.Count > 0) s.Append(",");

            for (int i = 0; i < gates.Count; i++)
            {
                s.AppendLine($"\"Gate{i}\":[{GateJson(gates[i])}]");
                if (i < gates.Count - 1) { s.Append(","); }
            }


            s.AppendLine("}");
            return s.ToString();
        }

        // TODO:
        private static string MatrixJson(Matrix m)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("{");
            s.AppendLine($"\n\"Columns\":\"{m.getColumns()}\",");
            s.AppendLine($"\n\"Rows\":\"{m.getRows()}\",");
            s.AppendLine($"\n\"Preceder\":\"{m.getPreceder()}\",");

            s.AppendLine($"\n\"Data\":[");

            for (int r = 0; r < m.getRows(); r++)
            {
                s.AppendLine("{");
                for (int c = 0; c < m.getColumns(); c++)
                {
                    Complex value = m.getData()[r, c];
                    s.AppendLine($"\n\"Cell{r}-{c}\":[{{");
                    s.AppendLine($"\n\"Real\":\"{value.Real}\",");
                    s.AppendLine($"\n\"Imaginary\":\"{value.Imaginary}\"");
                    s.AppendLine("}]");
                }
                s.AppendLine("}");
                if (r < m.getRows() - 1) s.Append(",");
            }



            s.AppendLine("]\n}");
            return s.ToString();
        }

        // TODO:
        private static string GateJson(Gate g)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("{}");
            return s.ToString();
        }

        private static string PointJson(Point p)
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("{");
            s.AppendLine($"\"X\":\"{p.X}\",");
            s.AppendLine($"\"Y\":\"{p.Y}\"");
            s.AppendLine("}");
            return s.ToString();
        }

        /// <summary>
        /// Write the string to the specified file.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="data"></param>
        private static void WriteFile(string path, string data)
        {
            if (File.Exists(path))
            { File.Delete(path); }

            File.WriteAllText(path, data);
            Process.Start("notepad.exe", path);
        }
    }
}
