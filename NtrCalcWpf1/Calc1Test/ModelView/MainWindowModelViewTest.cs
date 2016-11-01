using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Calc1.Model;
using Calc1.View1;

namespace Calc1Test.ModelView
{
    [TestClass]
    public class MainWindowModelViewTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            MockRepository mocks = new MockRepository();
            ICalculatorFacade calc = (ICalculatorFacade)mocks.StrictMock(typeof(ICalculatorFacade));
            calc.numericButtonPressed(2);
            mocks.ReplayAll();

            MainWindowViewModel viewModel = new MainWindowViewModel(calc);
            viewModel.NumericButtonCommand.Execute("2");
            mocks.VerifyAll();
        }
    }
}
