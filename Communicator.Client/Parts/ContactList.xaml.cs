using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Communicator.Client.ViewModels
{
    /// <summary>
    /// Logika interakcji dla klasy ContactList.xaml
    /// </summary>
    public partial class ContactList : UserControl
    {
        public ContactList()
        {
            InitializeComponent();
            DataContext = new ContactViewModel();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
            ConversationWindow sample = new ConversationWindow();
            sample.Show();
        }
    }
}
