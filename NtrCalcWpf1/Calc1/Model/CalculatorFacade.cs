using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
    public class CalculatorFacade : ICalculatorFacade
    {
        ICalculator currentCalculator = new ScreenViewCalculatorState(0, "0", CalculatorOperator.ADD);

        public override void numericButtonPressed(int button)
        {
            currentCalculator = currentCalculator.numericButtonPressed(button);
        }
        public override void operatorButtonPressed(CalculatorOperator calculatorOperator)
        {
            currentCalculator = currentCalculator.operatorButtonPressed(calculatorOperator);
        }
        public override void dotButtonPressed()
        {
            currentCalculator = currentCalculator.dotButtonPressed();
        }
        public override void clearButtonPressed()
        {
            currentCalculator = currentCalculator.clearButtonPressed();
        }
        public override void invertSignButtonPressed()
        {
            currentCalculator = currentCalculator.invertSignButtonPressed();
        }
        public override void squareRootButtonPressed()
        {
            currentCalculator = currentCalculator.squareRootButtonPressed();
        }
        public override void equalButtonPressed()
        {
            currentCalculator = currentCalculator.equalButtonPressed();
        }
        public override void percentButtonPressed()
        {
            currentCalculator = currentCalculator.percentButtonPressed();
        }
        public override decimal AccumulatorValue
        {
            get
            {
                return currentCalculator.AccumulatorValue;
            }
        }
        public override string ScreenText
        {
            get
            {
                return currentCalculator.ScreenText;
            }
        }
        public override CalculatorState CalculatorState
        {
            get
            {
                return currentCalculator.CalculatorState;
            }
        }
    }
}
