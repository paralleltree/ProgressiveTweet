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
    public class SettingMenuViewModel : NavigativeViewModel
    {
        public SettingMenuViewModel(NavigativeViewModel host)
            : base(host)
        {
        }


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
        #endregion
    }
}
