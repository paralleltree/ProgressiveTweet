using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public class RootMenuViewModel : NavigativeViewModel
    {
        private Model Model { get; set; }
        private TweetCreationViewModel TweetCreation { get; set; }
        private SettingMenuViewModel SettingMenu { get; set; }

        public RootMenuViewModel(NavigativeViewModel host)
            : base(host)
        {
            this.Model = Model.Instance;
            SettingMenu = new SettingMenuViewModel(this);
        }


        #region OpenTweetCreationWindowCommand
        private ViewModelCommand _OpenTweetCreationWindowCommand;

        public ViewModelCommand OpenTweetCreationWindowCommand
        {
            get
            {
                if (_OpenTweetCreationWindowCommand == null)
                {
                    _OpenTweetCreationWindowCommand = new ViewModelCommand(OpenTweetCreationWindow, CanOpenTweetCreationWindow);
                }
                return _OpenTweetCreationWindowCommand;
            }
        }

        public bool CanOpenTweetCreationWindow()
        {
            return Model.CurrentToken != null;
        }

        public void OpenTweetCreationWindow()
        {
            if (TweetCreation == null || TweetCreation.IsSent)
                TweetCreation = new TweetCreationViewModel();

            Messenger.Raise(new TransitionMessage()
            {
                MessageKey = "OpenTweetCreationWindow",
                TransitionViewModel = TweetCreation
            });
        }
        #endregion

        #region GoSettingMenuCommand
        private ViewModelCommand _GoSettingMenuCommand;

        public ViewModelCommand GoSettingMenuCommand
        {
            get
            {
                if (_GoSettingMenuCommand == null)
                {
                    _GoSettingMenuCommand = new ViewModelCommand(GoSettingMenu);
                }
                return _GoSettingMenuCommand;
            }
        }

        public void GoSettingMenu()
        {
            this.GoForward(SettingMenu);
        }
        #endregion

        #region GoBackCommand
        private ViewModelCommand _GoBackCommand;

        public ViewModelCommand GoBackCommand
        {
            get
            {
                if (_GoBackCommand == null)
                {
                    _GoBackCommand = new ViewModelCommand(base.GoBack);
                }
                return _GoBackCommand;
            }
        }

        // public void GoBack() { } // already defined in base
        #endregion
    }
}
