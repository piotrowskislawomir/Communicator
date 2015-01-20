using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Communicator.BusinessLayer;
using Communicator.BusinessLayer.Enums;
using Communicator.BusinessLayer.Interfaces;
using Communicator.Client.Helpers;
using Communicator.Client.Models;
using Communicator.Protocol.Enums;
using Communicator.Protocol.Model;
using Communicator.Protocol.Notifications;
using Communicator.Protocol.Requests;
using Microsoft.Practices.Prism.Commands;

namespace Communicator.Client.ViewModels
{
    public class CommunicatorViewModel : ViewModelBase
    {
        private const string ImageOnline = "../UI/statusGreen.jpg";
        private const string ImageAfk = "../UI/statusYellow.jpg";
        private const string ImageOffline = "../UI/statusRed.jpg";
        private readonly ILogicClient _logicClient;
        private PresenceStatus _selectedStatus;

        public CommunicatorViewModel(ILogicClient logicClient)
        {
            _logicClient = logicClient;
            _logicClient.Repeater += ProceedCommand;
            Contacts = new ObservableCollection<ContactViewModel>();
            ConversationWindows = new List<ConversationViewModel>();
            Statuses = new ObservableCollection<PresenceStatus>
            {
                PresenceStatus.Online,
                PresenceStatus.Afk,
                PresenceStatus.Offline
            };
            Status = PresenceStatus.Online;
            SelectedStatus = PresenceStatus.Online;
            InitTimer();
        }

        public PresenceStatus Status { get; set; }
        public ObservableCollection<ContactViewModel> Contacts { get; set; }
        private List<ConversationViewModel> ConversationWindows { get; set; }

        public ObservableCollection<PresenceStatus> Statuses { get; set; }


        public PresenceStatus SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                OnPropertyChanged();
                ChangeStatusAction(_selectedStatus);
            }
        }

        public ICommand HistoryCommand
        {
            get { return new DelegateCommand(HistoryAction); }
        }

        public ICommand CloseCommand
        {
            get { return new DelegateCommand(CloseAction); }
        }

        public ICommand LogoutCommand
        {
            get { return new DelegateCommand(LogoutAction); }
        }


        public ICommand ContactCommand
        {
            get { return new DelegateCommand<string>(ContactAction); }
        }

        private void ChangeStatusAction(PresenceStatus status)
        {
            if (status != Status)
            {
                Status = status;
                _logicClient.SendPing(Status);
            }
        }

        public event EventHandler OnRequestClose;

        private void ContactAction(string login)
        {
            if (!ConversationWindows.Any(cmv => cmv.Recipeint == login))
            {
                var conversationWindow = new ConversationWindow();
                var conversationViewModel = new ConversationViewModel(_logicClient);
                conversationViewModel.OnRequestClose += (s, ee) =>
                {
                    conversationWindow.Close();
                    DeleteFromConversationList(conversationViewModel.Recipeint);
                };
                conversationViewModel.Initialize(login);
                conversationWindow.DataContext = conversationViewModel;
                conversationWindow.Show();

                ConversationWindows.Add(conversationViewModel);
            }
        }

        private void InitTimer()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 30);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            _logicClient.SendPing(Status);
        }


        private void HistoryAction()
        {
            var historyWindow = new HistoryWindow();
            var historyViewModel = new HistoryViewModel(_logicClient);
            historyViewModel.OnRequestClose += (s, e) => historyWindow.Close();
            historyWindow.DataContext = historyViewModel;
            historyWindow.Show();
        }


        private void CloseAction()
        {
            _logicClient.SendPing(PresenceStatus.Offline);
            if (OnRequestClose != null)
            {
                OnRequestClose(this, new EventArgs());
            }
        }

        private void LogoutAction()
        {
            Status = PresenceStatus.Offline;
            _logicClient.SendPing(Status);


            if (OnRequestClose != null)
            {
                OnRequestClose(this, new EventArgs());
            }

            
        }

        public void Inicialize()
        {
            _logicClient.GetUserList();
        }


        private string getStatusImage(PresenceStatus status)
        {
            switch (status)
            {
                case PresenceStatus.Online:
                {
                    return ImageOnline;
                }
                case PresenceStatus.Afk:
                {
                    return ImageAfk;
                }
            }

            return ImageOffline;
        }

        public void ProceedCommand(object sender, RepeaterEventArgs e)
        {
            if (e.Type == ActionTypes.ContactList)
            {
                if (e.Result)
                {
                    var users = (List<User>) e.Data;

                    DispatchService.Invoke(() =>
                    {
                        Contacts.Clear();
                        users.ForEach(u =>
                        {
                            var contactViewModel =
                                new ContactViewModel(new ContactModel
                                {
                                    Login = u.Login,
                                    Status = u.Status,
                                    StatusImageUri = getStatusImage(u.Status)
                                });
                            Contacts.Add(contactViewModel);
                        });
                    });
                }
            }
            else if (e.Type == ActionTypes.Message)
            {
                if (e.Result)
                {
                    var messageReq = (MessageReq) e.Data;
                    if (!ConversationWindows.Any(cmv => cmv.Recipeint == messageReq.Login))
                    {
                        DispatchService.Invoke(() =>
                        {
                            var conversationWindow = new ConversationWindow();
                            var conversationViewModel = new ConversationViewModel(_logicClient);
                            conversationViewModel.OnRequestClose += (s, ee) =>
                            {
                                conversationWindow.Close();
                                DeleteFromConversationList(conversationViewModel.Recipeint);
                            };

                            var messageModel = new MessageModel
                            {
                                DateTimeDelivery = messageReq.SendTime.ToString("yy-MM-dd hh:dd"),
                                Message = messageReq.Message,
                                Sender = messageReq.Login,
                                Image = GetImageFromAttach(messageReq.Attachment)
                            };
                            conversationViewModel.Initialize(messageReq.Login);
                            conversationViewModel.AddMessage(messageModel);
                            conversationWindow.DataContext = conversationViewModel;
                            conversationWindow.Show();

                            ConversationWindows.Add(conversationViewModel);
                        });
                    }
                }
            }
            else if (e.Type == ActionTypes.PresenceNotification)
            {
                var presenceNotification = (PresenceStatusNotification) e.Data;

                ContactViewModel contact = Contacts.SingleOrDefault(c => c.Login == presenceNotification.Login);
                if (contact != null)
                {
                    contact.Status = presenceNotification.PresenceStatus;
                    contact.StatusImageUri = getStatusImage(presenceNotification.PresenceStatus);
                }
            }
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

                string fileName = Guid.NewGuid() + ".jpg";
                using (var fs = new FileStream(directory + fileName, FileMode.CreateNew))
                {
                    fs.Write(attachment.Data, 0, attachment.Data.Length);
                }
                return directory + fileName;
            }

            return DependencyProperty.UnsetValue;
        }

        private void DeleteFromConversationList(string login)
        {
            ConversationViewModel conversation = ConversationWindows.SingleOrDefault(cw => cw.Recipeint == login);
            if (conversation != null)
            {
                ConversationWindows.Remove(conversation);
            }
        }
    }
}