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
    public sealed partial class MainWindow : ApplicationWindowBase
    {       
        public MainWindow()
        {
            InitializeComponent();
            SetDictionary();
        }
        private void SetDictionary()
        {
            var dict = new ResourceDictionary();
            dict.Source = new Uri("..\\Resources\\NamesDictionary.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);
            this.Resources.MergedDictionaries.Add(dict);
        }

        private void Button_Register_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow register = new RegisterWindow();
            register.Show();

            //to to samo co wyżej: (new RegisterWindow()).Show();
        }

        private void Button_Login_Click(object sender, RoutedEventArgs e)
        {
            //Application.Current.MainWindow.Height = 500;

            //sprawdzam poprawność danych użytkownika
            
            //otwórz komunikator, po prawidłowym zalogowaniu
            CommunicatorWindow chat = new CommunicatorWindow();
            chat.Show();

            //zamykam okno
            this.Close();
        }
    }
}
