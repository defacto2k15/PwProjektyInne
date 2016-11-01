using Calc;
using Calc1.Model;
using Numerics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calc1.View1
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ICalculatorFacade calculator;

        public ICommand NumericButtonCommand {get; private set;}
        public ICommand DotButtonCommand { get; private set; } 
        public ICommand SignButtonCommand { get; private set; }
        public ICommand DivideButtonCommand { get; private set; }
        public ICommand MultiplyButtonCommand { get; private set; }
        public ICommand SubtractButtonCommand { get; private set; }
        public ICommand AddButtonCommand { get; private set; }
        public ICommand CancelButtonCommand { get; private set; }
        public ICommand SquareRootButtonCommand { get; private set; }
        public ICommand PercentButtonCommand { get; private set; }
        public ICommand EqualButtonCommand { get; private set; }

        public MainWindowViewModel(ICalculatorFacade calculator)
        {
            this.calculator = calculator;
            screenText = "0";
            NumericButtonCommand = createActionCommand((string a) => NumericButtonClicked(a));
            DotButtonCommand = createActionCommand( () => calculator.dotButtonPressed());
            SignButtonCommand = createActionCommand( () => calculator.invertSignButtonPressed());
            DivideButtonCommand = createActionCommand( () => calculator.operatorButtonPressed(CalculatorOperator.DIVIDE));
            MultiplyButtonCommand = createActionCommand( () => calculator.operatorButtonPressed(CalculatorOperator.MULTIPLY));
            SubtractButtonCommand = createActionCommand( () => calculator.operatorButtonPressed(CalculatorOperator.SUBTRACT));
            AddButtonCommand = createActionCommand( () => calculator.operatorButtonPressed(CalculatorOperator.ADD));
            CancelButtonCommand = createActionCommand( () => calculator.clearButtonPressed());
            SquareRootButtonCommand = createActionCommand( () => calculator.squareRootButtonPressed());
            PercentButtonCommand = createActionCommand( () => calculator.percentButtonPressed());
            EqualButtonCommand = createActionCommand( () => calculator.equalButtonPressed());
        }

        public void NumericButtonClicked(string obj)
        {
            calculator.numericButtonPressed(Int32.Parse(obj));
        }

        private ICommand createActionCommand<T>(Action<T> newAction)
        {
            Action<T> finalAction = (arg) =>
            {
                newAction(arg);
                setScreen();
            };
            return new RelayCommand<T>(finalAction); 
        }

        private ICommand createActionCommand(Action newAction)
        {
            Action finalAction = () =>
            {
                newAction();
                setScreen();
            };
            return new NoArgumentRelayCommand(finalAction);
        }

        public const int DIGITS_SCREEN_SIZE = 10;

        private void setScreen()
        {
            if (calculator.CalculatorState == CalculatorState.ACCUMULATOR_VIEW)
            {
                String accumulatorValueString = calculator.AccumulatorValue.ToString("G");
                var trimmedNumber = trimNumberToScreen(accumulatorValueString);
                if (trimmedNumber.Item1)
                {
                    ShouldButtonsBeEnabled = true;
                    ScreenText = trimmedNumber.Item2;
                } else {
                    ShouldButtonsBeEnabled = false;
                    new ErrorCalculatorState(trimmedNumber.Item2 + "E");
                    ScreenText = calculator.ScreenText;
                }
            }
            else if (calculator.CalculatorState == CalculatorState.SCREEN_VIEW)
            {
                var trimmedNumber = trimNumberToScreen(calculator.ScreenText);
                if (trimmedNumber.Item1)
                {
                    ShouldButtonsBeEnabled = true;
                    ScreenText = trimmedNumber.Item2;
                }
                else
                {
                    ShouldButtonsBeEnabled = false;
                    new ErrorCalculatorState(trimmedNumber.Item2 + "E");
                    ScreenText = calculator.ScreenText;
                }
            }
            else
            {
                ShouldButtonsBeEnabled = false;
                ScreenText = calculator.ScreenText;
            }
            Console.WriteLine("It is " + calculator.ScreenText);
        }

        Tuple<bool, string> trimNumberToScreen(string inputString)
        {
            bool numberIsNegative = false;
            bool goodNumber = true;
            if (inputString.StartsWith("-"))
            {
                numberIsNegative = true;
                inputString = inputString.Substring(1);
            }

            if (inputString.Length > DIGITS_SCREEN_SIZE)
            {
                if (inputString.Contains(Constants.decimalNumberSeparator) == false)
                {
                    inputString = inputString.Substring(0, DIGITS_SCREEN_SIZE );
                    goodNumber = false;
                }
                else
                {
                    var commaIndex = inputString.IndexOf(Constants.decimalNumberSeparator);
                    if (commaIndex > DIGITS_SCREEN_SIZE)
                    {
                        inputString = inputString.Substring(0, DIGITS_SCREEN_SIZE );
                        goodNumber = false;
                    }
                    else if (commaIndex == DIGITS_SCREEN_SIZE-1)
                    {
                        if( inputString.Length >= DIGITS_SCREEN_SIZE ){
                            inputString = inputString.Substring(0, DIGITS_SCREEN_SIZE+1);    
                        } else {
                            inputString = inputString.Substring(0, DIGITS_SCREEN_SIZE - 1);
                        }
                        goodNumber = true;
                    }
                    else
                    {
                        inputString = inputString.Substring(0, DIGITS_SCREEN_SIZE );
                        goodNumber = true;
                    }
                }
            }
            if (numberIsNegative)
            {
                inputString = "-" + inputString;
            }  
            return Tuple.Create(goodNumber, inputString);
        }

        private string screenText;
        public string ScreenText
        {
            get { return screenText; }
            set
            {
                screenText = value;
                OnPropertyChanged("ScreenText");
            }
        }

        private bool shouldButtonsBeEnabled = true;
        private bool shouldButtonsBeGreyedOut()
        {
            return !shouldButtonsBeEnabled;
        }
        public bool ShouldButtonsBeEnabled
        {
            get { return shouldButtonsBeEnabled; }
            set
            {
                log.Debug("Czy przyciski powinny byc wyszarzone: " + value);
                shouldButtonsBeEnabled = value;
                OnPropertyChanged("shouldButtonsBeEnabled");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null) this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
