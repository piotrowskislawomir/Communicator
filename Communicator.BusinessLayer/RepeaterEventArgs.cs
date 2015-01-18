using Communicator.BusinessLayer.Enums;

namespace Communicator.BusinessLayer
{
    public class RepeaterEventArgs
    {
        public ActionTypes Type { get; set; }
        public bool Result { get; set; }
        public object Data { get; set; }
    }
}