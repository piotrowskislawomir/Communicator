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
    public partial class ContactList : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Uri statusImageUri;

        public Uri StatusImageUri
        {
            //sorawdź login dla obecnego użytkownika
            //sprawdz status i zwroc odpowiedni link do orbrazka
            get
            {
                return statusImageUri;
            }
            set
            {
                statusImageUri = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("StatusImageUri"));
                }
            }
        }

        public ContactList()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
