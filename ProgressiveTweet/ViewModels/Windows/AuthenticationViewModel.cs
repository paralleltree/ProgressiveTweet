using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using ProgressiveTweet.Models;

namespace ProgressiveTweet.ViewModels
{
    public class AuthenticationViewModel : ViewModel
    {
        private Authentication Source { get; set; }

        public string Pin
        {
            get { return Source.Pin; }
            set
            {
                Source.Pin = value;
                AuthenticateCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsAuthenticating
        {
            get { return _isAuthenticating; }
            set
            {
                _isAuthenticating = value;
                AuthenticateCommand.RaiseCanExecuteChanged();
            }
        }
        private bool _isAuthenticating;


        public AuthenticationViewModel()
        {
            Source = new Authentication();
        }

        public void Initialize()
        {
            var listener = new PropertyChangedEventListener(Source,
                (sender, e) => RaisePropertyChanged(e.PropertyName));
            this.CompositeDisposable.Add(listener);

            Task.Run(() =>
            {
                Source.CreateSession();
                System.Diagnostics.Process.Start(Source.CurrentSession.AuthorizeUri.AbsoluteUri);
            });
        }


        #region AuthenticateCommand
        private ViewModelCommand _AuthenticateCommand;

        public ViewModelCommand AuthenticateCommand
        {
            get
            {
                if (_AuthenticateCommand == null)
                {
                    _AuthenticateCommand = new ViewModelCommand(Authenticate, CanAuthenticate);
                }
                return _AuthenticateCommand;
            }
        }

        public bool CanAuthenticate()
        {
            return !string.IsNullOrWhiteSpace(Pin) && !IsAuthenticating;
        }

        public void Authenticate()
        {
            IsAuthenticating = true;
            Task.Run(() =>
            {
                Source.GenerateToken();

                if (Source.IsTokenGenerated)
                {
                    Model.Instance.CurrentToken = Source.GeneratedToken;
                    Messenger.Raise(new WindowActionMessage("Close"));
                }
                else
                {
                    Messenger.Raise(new InformationMessage()
                    {
                        MessageKey = "Information",
                        Text = "認証できませんでした。\n再試行してください。",
                        Image = System.Windows.MessageBoxImage.Error
                    });
                }
                IsAuthenticating = false;
            });
        }
        #endregion
    }
}
