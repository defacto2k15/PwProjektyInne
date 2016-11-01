using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
    interface ICalculator
    {
        ICalculator numericButtonPressed(int button);
        ICalculator operatorButtonPressed(CalculatorOperator calculatorOperator);
        ICalculator dotButtonPressed();
        ICalculator clearButtonPressed();
        ICalculator invertSignButtonPressed();
        ICalculator squareRootButtonPressed();
        ICalculator equalButtonPressed();
        ICalculator percentButtonPressed();
        decimal AccumulatorValue { get; }
        string ScreenText { get; }
        CalculatorState CalculatorState{ get;}
    }
}
