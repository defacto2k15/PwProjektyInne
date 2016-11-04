using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
    public class CalculatorFacade : ICalculatorFacade
    {
        private int maxDigitsOnScreen;
        public CalculatorFacade( int maxDigitsOnScreen)
        {
            this.maxDigitsOnScreen = maxDigitsOnScreen;
            this.currentCalculator = new ScreenViewCalculatorState(0, "0", CalculatorOperator.ADD, maxDigitsOnScreen);
        }

        ICalculator currentCalculator;

        public  void numericButtonPressed(int button)
        {
            currentCalculator = currentCalculator.numericButtonPressed(button);
        }
        public  void operatorButtonPressed(CalculatorOperator calculatorOperator)
        {
            currentCalculator = currentCalculator.operatorButtonPressed(calculatorOperator);
        }
        public  void dotButtonPressed()
        {
            currentCalculator = currentCalculator.dotButtonPressed();
        }
        public  void clearButtonPressed()
        {
            currentCalculator = currentCalculator.clearButtonPressed();
        }
        public  void invertSignButtonPressed()
        {
            currentCalculator = currentCalculator.invertSignButtonPressed();
        }
        public  void squareRootButtonPressed()
        {
            currentCalculator = currentCalculator.squareRootButtonPressed();
        }
        public  void equalButtonPressed()
        {
            currentCalculator = currentCalculator.equalButtonPressed();
        }
        public  void percentButtonPressed()
        {
            currentCalculator = currentCalculator.percentButtonPressed();
        }
        public void goToErrorState(string description)
        {
            currentCalculator = new ErrorCalculatorState(description, maxDigitsOnScreen);
        }

        public  decimal AccumulatorValue
        {
            get
            {
                return currentCalculator.AccumulatorValue;
            }
        }
        public  string ScreenText
        {
            get
            {
                return currentCalculator.ScreenText;
            }
        }
        public  CalculatorState CalculatorState
        {
            get
            {
                return currentCalculator.CalculatorState;
            }
        }
    }
}
