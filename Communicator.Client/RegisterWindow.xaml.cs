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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public sealed partial class RegisterWindow : ApplicationWindowBase
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            //zapisywanie danych dot. użytkownika
            //zamykam okno
            this.Close();
        }
    }
}
