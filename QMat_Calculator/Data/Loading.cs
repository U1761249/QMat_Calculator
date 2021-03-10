using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QMat_Calculator.Circuits;
using QMat_Calculator.Circuits.Gates;
using QMat_Calculator.Drawable;
using QMat_Calculator.Interfaces;
using QMat_Calculator.Matrices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

/**
* @author Adam Birch - U1761249
*
* @date - 3/2/2021 10:50:52 AM 
*/

namespace QMat_Calculator.Data
{
    class Loading
    {
        private static JObject jObj = null;
        private static double mostPopulated = 0;
        private static double oldWidth = 0;
        private static double oldHeight = 0;

        /// <summary>
        /// Perform the main loading functionality.
        /// </summary>
        public static void Load()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "JSON File (.json)|*.json|All(*.*)|*";

            if (ofd.ShowDialog() == false) return;

            string JsonData = File.ReadAllText(ofd.FileName);
            //elements = JsonConvert.DeserializeObject<List<CircuitComponent>>(JsonData);

            jObj = JObject.Parse(JsonData);

            ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.Children.Clear();

            oldHeight = (double)jObj["Canvas"]["Height"];
            oldWidth = (double)jObj["Canvas"]["Width"];
            mostPopulated = (double)jObj["Canvas"]["MostPopulated"] + 1;

            GetQubits();
            GetComponents();

            ((CircuitCanvas)Manager.getCircuitCanvas()).ResizeQubits();
            ((CircuitCanvas)Manager.getCircuitCanvas()).OrderComponents();
        }

        private static void GetComponents()
        {
            int componentCount = jObj["CircuitComponents"].Count();

            for (int i = 0; i < componentCount; i++)
            {
                Gate g = GetGate(jObj["CircuitComponents"][i]["Gate"]);
                string imageSource = (String)jObj["CircuitComponents"][i]["ImageSource"];
                Point p = GetPoint(jObj["CircuitComponents"][i]["Point"]);
                //Point p = GetPosition(jObj["CircuitComponents"][i]["Position"]);


                CircuitComponent comp = new CircuitComponent(g, imageSource, p);
                ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.Children.Add(comp);
            }
        }

        private static void GetQubits()
        {

            double canvasHeight = ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.ActualHeight;
            double canvasWidth = ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.ActualWidth;

            List<Qubit> qubits = new List<Qubit>();

            int qubitCount = jObj["QubitComponents"].Count();

            for (int i = 0; i < qubitCount; i++)
            {
                //MessageBox.Show(jObj["QubitComponents"][i]["Qubit"]["Matrix"].ToString());


                Matrix m = GetMatrix(jObj["QubitComponents"][i]["Qubit"]["Matrix"]);
                List<Gate> gates = GetGates(jObj["QubitComponents"][i]["Qubit"]["Gates"]);
                Point p = GetPoint(jObj["QubitComponents"][i]["Point"]);

                //double height = (canvasHeight / (qubitCount + 1)) * (i + 1);
                //Point p = new Point(canvasWidth, height);

                Qubit q = new Qubit(m, gates);
                QubitComponent qc = new QubitComponent(q, p);

                qubits.Add(q);
                ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.Children.Add(qc);
                ((CircuitCanvas)Manager.getCircuitCanvas()).addQubitComponents(qc);

                // var qubit = jObj["QubitComponents"][i];
                // MessageBox.Show(qubit.ToString());
            }

            Manager.setQubits(qubits);
        }

        /// <summary>
        /// Get the list of gates within a QubitComponent
        /// </summary>
        /// <param name="gateData"></param>
        /// <returns></returns>
        private static List<Gate> GetGates(JToken gateData)
        {
            List<Gate> gates = new List<Gate>();

            int gateCount = gateData.Count();
            for (int i = 0; i < gateCount; i++)
            {
                gates.Add(GetGate(gateData[i][$"Gate{i}"]));
            }

            return gates;
        }

