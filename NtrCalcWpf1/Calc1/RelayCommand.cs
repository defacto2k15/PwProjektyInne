using Calc1;
using System;
using System.Windows.Input;
namespace Calc
{
    class RelayCommand<T> : MyCommand
    {
        private Action<T> _action;
        public RelayCommand(Action<T> action)
        {
            _action = action;
        }

        #region ICommand Members


        public override void Execute(object parameter)
        {
            if (parameter != null)
            {
                _action((T)parameter);
            }
            else
            {
                throw new ArgumentNullException("In execute method in RelayCommand");
            }          
        }
        #endregion
    }
}