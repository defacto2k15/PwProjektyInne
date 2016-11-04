using Calc1.View1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
    class ScreenViewCalculatorState : BaseCalculatorState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ScreenViewCalculatorState(decimal accumulatorValue, string screenValue, CalculatorOperator calculatorOperator, int maxDigitsOnScreen) 
            : base(accumulatorValue, screenValue, calculatorOperator, maxDigitsOnScreen)
        {
                log.Debug("Nowy stan kalkulatora: ScreenViewCalculatorStateL Wartosc w akumulatorze:" + accumulatorValue + " Wartosc na ekranie:" + screenValue + " operator:" + calculatorOperator);
        }

        public override ICalculator numericButtonPressed(int button)
        {
            if (ScreenText.Length < maxDigitsOnScreen)
            {
                if (ScreenText.Equals("0"))
                {
                    ScreenValue = button;
                }
                else
                {
                    ScreenText = ScreenText + button;
                }
            }
			return new ScreenViewCalculatorState(AccumulatorValue, ScreenText, calculatorOperator, maxDigitsOnScreen);

        }
        public override ICalculator operatorButtonPressed(CalculatorOperator newCalculatorOperator)
        {
            log.Debug("Wcisnieto przycisk operatora " + calculatorOperator);
            try
            {
                if (calculatorOperator == CalculatorOperator.DIVIDE && ScreenValue == 0)
                {
                    return new ErrorCalculatorState(Constants.defaultErrorString, maxDigitsOnScreen);
                }
                AccumulatorValue = calculatorOperator.executeAction(ScreenValue, AccumulatorValue);
                return new ScreenViewCalculatorState(AccumulatorValue, "0", newCalculatorOperator, maxDigitsOnScreen);
            } catch( OverflowException e)
            {
                log.Warn("Overflow exception ", e);
                return new ErrorCalculatorState(Constants.OVERFLOW, maxDigitsOnScreen);
            }
        }
        public override ICalculator dotButtonPressed()
        {
            if (thereIsNoDot() && ScreenText.Length < maxDigitsOnScreen)
            {
                ScreenText = ScreenText + Constants.decimalNumberSeparator;
            }
            return this;
        }

        private bool thereIsNoDot(){
            return !ScreenText.Contains(Constants.decimalNumberSeparator);
        }

        public override ICalculator clearButtonPressed()
        {
            log.Debug("Wcisnieto przycisk czyszczenia ");
            return new ScreenViewCalculatorState(0, "0", CalculatorOperator.ADD, maxDigitsOnScreen);
        }
        public override ICalculator invertSignButtonPressed()
        {
            log.Debug("Wcisnieto przycisk odwrocenia znaku ");
            String newScreenValue = "?";
            if( ScreenText.StartsWith("-") ){
                newScreenValue = ScreenText.Substring(1);
            }
            else if (!ScreenText.Equals("0"))
            {
                newScreenValue = '-' + ScreenText;
            }
            else
            {
                newScreenValue = ScreenText;
            }
            return new ScreenViewCalculatorState(AccumulatorValue, newScreenValue, calculatorOperator, maxDigitsOnScreen);
        }

        public override ICalculator squareRootButtonPressed()
        {
            log.Debug("Wcisnieto przycisk pierwiastka ");
            if (ScreenValue < 0)
            {
                return new ErrorCalculatorState(Constants.defaultErrorString, maxDigitsOnScreen);
            }
            ScreenValue = (decimal)Math.Sqrt((double)ScreenValue);
            return new AccumulatorViewCalculatorState(ScreenValue, "0", CalculatorOperator.ADD, maxDigitsOnScreen);
        }
        public override ICalculator equalButtonPressed()
        {
            log.Debug("Wcisnieto przycisk wykonania rownania ");
            try
            {
                if (calculatorOperator == CalculatorOperator.DIVIDE && ScreenValue == 0)
                {
                    return new ErrorCalculatorState(Constants.defaultErrorString, maxDigitsOnScreen);
                }
                AccumulatorValue = calculatorOperator.executeAction(ScreenValue, AccumulatorValue);
                return new AccumulatorViewCalculatorState(AccumulatorValue, ScreenText, calculatorOperator, maxDigitsOnScreen);
            }
            catch (OverflowException e)
            {
                log.Warn("Overflow exception ", e);
                return new ErrorCalculatorState(Constants.OVERFLOW, maxDigitsOnScreen);
            }
        }
        public override ICalculator percentButtonPressed()
        {
            log.Debug("Wcisnieto przycisk procentu ");
            ScreenValue = AccumulatorValue * (ScreenValue * (decimal)0.01);
            if( calculatorOperator == CalculatorOperator.DIVIDE && ScreenValue == 0 ){
                return new ErrorCalculatorState(Constants.defaultErrorString, maxDigitsOnScreen);
            }
            AccumulatorValue = calculatorOperator.executeAction(ScreenValue, AccumulatorValue);
            return new AccumulatorViewCalculatorState(AccumulatorValue, "0", calculatorOperator, maxDigitsOnScreen);
        }
        public override CalculatorState CalculatorState
        {
            get
            {
                return CalculatorState.SCREEN_VIEW;
            }
        }
    }
}