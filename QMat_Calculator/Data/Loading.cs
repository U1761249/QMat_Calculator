using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using QMat_Calculator.Circuits;
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

            GetQubits();


            //int qubitCount = jObj["QubitComponents"].Count();
            //for (int i = 0; i < qubitCount; i++)
            //{
            //    var qubit = jObj["QubitComponents"][i];
            //    MessageBox.Show(qubit.ToString());
            //}

            //int componentCount = jObj["CircuitComponents"].Count();
            //for (int i = 0; i < componentCount; i++)
            //{
            //    var comp = jObj["CircuitComponents"][i];
            //    MessageBox.Show(comp.ToString());
            //}
        }

        private static void GetQubits()
        {
            ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.Children.Clear();

            List<Qubit> qubits = new List<Qubit>();

            int qubitCount = jObj["QubitComponents"].Count();
            for (int i = 0; i < qubitCount; i++)
            {
                //MessageBox.Show(jObj["QubitComponents"][i]["Qubit"]["Matrix"].ToString());


                Matrix m = GetMatrix(jObj["QubitComponents"][i]["Qubit"]["Matrix"]);
                List<Gate> gates = GetGates(jObj["QubitComponents"][i]["Qubit"]["Gates"]);
                Point p = GetPoint(jObj["QubitComponents"][i]["Point"]);

                QubitComponent qc = new QubitComponent(new Qubit(m, gates), p);

                ((CircuitCanvas)Manager.getCircuitCanvas()).MainCircuitCanvas.Children.Add(qc);

                // var qubit = jObj["QubitComponents"][i];
                // MessageBox.Show(qubit.ToString());
            }

            Manager.setQubits(qubits);
            ((CircuitCanvas)Manager.getCircuitCanvas()).ResizeQubits();
            ((CircuitCanvas)Manager.getCircuitCanvas()).OrderComponents();
        }

        private static List<Gate> GetGates(JToken gateData)
        {
            List<Gate> gates = new List<Gate>();



            return gates;
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
            int preceder = (int)matrixData["Preceder"];

            Complex[,] data = new Complex[rows, columns];
            JToken jsonData = matrixData["Data"];

            // Loop through the data values to calculate the complex for each cell of the matrix.
            int index = 0;
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {

                    string cellTitle = $"Cell_{r}-{c}";

                    double real = (double)jsonData[index][cellTitle]["Real"];
                    double imaginary = (double)jsonData[index][cellTitle]["Imaginary"];
                    Complex complex = new Complex(real, imaginary);
                    data[r, c] = complex;
                    index++;
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

            return new Point(x, y);
        }

    }
}
