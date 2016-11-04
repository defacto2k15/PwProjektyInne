using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Calc1.Model;
using Calc1.View1;
using System.Globalization;

namespace Zad2Test.ModelView
{
    [TestClass]
    public class MainWindowModelViewTest
    {
        MockRepository mocks;
        ICalculatorFacade calc;
        MainWindowViewModel viewModel;
        [TestInitialize]
        public void setUpMocks()
        {
            mocks = new MockRepository();
            calc = (ICalculatorFacade)mocks.StrictMock(typeof(ICalculatorFacade));
            viewModel = new MainWindowViewModel(calc);
        }

        [TestMethod]
        public void MainVindow_NumericButtonIsPassed()
        {
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.SCREEN_VIEW);
            calc.Stub(c => c.ScreenText).Return("2");
            calc.numericButtonPressed(2);
            mocks.ReplayAll();

            viewModel.NumericButtonCommand.Execute("2");
            mocks.VerifyAll();
        }

        [TestMethod]
        public void MainVindow_AddOperatorButtonIsPassed()
        {
            CalculatorOperator calculatorOperator = CalculatorOperator.ADD;

            calc.Stub(c => c.CalculatorState).Return(CalculatorState.SCREEN_VIEW);
            calc.Stub(c => c.ScreenText).Return("2");
            calc.operatorButtonPressed(calculatorOperator);
            mocks.ReplayAll();

            viewModel.AddButtonCommand.Execute(null);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void MainVindow_SqrtButtonIsPassed()
        {
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.SCREEN_VIEW);
            calc.Stub(c => c.ScreenText).Return("2");
            calc.squareRootButtonPressed();
            mocks.ReplayAll();

            viewModel.SquareRootButtonCommand.Execute(null);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void MainVindow_DotButtonIsPassed()
        {
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.SCREEN_VIEW);
            calc.Stub(c => c.ScreenText).Return("2");
            calc.dotButtonPressed();
            mocks.ReplayAll();

            viewModel.DotButtonCommand.Execute(null);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void MainVindow_EqualButtonIsPassed()
        {
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.ACCUMULATOR_VIEW);
            calc.Stub(c => c.AccumulatorValue).Return(2);
            calc.equalButtonPressed();
            mocks.ReplayAll();

            viewModel.EqualButtonCommand.Execute(null);
            mocks.VerifyAll();
        }

        [TestMethod]
        public void MainVindow_ClearButtonIsPassed()
        {
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.SCREEN_VIEW);
            calc.Stub(c => c.ScreenText).Return("2");
            calc.clearButtonPressed();
            mocks.ReplayAll();

            viewModel.CancelButtonCommand.Execute(null);
            mocks.VerifyAll();
        }


        [TestMethod]
        public void MainVindow_WhenInAccumulatorStateAccumulatorValueIsPassedToScreen()
        {
            decimal accumulatorValue = 12345;
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.ACCUMULATOR_VIEW);
            calc.Stub(c => c.AccumulatorValue).Return(accumulatorValue);
            calc.equalButtonPressed();
            mocks.ReplayAll();

            viewModel.EqualButtonCommand.Execute(null);
            mocks.VerifyAll();

            Assert.AreEqual(accumulatorValue.ToString("G"), viewModel.ScreenText);
        }

        [TestMethod]
        public void MainVindow_WhenInScreenStateScreenValueIsPassedToScreen()
        {
            String screenValue = "123";
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.SCREEN_VIEW);
            calc.Stub(c => c.ScreenText).Return(screenValue);
            calc.operatorButtonPressed(CalculatorOperator.ADD);
            mocks.ReplayAll();

            viewModel.AddButtonCommand.Execute(null);
            mocks.VerifyAll();

            Assert.AreEqual(screenValue, viewModel.ScreenText);
        }

        [TestMethod]
        public void MainVindow_WhenAccumulatorValueIsTooBigAfterPointItIsTrimmed()
        {
            decimal accValue = new decimal(12345678.12345);
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.ACCUMULATOR_VIEW);
            calc.Stub(c => c.AccumulatorValue).Return(accValue);
            calc.equalButtonPressed();
            mocks.ReplayAll();

            viewModel.EqualButtonCommand.Execute(null);
            mocks.VerifyAll();

            Assert.AreEqual(new decimal(12345678.12).ToString("G"), viewModel.ScreenText);
        }

        [TestMethod]
        public void MainVindow_WhenAccumulatoorValueIsTooBigItIsWrittenInScientificNotation()
        {
            decimal accValue = new decimal(12345678999);
            calc.Stub(c => c.CalculatorState).Return(CalculatorState.ACCUMULATOR_VIEW);
            calc.Stub(c => c.AccumulatorValue).Return(accValue);
            calc.equalButtonPressed();
            mocks.ReplayAll();

            viewModel.EqualButtonCommand.Execute(null);
            mocks.VerifyAll();

            Assert.AreEqual(accValue.ToString("e", CultureInfo.CurrentCulture), viewModel.ScreenText);
        }
    }
}
