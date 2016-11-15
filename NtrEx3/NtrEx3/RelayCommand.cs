using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NtrEx3
{
    public class RelayCommand : ICommand
    {
        private Action _action;
        private Func<bool> _canExecuteFunc;

        public RelayCommand(Action action)
        {
            _action = action;
            _canExecuteFunc = () => true;
        }

        public RelayCommand(Action action, Func<bool> canExecuteFunc)
        {
            _action = action;
            _canExecuteFunc = canExecuteFunc;
        }


        public virtual void Execute(object parameter)
        {
            _action();
        }

        public virtual bool CanExecute(object unused)
        {
            return _canExecuteFunc();
        }

        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
