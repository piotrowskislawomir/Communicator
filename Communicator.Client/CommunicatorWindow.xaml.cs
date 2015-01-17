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
using Communicator.Protocol.Requests;
using Communicator.Queue.Services;
using Communicator.Untils;
//using Communicator.Untils.Serializers;

namespace Communicator.Client
{
    public sealed partial class CommunicatorWindow : ApplicationWindowBase
    {
        public CommunicatorWindow()
        {
            InitializeComponent();
        }
    }
}
