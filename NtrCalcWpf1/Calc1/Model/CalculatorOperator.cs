using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
	public class CalculatorOperator {
        private Func<decimal, decimal, decimal> _actionFunc;

        private CalculatorOperator(Func<decimal, decimal, decimal> _actionFunc)
        {
            this._actionFunc = _actionFunc;
        }

        public decimal executeAction(decimal screenVal, decimal accumulatorVal)
        {
            return _actionFunc.Invoke(screenVal, accumulatorVal);
        }

        public static readonly CalculatorOperator ADD = new CalculatorOperator((decimal arg1, decimal arg2) => { return arg1 + arg2; });
        public static readonly CalculatorOperator SUBTRACT = new CalculatorOperator((arg1, arg2) => arg2 - arg1);
        public static readonly CalculatorOperator DIVIDE = new CalculatorOperator((arg1, arg2) => arg2 / arg1);
        public static readonly CalculatorOperator MULTIPLY = new CalculatorOperator((arg1, arg2) => arg1 * arg2);
	}
}
