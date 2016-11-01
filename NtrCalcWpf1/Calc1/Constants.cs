using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1
{
    class Constants
    {
        public static string decimalNumberSeparator
        {
            get {
                return NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
            }
        }

        public static string defaultErrorString
        {
            get
            {
                return "ERR";
            }
        }
    }
}
