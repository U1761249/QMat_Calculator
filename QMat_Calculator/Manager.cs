using QMat_Calculator.Circuits;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 12/18/2020 1:33:05 PM 
*/

namespace QMat_Calculator
{
    /// <summary>
    /// /// This class stores persistent information throughout the runtime of the program.
    /// </summary>
    static class Manager
    {
        public static List<Qubit> qubits = new List<Qubit>();
        //TODO: Calculate the number of Qubits used and the appropriate Kronecker product for the gates used.
    }
}
