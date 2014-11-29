using System.Windows.Input;
using System.Windows.Media;

namespace Communicator.Ribbon.Demo {
    public sealed class ColorContext : NotificationObject {

        public ColorContext() {
            ChangeColorCommand = new ChangeColorCommand(this);
        }
        
        public SolidColorBrush Brush { get; set; }
        public ICommand ChangeColorCommand { get; set; }
    }
}
