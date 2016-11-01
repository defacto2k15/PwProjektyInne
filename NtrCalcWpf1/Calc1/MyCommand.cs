using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Calc1
{
    abstract class MyCommand : ICommand
    {
        public virtual bool CanExecute(object unused)
        {
            return true;
        }
        public abstract void Execute(object parameter);
        public event EventHandler CanExecuteChanged;
    }
}
