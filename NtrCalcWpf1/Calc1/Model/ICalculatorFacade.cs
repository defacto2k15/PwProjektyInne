using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
    public interface ICalculatorFacade
    {
         void numericButtonPressed(int button);
         void operatorButtonPressed(CalculatorOperator calculatorOperator);
         void dotButtonPressed();
         void clearButtonPressed();
         void invertSignButtonPressed();
         void squareRootButtonPressed();
         void equalButtonPressed();
         void percentButtonPressed();
         void goToErrorState(string description);
         decimal AccumulatorValue
        {
            get;
        }
         string ScreenText
        {
            get;
        }
         CalculatorState CalculatorState
        {
            get;
        }
    }
}
