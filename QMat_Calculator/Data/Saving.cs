﻿using Newtonsoft.Json;
using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Drawable;
using QMat_Calculator.Interfaces;
using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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

        /// <summary>
        /// Perform the main saving functionality.
        /// </summary>
        public static void Save()
        {
            //string directory = "F:\\Users\\Adam\\Desktop\\JsonText.txt";

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();

            sfd.FileName = $"Project";
            sfd.Filter = "JSON File (.json)|*.json|All(*.*)|*";
            sfd.DefaultExt = ".json"; // Default file extension

            bool result = (bool)sfd.ShowDialog();
            if (!result) return;

            string directory = sfd.FileName;
            qubits = new List<QubitComponent>();
            components = new List<CircuitComponent>();


            foreach (UserControl component in ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.Children)
            {
                if (component == null) continue;

                if (component.GetType() == typeof(CircuitComponent)) { components.Add((CircuitComponent)component); }
                if (component.GetType() == typeof(QubitComponent)) { qubits.Add((QubitComponent)component); }
            }

            //string jsonText = JsonConvert.SerializeObject(components, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            //WriteFile(directory, jsonText.ToString());

            string jsonText = CreateJSON();
            WriteFile(directory, jsonText);

        }

        /// <summary>
        /// Parse the lists into distinct Json entries.
        /// </summary>
        /// <returns></returns>
        private static string CreateJSON()
        {
            StringBuilder s = new StringBuilder();

            s.AppendLine("{\n\"Canvas\":");
            s.AppendLine(CanvasJson());
            s.AppendLine(",\n\"QubitComponents\":");
            s.AppendLine(QubitComponentJson());
            s.AppendLine(",\n\"CircuitComponents\":");
            s.AppendLine(CircuitComponentJson());
            s.AppendLine("}\n");

            return s.ToString();
        }

        private static string CanvasJson()
        {
            CircuitCanvas cc = Manager.getCircuitCanvas();

            StringBuilder s = new StringBuilder();
            s.AppendLine("{");
            s.AppendLine($"\"Width\":\"{cc.ActualWidth}\",");
            s.AppendLine($"\"Height\":\"{cc.ActualHeight}\",");
            s.AppendLine($"\"MostPopulated\":\"{Manager.getMostPopulated()}\"");
            s.AppendLine("}");
            return s.ToString();
        }

        private static string QubitComponentJson()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("[");

            for (int i = 0; i < qubits.Count; i++)
            {
                //s.AppendLine($"\"Qubit{i}\":{{");
                s.AppendLine("{");

                QubitComponent q = qubits[i];

                s.AppendLine($"\"Qubit\":{QubitJson(qubits[i].getQubit())},");
                s.AppendLine($"\n\"Point\":{PointJson(q.GetPoint())}");

                s.AppendLine("}");
                if (i < qubits.Count - 1) { s.Append(","); }
            }

            s.AppendLine("]");
            return s.ToString();
        }

        private static string CircuitComponentJson()
        {
            StringBuilder s = new StringBuilder();
            s.AppendLine("[");

            for (int i = 0; i < components.Count; i++)
            {
                CircuitComponent c = components[i];
                s.AppendLine($"{{\n\"Gate\":{GateJson(c.getGate())},");
                s.AppendLine($"\n\"ImageSource\":\"{c.getImagePath()}\",");

                s.AppendLine($"\n\"Point\":{PointJson(c.getPoint())},");
                s.AppendLine($"\n\"Position\":{PositionJson(c)},");

                s.AppendLine($"\n\"ControlQubits\":{ControlQubitJson(c.getControlQubits())}}}");

                //s.AppendLine($"\n\"Gate{i}\":{GateJson(c.getGate())},");
                //s.AppendLine($"\n\"Point{i}\":{PointJson(c.getPoint())},");
                //s.AppendLine($"\n\"ControlQubits{i}\":[{ControlQubitJson(c.getControlQubits())}]");

                if (i < components.Count - 1) { s.Append(","); }
            }

            s.AppendLine("]");
            return s.ToString();
        }

        private static string PositionJson(CircuitComponent c)
        {
            UserControl[,] alignment = Manager.getFinalAlignment();
            if (c == null) { return "{}"; }

            bool placed = false;
            double x = 0;
            double y = 0;

            for (int row = 0; row < Manager.getQubitCount(); row++)
            {
                if (placed) break;

                for (int col = 0; col < Manager.getMostPopulated(); col++)
                {
                    if (alignment[row, col] == c)
                    {
                        x = col;
                        y = row;
                        placed = true;
                        break;
                    }
                }
            }

            StringBuilder s = new StringBuilder();
            s.AppendLine("{");
            s.AppendLine($"\"X\":\"{x}\",");
            s.AppendLine($"\"Y\":\"{y}\"");
            s.AppendLine("}");
            return s.ToString();
        }

        private static string ControlQubitJson(List<ControlQubit> qubits)
        {
            if (qubits.Count == 0)
            {
                return ("null");
            }

            StringBuilder s = new StringBuilder();
            s.AppendLine("[");

            for (int i = 0; i < qubits.Count; i++)
            {
                s.AppendLine($"{{\"ControlQubit{i}\":{{");
                s.AppendLine($"\"Point\":{PointJson(qubits[i].getPoint())}");
                s.AppendLine("}}");


                if (i < qubits.Count - 1) { s.Append(","); }
            }

            s.AppendLine("]");
            return s.ToString();
        }
        private static string QubitJson(Qubit q)
        {
            if (q == null) { return "{}"; }
            StringBuilder s = new StringBuilder();
            List<Gate> gates = q.getGates();
            s.AppendLine("{");

            s.AppendLine($"\n\"Matrix\":{MatrixJson(q.getMatrix())},");
            s.AppendLine("\"Gates\":[{");

            for (int i = 0; i < gates.Count; i++)
            {
                s.AppendLine($"\"Gate{i}\":[{GateJson(gates[i])}]");
                if (i < gates.Count - 1) { s.Append(","); }
            }


            s.AppendLine("}]}");
            return s.ToString();
        }


        private static string MatrixJson(Matrix m)
        {
            if (m == null) { return "{}"; }

            StringBuilder s = new StringBuilder();
            s.AppendLine("\n{");
            s.AppendLine($"\"Columns\":\"{m.getColumns()}\",");
            s.AppendLine($"\"Rows\":\"{m.getRows()}\",");
            s.AppendLine($"\"Preceder\":\"{m.getPreceder()}\",");

            s.AppendLine($"\"Data\":[");

            for (int r = 0; r < m.getRows(); r++)
            {
                s.AppendLine("{");
                for (int c = 0; c < m.getColumns(); c++)
                {
                    Complex value = m.getData()[r, c];
                    s.AppendLine($"\n\"Cell_{r}-{c}\":{{");
                    s.AppendLine($"\"Real\":\"{value.Real}\",");
                    s.AppendLine($"\"Imaginary\":\"{value.Imaginary}\"");
                    s.AppendLine("}");
                    if (c < m.getColumns() - 1) s.Append(",");
                }
                s.AppendLine("}");
                if (r < m.getRows() - 1) s.Append(",");
            }

            s.AppendLine("]\n}");
            return s.ToString();
        }

        private static string GateJson(Gate g)
        {
            if (g == null) { return "{}"; }
            StringBuilder s = new StringBuilder();
            s.AppendLine("{");

            if (g.GetType() != typeof(Pauli))
            {
                s.AppendLine($"\"Type\":\"{g.GetType()}\",");
            }
            else // Add the pauli type to the end of the string
            {
                s.AppendLine($"\"Type\":\"{g.GetType()}.{((Pauli)g).GetPauliType()}\",");
            }

            s.AppendLine($"\"GUID\":\"{g.getGuid()}\",");
            s.AppendLine($"\"NodeCount\":\"{g.getNodeCount()}\",");
            s.AppendLine($"\"Matrix\":{MatrixJson(g.getMatrix())}");
            s.AppendLine("}");
            return s.ToString();
        }

        private static string PointJson(Point p)
        {
            if (p == null) { return "{}"; }
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
            //Process.Start("notepad++.exe", path);
        }
    }
}
