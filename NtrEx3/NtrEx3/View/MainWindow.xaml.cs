using NtrEx3.Model;
using NtrEx3.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace NtrEx3.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public MainWindow()
        {
            log.Info("Uruchomienie Progamu");
            InitializeComponent();
            DataContext = new MainWindowViewModel(new Storage(), new DialogWindowsGenerator(), new ValidationEnforcer( new List<TextBox>( new TextBox[]{FirstNameTextBox,LastNameTextBox, IndexTextBox})) );
        }
    }
}
