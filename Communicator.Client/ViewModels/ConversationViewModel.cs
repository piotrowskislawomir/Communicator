using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Client.Annotations;
using Communicator.Client.Helpers;
using Communicator.Client.Models;
using Communicator.Protocol.Model;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Requests;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Win32;

namespace Communicator.Client.ViewModels
{
    public class ConversationViewModel : ViewModelBase
    {
        private readonly ILogicClient _logicClient;
        public event EventHandler OnRequestClose;

        public ObservableCollection<MessageModel> Messages { get; set; }

        public string Recipeint { get; set; }
        public DateTime LastSendWritingNotification { get; set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
                MessageInputAction();
            }
        }

        private string _userWriting;
        public string UserWriting
        {
            get { return _userWriting; }
            set
            {
                _userWriting = value;
                OnPropertyChanged();
            }
        }


        public byte[] ImageData { get; set; }
        public string ImagePath { get; set; }

        public ICommand SendCommand
        {
            get
            {
                return new DelegateCommand(SendAction);
            }
        }
        public ICommand AttachImageCommand
        {
            get
            {
                return new DelegateCommand(AttachImageAction);
            }
        }
        
        private void MessageInputAction()
        {
            if (LastSendWritingNotification < DateTime.Now.AddSeconds(-2))
            {
                _logicClient.SendUserWriting(Recipeint);
                LastSendWritingNotification = DateTime.Now;
            }
        }

        private void AttachImageAction()
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                using (var fileStream = new FileStream(openFileDialog.FileName, FileMode.Open))
                {
                    ImagePath = openFileDialog.FileName;
                    ImageData = new byte[fileStream.Length];
                    fileStream.Read(ImageData, 0, ImageData.Length);
                }
            }
        }

        private void SendAction()
        {
            _logicClient.SendMessage(Recipeint, Message, ImageData);
            var messageModel = new MessageModel
            {
                DateTimeDelivery = DateTimeOffset.Now.ToString("yy-MM-dd hh:mm"),
                Message = Message,
                Sender = "Ja",
                Image = ImagePath
            };
            Messages.Add(messageModel);
            Message = string.Empty;
            ImageData = null;
            ImagePath = null;
        }

        public ICommand CloseCommand
        {
            get
            {
                return new DelegateCommand(CloseAction);
            }
        }

        private void CloseAction()
        {
            if (OnRequestClose != null)
            {
                OnRequestClose(this, new EventArgs());
            }
        }

        public ConversationViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            _logicClient.Repeater += ProceedCommand;
            Messages = new ObservableCollection<MessageModel>();
            LastSendWritingNotification = DateTime.Now;
        }

        public void Initialize(string login)
        {
            Recipeint = login;
        }

        public void AddMessage(MessageModel message)
        {
            Messages.Add(message);
        }

        private object GetImageFromAttach(Attachment attachment)
        {
            if (attachment != null)
            {
                string directory = Environment.CurrentDirectory + "\\data\\";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                var fileName = Guid.NewGuid() + ".jpg";
                using (var fs = new FileStream(directory + fileName, FileMode.CreateNew))
                {
                    fs.Write(attachment.Data, 0, attachment.Data.Length);
                }
                return directory + fileName;
            }

            return DependencyProperty.UnsetValue;
        }

        public void ProceedCommand(object sender, RepeaterEventArgs e)
        {
            if (e.Type == ActionTypes.Message)
            {
                if (e.Result)
                {
                    var message = (MessageReq)e.Data;
                    if (message.Login == Recipeint)
                    {
                        DispatchService.Invoke(() =>
                        {
                        var messageModel = new MessageModel
                        {
                            DateTimeDelivery = message.SendTime.ToString("yy-MM-dd hh:mm"),
                            Message = message.Message,
                            Sender = message.Login,
                            Image = GetImageFromAttach(message.Attachment)
                        };
                       
                            Messages.Add(messageModel);
                        });
                    }
                }
                
            }
            else if (e.Type == ActionTypes.UserWriting)
            {
                if (e.Result)
                {
                    var notification = (ActivityNotification)e.Data;
                    if (notification.Sender == Recipeint)
                    {
                        Task.Factory.StartNew(() =>
                        {
                            DispatchService.Invoke(() =>
                            {
                                UserWriting = notification.IsWriting ? "Użytkownik pisze..." : "";
                            });

                                Thread.Sleep(3000);
                                DispatchService.Invoke(() =>
                                {
                                    UserWriting = "";
                                });
                                
                            
                        });

                    }
                }
            }
        }

    }
}
