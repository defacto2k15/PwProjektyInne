using Calc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calc1
{
    class NoArgumentRelayCommand : MyCommand
    {
        Action _action;
        public NoArgumentRelayCommand(Action action)
        {
            this._action = action;
        }

        #region ICommand Members
        public event EventHandler CanExecuteChanged;

        public override void Execute(object parameter)
        {
            _action();  
        }
        #endregion

    }
}
