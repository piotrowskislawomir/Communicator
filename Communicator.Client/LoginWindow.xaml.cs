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
using System.Windows.Shapes;

namespace Communicator.Client
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public sealed partial class LoginWindow : ApplicationWindowBase
    {
        private void Button_WinClose_Click(object sender, RoutedEventArgs e)
        {

            this.Close();
        }

        private void Button_WinMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Height = 500;
        }
    }
}