        /// <summary>
        /// Get an individual gate.
        /// </summary>
        /// <param name="gateData"></param>
        /// <returns></returns>
        public static Gate GetGate(JToken gateData)
        {
            // Remove array brackets from Qubit gates array
            if (gateData.ToString().Substring(0, 1) == "[")
            {
                gateData = (JToken)gateData[0];
            }

            string gateType = (string)gateData["Type"];
            Guid guid = (Guid)gateData["GUID"];
            int nodeCount = (int)gateData["NodeCount"];
            Matrix m = GetMatrix(gateData["Matrix"]);

            switch (gateType)
            {
                case ("QMat_Calculator.Circuits.Gates.Hadamard"):
                    return new Hadamard(guid, nodeCount, m);
                case ("QMat_Calculator.Circuits.Gates.Pauli.X"):
                    return new Pauli(guid, Pauli.PauliType.X, nodeCount, m);
                case ("QMat_Calculator.Circuits.Gates.Pauli.Y"):
                    return new Pauli(guid, Pauli.PauliType.Y, nodeCount, m);
                case ("QMat_Calculator.Circuits.Gates.Pauli.Z"):
                    return new Pauli(guid, Pauli.PauliType.Z, nodeCount, m);
                case ("QMat_Calculator.Circuits.Gates.CNOT"):
                    return new CNOT(guid, nodeCount, m);
                case ("QMat_Calculator.Circuits.Gates.SqrtNOT"):
                    return new SqrtNOT(guid, nodeCount, m);
                case ("QMat_Calculator.Circuits.Gates.Toffoli"):
                    return new Toffoli(guid, nodeCount, m);

                case ("QMat_Calculator.Circuits.Gates.ControlBit"):
                    return new ControlBit(guid, nodeCount, m);
            }
            return CustomGate(gateData);
        }

        /// <summary>
        /// Return the data for a custom gate.
        /// </summary>
        /// <param name="gateData"></param>
        /// <returns></returns>
        public static Gate CustomGate(JToken gateData)
        {
            // TODO: Create custom gate loading.
            return null;
        }


        /// <summary>
        /// Convert a jToken into a matrix.
        /// </summary>
        /// <param name="matrixData"></param>
        /// <returns></returns>
        private static Matrix GetMatrix(JToken matrixData)
        {
            //if (matrixData == null) return new Matrix();

            int columns = (int)matrixData["Columns"];
            int rows = (int)matrixData["Rows"];
            double preceder = (double)matrixData["Preceder"];

            Complex[,] data = new Complex[rows, columns];
            JToken jsonData = matrixData["Data"];

            // Loop through the data values to calculate the complex for each cell of the matrix.
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {

                    string cellTitle = $"Cell_{r}-{c}";

                    double real = (double)jsonData[r][cellTitle]["Real"];
                    double imaginary = (double)jsonData[r][cellTitle]["Imaginary"];
                    Complex complex = new Complex(real, imaginary);
                    data[r, c] = complex;

                }
            }


            return new Matrix(rows, columns, preceder, data);

        }

        /// <summary>
        /// Convert a jToken into a Point
        /// </summary>
        /// <param name="pointData"></param>
        /// <returns></returns>
        private static Point GetPoint(JToken pointData)
        {
            //if (pointData == null) return new Point();

            double x = (double)pointData["X"];
            double y = (double)pointData["Y"];

            double width = ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.ActualWidth;
            double height = ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.ActualHeight;

            x = (x / oldWidth) * width;
            y = (y / oldHeight) * height;

            return new Point(x, y);
        }

        private static Point GetPosition(JToken pointData)
        {

            double x = (double)pointData["X"];
            double y = (double)pointData["Y"];

            double width = ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.ActualWidth;
            double height = ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.ActualHeight;
            double qubitCount = Manager.getQubitCount();

            x = (x + 1) * (width / mostPopulated);
            y = (y + 1) * (height / qubitCount);


            return new Point(x, y);
        }

    }
}
