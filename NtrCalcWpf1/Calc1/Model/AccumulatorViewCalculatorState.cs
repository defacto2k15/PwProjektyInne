﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
    class AccumulatorViewCalculatorState : BaseCalculatorState
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AccumulatorViewCalculatorState(decimal accumulatorValue, string screenValue, CalculatorOperator calculatorOperator)
            : base(accumulatorValue, screenValue, calculatorOperator)
        {
            log.Debug("Nowy stan kalkulatora:AccumulatorViewCalculatorState: Wartosc w akumulatorze:" + accumulatorValue + " Wartosc na ekranie:" + screenValue + " operator:" + calculatorOperator);
        }

        public override ICalculator numericButtonPressed(int button)
        {
			return new ScreenViewCalculatorState(0, "0", CalculatorOperator.ADD).numericButtonPressed(button);

        }
        public override ICalculator operatorButtonPressed(CalculatorOperator calculatorOperator)
        {
            log.Debug("Wcisnieto przycisk operatora "+calculatorOperator);
            return new ScreenViewCalculatorState(AccumulatorValue, "0", calculatorOperator);
        }
        public override ICalculator dotButtonPressed()
        {
            return new ScreenViewCalculatorState(0, "0", CalculatorOperator.ADD).dotButtonPressed();
        }
        public override ICalculator clearButtonPressed()
        {
            log.Debug("Wcisnieto przycisk czyszczenia ");
            return new ScreenViewCalculatorState(0, "0", CalculatorOperator.ADD);
        }
        public override ICalculator invertSignButtonPressed()
        {
            log.Debug("Wcisnieto przycisk odwrocenia znaku ");
            AccumulatorValue = AccumulatorValue * -1;
            return new AccumulatorViewCalculatorState(AccumulatorValue, ScreenText, calculatorOperator);
        }

        public override ICalculator squareRootButtonPressed()
        {
            log.Debug("Wcisnieto przycisk pierwiastka ");
            if (AccumulatorValue < 0)
            {
                return new ErrorCalculatorState(Constants.defaultErrorString);
            }
            return new AccumulatorViewCalculatorState(  (decimal)Math.Sqrt((double)AccumulatorValue), ScreenText, CalculatorOperator.ADD);
        }
        public override ICalculator equalButtonPressed()
        {
            log.Debug("Wcisnieto przycisk wykonania rownania ");
            return new ScreenViewCalculatorState(AccumulatorValue, ScreenText, calculatorOperator).equalButtonPressed();
        }
        public override ICalculator percentButtonPressed()
        {
            log.Debug("Wcisnieto przycisk procentu ");
            return clearButtonPressed();
        }
        public override CalculatorState CalculatorState
        {
            get
            {
                return CalculatorState.ACCUMULATOR_VIEW;
            }
        }
    }
}