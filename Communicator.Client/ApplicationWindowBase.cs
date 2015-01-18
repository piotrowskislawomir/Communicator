using System.Windows;
using System.Windows.Input;

namespace Communicator.Client
{
    public class ApplicationWindowBase : Window
    {
        public void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        public void Button_WinClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Button_WinMin_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}