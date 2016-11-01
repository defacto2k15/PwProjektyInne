using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calc1.Model
{
    class ErrorCalculatorState : ICalculator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ErrorCalculatorState(string screenText)
        {
            log.Debug("Nowy stan kalkulatora:ErrorCalculatorState: Wartosc na ekranie:" + screenText);
            ScreenText = screenText;
        }

        public ICalculator numericButtonPressed(int button)
        {
            return this;
        }
        public ICalculator operatorButtonPressed(CalculatorOperator calculatorOperator)
        {
            log.Error("Nie powinno to sie stac: Wcisnieto przycisk operatora " + calculatorOperator);
            return this;
        }
        public ICalculator dotButtonPressed()
        {
            log.Error("Nie powinno to sie stac: Wcisnieto przycisk przecinka");
            return this;
        }
        public ICalculator clearButtonPressed()
        {
            log.Debug("Wcisnieto przycisk czyszczenia ");
            return new ScreenViewCalculatorState(0, "0", CalculatorOperator.ADD);
        }
        public ICalculator invertSignButtonPressed()
        {
            log.Error("Nie powinno to sie stac: Wcisnieto przycisk odwrocenia znaku ");
            return this;
        }
        public ICalculator squareRootButtonPressed()
        {
            log.Error("Nie powinno to sie stac: Wcisnieto przycisk pierwiastka ");
            return this;
        }
        public ICalculator equalButtonPressed()
        {
            log.Error("Nie powinno to sie stac: Wcisnieto przycisk rownania ");
            return this;
        }
        public ICalculator percentButtonPressed()
        {
            log.Error("Nie powinno to sie stac: Wcisnieto przycisk procentu ");
            return this;
        }
        public decimal AccumulatorValue
        {
            get
            {
                return 0;
            }
        }
        public string ScreenText
        {
            get;
            private set;
        }
        public  CalculatorState CalculatorState{
            get{
                return CalculatorState.ERROR;
            }
        }
    }
}
