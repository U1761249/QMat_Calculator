using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 1/28/2021 11:18:04 AM 
*/

namespace QMat_Calculator.Circuits.Gates
{
    public class ControlBit : Gate
    {
        public ControlBit()
        {

        }

        public string CreateJson()
        {
            string s =
                "{\n" +
                "\n}\n";

            return s;
        }

        //TODO: Implement a control bit to populate a column when a multi-bit gate is used.
    }
}
