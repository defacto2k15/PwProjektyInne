using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Calc1.Model;
using Calc1;

namespace Zad2Test.Model
{
    [TestClass]
    public class CalculatorFacadeTest
    {
        [TestMethod]
        public void CalculatorFacadeMainTest()
        {
            test(aTestData().pressNumbers("1234"), ExpectedScreenValue:"1234");
            test(aTestData().pressNumbers("1234567890"), ExpectedScreenValue: "1234567890");
            test(aTestData().pressNumbers("12345678901"), ExpectedScreenValue: "1234567890");
            test(aTestData().pressNumbers("1234").dot().pressNumbers("1234"), ExpectedScreenValue: "1234,1234");
            test(aTestData().pressNumbers("1234").dot().dot().dot().pressNumbers("1234"), ExpectedScreenValue: "1234,1234");
            test(aTestData().dot().pressNumbers("1234"), ExpectedScreenValue: "0,1234");
            test(aTestData().pressNumbers("000012").dot().pressNumbers("1234"), ExpectedScreenValue: "12,1234");
            test(aTestData().dot().pressNumbers("0001234"), ExpectedScreenValue: "0,0001234");

            test(aTestData().pressNumbers("1234").add().pressNumbers("1234").eq(), ExpectedAccumulatorValue:2468);
            test(aTestData().pressNumbers("1234").add().dot().pressNumbers("1234").eq(), ExpectedAccumulatorValue: new Decimal(1234.1234));
            test(aTestData().pressNumbers("300").subtract().pressNumbers("600").eq(), ExpectedAccumulatorValue: -300);
            test(aTestData().pressNumbers("300").multiply().pressNumbers("3").eq(), ExpectedAccumulatorValue: 900);
            test(aTestData().pressNumbers("300").multiply().pressNumbers("00").dot().pressNumbers("2").eq(), ExpectedAccumulatorValue: 60);
            test(aTestData().pressNumbers("100").divide().pressNumbers("4").eq(), ExpectedAccumulatorValue: 25);
            test(aTestData().pressNumbers("100").divide().pressNumbers("25").eq(), ExpectedAccumulatorValue: 4);
            test(aTestData().pressNumbers("100").divide().pressNumbers("00000").eq(), state:CalculatorState.ERROR);
            test(aTestData().pressNumbers("100").divide().eq(), null, null, state:CalculatorState.ERROR);

            test(aTestData().pressNumbers("81").sqrt(), ExpectedAccumulatorValue:9, state: CalculatorState.ACCUMULATOR_VIEW);
            test(aTestData().pressNumbers("81").inv().sqrt(), state:CalculatorState.ERROR);

            test(aTestData().pressNumbers("1234").inv(), ExpectedScreenValue:"-1234");
            test(aTestData().pressNumbers("0000").inv(), ExpectedScreenValue:"0");

            test(aTestData().pressNumbers("1234").clear().pressNumbers("2222"), ExpectedScreenValue:"2222");
            test(aTestData().pressNumbers("1234").add().clear().pressNumbers("2222"), ExpectedScreenValue:"2222");

            test(aTestData().pressNumbers("100").add().pressNumbers("5").eq().eq().eq().eq(), ExpectedAccumulatorValue: 120);

            test(aTestData().pressNumbers("999999999").multiply().pressNumbers("999999999").eq().eq().eq(), ExpectedScreenValue: "OVERFLOW", state:CalculatorState.ERROR);
        }

        void test(TestData data, string ExpectedScreenValue = null, decimal? ExpectedAccumulatorValue = null, CalculatorState? state = null)
        {
            CalculatorFacade calculator = new CalculatorFacade(Constants.DIGITS_SCREEN_SIZE);
            String description = data.executeActions(calculator);
            if (ExpectedScreenValue != null)
            {
                Assert.AreEqual(ExpectedScreenValue, calculator.ScreenText, description);
            }
            if (ExpectedAccumulatorValue != null)
            {
                Assert.AreEqual(ExpectedAccumulatorValue, calculator.AccumulatorValue, description);
            }
            if (state != null && state.HasValue)
            {
                Assert.AreEqual(state.Value, calculator.CalculatorState, description);
            }  
        }

        TestData aTestData()
        {
            return new TestData();
        }
    }

    class TestData
    {
        private string description;
        private List<Action<CalculatorFacade>> actionsList = new List<Action<CalculatorFacade>>();

        public string executeActions(CalculatorFacade calc)
        {
            foreach (var action in actionsList)
            {
                action(calc);
            }
            return description;
        }

        public TestData pressNumbers(string numbers)
        {
            description = description + " PressedNumbers:" + numbers;
            actionsList.Add((calc) => InternalPressNumbers(calc, numbers));
            return this;
        }

        public TestData dot()
        {
            description = description + " PressedDot";
            actionsList.Add((calc) => calc.dotButtonPressed());
            return this;
        }

        public TestData add()
        {
            description = description + " PressedAdd";
            actionsList.Add((calc) => calc.operatorButtonPressed(CalculatorOperator.ADD));
            return this;
        }

        public TestData subtract()
        {
            description = description + " PressedSubtract";
            actionsList.Add((calc) => calc.operatorButtonPressed(CalculatorOperator.SUBTRACT));
            return this;
        }

        public TestData multiply()
        {
            description = description + " PressedMultiply";
            actionsList.Add((calc) => calc.operatorButtonPressed(CalculatorOperator.MULTIPLY));
            return this;
        }
        public TestData divide()
        {
            description = description + " PressedDivide";
            actionsList.Add((calc) => calc.operatorButtonPressed(CalculatorOperator.DIVIDE));
            return this;
        }
        public TestData eq()
        {
            description = description + " PressedEquals";
            actionsList.Add((calc) => calc.equalButtonPressed());
            return this;
        }
        public TestData sqrt()
        {
            description = description + " PressedSqrt";
            actionsList.Add((calc) => calc.squareRootButtonPressed());
            return this;
        }
        public TestData inv()
        {
            description = description + " PressedInv";
            actionsList.Add((calc) => calc.invertSignButtonPressed());
            return this;
        }
        public TestData clear()
        {
            description = description + " PressedClear";
            actionsList.Add((calc) => calc.clearButtonPressed());
            return this;
        }

        private void InternalPressNumbers(CalculatorFacade calculator, String inputNumbers)
        {
            foreach (var digit in inputNumbers.ToCharArray())
            {
                var digitAsInt = Int32.Parse(digit.ToString());
                calculator.numericButtonPressed(digitAsInt);
            }
        }
    }

}
