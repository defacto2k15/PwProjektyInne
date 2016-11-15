using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NtrEx3.View
{
    class DialogWindowsGenerator : IDialogWindowsGenerator
    {
        public  void generateWarningWindow(String description)
        {
            MessageBox.Show(description);
        }
    }
}
