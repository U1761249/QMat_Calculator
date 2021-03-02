using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// Perform the main loading functionality.
        /// </summary>
        public static void Load()
        {

            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.Filter = "JSON File (.json)|*.json|All(*.*)|*";

            if (ofd.ShowDialog() == false) return;

            string JsonData = File.ReadAllText(ofd.FileName);
            MessageBox.Show(JsonData);
        }
    }
}
