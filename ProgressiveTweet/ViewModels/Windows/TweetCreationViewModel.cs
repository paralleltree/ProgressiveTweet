using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using ProgressiveTweet.Models;

namespace ProgressiveTweet.ViewModels
{
    public class TweetCreationViewModel : ViewModel
    {
        private TweetCreation Source { get; set; }

        public string Text
        {
            get { return Source.Text; }
            set { Source.Text = value; }
        }

        public int RemainingCount
        {
            get { return 140 - Text.Length; }
        }

        public bool IsSent { get { return Source.IsSent; } }

        public bool IsSending
        {
            get { return _isSending; }
            private set
            {
                _isSending = value;
                RaisePropertyChanged();
                TweetCommand.RaiseCanExecuteChanged();
            }
        }
        private bool _isSending;


        public TweetCreationViewModel()
        {
            Source = new TweetCreation() { Text = "" };
            this.CompositeDisposable.Add(new PropertyChangedEventListener(Source,
                (sender, e) =>
                {
                    switch (e.PropertyName)
                    {
                        case "Text":
                            RaisePropertyChanged("Text");
                            RaisePropertyChanged("RemainingCount");
                            TweetCommand.RaiseCanExecuteChanged();
                            return;
                    }
                    RaisePropertyChanged(e.PropertyName);
                }));
        }


        #region TweetCommand
        private ViewModelCommand _TweetCommand;

        public ViewModelCommand TweetCommand
        {
            get
            {
                if (_TweetCommand == null)
                {
                    _TweetCommand = new ViewModelCommand(Tweet, CanTweet);
                }
                return _TweetCommand;
            }
        }

        public bool CanTweet()
        {
            return RemainingCount < 140 && RemainingCount >= 0 && !IsSending;
        }

        public void Tweet()
        {
            IsSending = true;
            Task.Run(() =>
            {
                Source.Tweet();

                if (Source.IsSent)
                {
                    Messenger.Raise(new WindowActionMessage("Close"));
                }
                else
                {
                    Messenger.Raise(new InformationMessage()
                    {
                        MessageKey = "Information",
                        Text = "ツイートの送信に失敗しました。",
                        Image = System.Windows.MessageBoxImage.Error
                    });
                }
                IsSending = false;
            });
        }
        #endregion
    }
}
