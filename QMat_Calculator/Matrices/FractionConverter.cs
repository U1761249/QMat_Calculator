﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
* @author Adam Birch - U1761249
*
* @date - 2/9/2021 9:48:11 AM 
*/

namespace QMat_Calculator.Matrices
{
    public static class FractionConverter
    {
        /// <summary>
        /// Convert a decimal value into an easy to read value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Convert(double value)
        {
            if (value == -1) return String.Format("{0, -5}", " ");

            // \u221A is the Unicode character for √

            // Round to 14 chars as the root2 value is 17 chars long, while the normal is 15. 
            // Round to 14 as they are rounded differently at 15.

            int significantFigures = 14; // How many significant figures to round to.

            value = Math.Round(value, significantFigures);
            double root2 = Math.Round(1 / Math.Sqrt(2), significantFigures, MidpointRounding.AwayFromZero);
            if (value == root2) return String.Format("{0, -5}", "1/\u221A2");

            if (value < root2)
            {
                double currentValue = value;
                int power = 2; // 1 is root2 and already false. Start from 2 (1/2)
                // Check if the value is a power of root2
                while (currentValue < 1)
                {
                    currentValue = Math.Round(currentValue / root2, significantFigures, MidpointRounding.AwayFromZero);

                    string currentString = currentValue.ToString();
                    if (currentString.Length > 12) { currentString = currentString.Substring(0, 12); }
                    string root2String = root2.ToString().Substring(0, 12);
                    if (currentString == root2String) { return Powerof(power, value); }
                    else { power++; }
                }
            }
            return String.Format("{0, -5}", " ");
        }

        /// <summary>
        /// Print the current value as a power of 1/√2.
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        private static string Powerof(int power, double value)
        {
            if (power % 2 == 0) // Even values can be converted to a standard fraction (1/2, 1/4, etc)
            {
                double denominator = Math.Pow(2, (power / 2));
                return String.Format("{0, -5}", $"1/{denominator}");
            }

            else if (power == 3) { return String.Format("{0, -5}", "\u221A2/4"); } // 3 can be simplified to √2/4


            return String.Format("{0, -5}", value.ToString());// any odd value past 3 cannot be simplified to a power of √2/x

        }
    }
}
