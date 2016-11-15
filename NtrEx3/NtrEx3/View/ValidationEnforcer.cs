using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NtrEx3.View
{
    public class ValidationEnforcer : IValidationEnforcer
    {
        private List<TextBox> boxesToValidate;

        public ValidationEnforcer(List<TextBox> boxesToValidate)
        {
            this.boxesToValidate = boxesToValidate;
        }

        public void validateInputFields()
        {
            foreach (TextBox box in boxesToValidate)
            {
                box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
    }
}
