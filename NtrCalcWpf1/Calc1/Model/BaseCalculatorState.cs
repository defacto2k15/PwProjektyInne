using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
    abstract class BaseCalculatorState : ICalculator
    {
        protected BaseCalculatorState(decimal AccumulatorValue, string ScreenText, CalculatorOperator calculatorOperator, int maxDigitsOnScreen)
        {
            this.AccumulatorValue = AccumulatorValue;
            this.ScreenText = ScreenText;
            this.calculatorOperator = calculatorOperator;
            this.maxDigitsOnScreen = maxDigitsOnScreen;
        }

        protected int maxDigitsOnScreen;
        protected CalculatorOperator calculatorOperator;
        public decimal AccumulatorValue { get; protected set; }
        public string ScreenText { get; protected set; }
        public decimal ScreenValue
        {
            get
            {
                return Decimal.Parse(ScreenText);
            }
            set
            {
                ScreenText = value.ToString();
            }
        }

        public abstract ICalculator numericButtonPressed(int button);
        public abstract ICalculator operatorButtonPressed(CalculatorOperator calculatorOperator);
        public abstract ICalculator dotButtonPressed();
        public abstract ICalculator clearButtonPressed();
        public abstract ICalculator invertSignButtonPressed();
        public abstract ICalculator squareRootButtonPressed();
        public abstract ICalculator equalButtonPressed();
        public abstract ICalculator percentButtonPressed();
        public abstract CalculatorState CalculatorState{
            get;   
        }
    }
}
