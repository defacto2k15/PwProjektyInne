using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1
{
    public class Constants
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
        public const int DIGITS_SCREEN_SIZE = 10;
        internal static string OVERFLOW = "OVERFLOW";
    }
}
