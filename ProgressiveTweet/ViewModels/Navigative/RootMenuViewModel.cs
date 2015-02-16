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

        public RootMenuViewModel(NavigativeViewModel host)
            : base(host)
        {
            this.Model = Model.Instance;
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

        // public void GoBack() { } // already defined in base
        #endregion
    }
}
