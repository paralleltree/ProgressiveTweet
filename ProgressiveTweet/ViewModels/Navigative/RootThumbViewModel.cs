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
    public class RootThumbViewModel : NavigativeViewModel
    {
        private RootMenuViewModel RootMenu { get; set; }

        public RootThumbViewModel(NavigativeViewModel host)
            : base(host)
        {
            RootMenu = new RootMenuViewModel(this);
        }


        #region GoRootMenuCommand
        private ViewModelCommand _GoRootMenuCommand;

        public ViewModelCommand GoRootMenuCommand
        {
            get
            {
                if (_GoRootMenuCommand == null)
                {
                    _GoRootMenuCommand = new ViewModelCommand(GoRootMenu);
                }
                return _GoRootMenuCommand;
            }
        }

        public void GoRootMenu()
        {
            this.GoForward(RootMenu);
        }
        #endregion
    }
}
