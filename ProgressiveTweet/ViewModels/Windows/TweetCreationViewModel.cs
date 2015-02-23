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
using ProgressiveTweet.Views.Behaviors;

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

        public ReadOnlyDispatcherCollection<System.IO.Stream> Media { get; private set; }

        public bool HasMedia
        {
            get { return Source.HasMedia; }
        }

        public int RemainingLength
        {
            get { return Source.RemainingLength; }
        }

        public bool IsSent
        {
            get { return Source.IsSent; }
        }

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

        /// <summary>
        /// Viewでのドラッグドロップを制御するDescriptionを格納します。
        /// </summary>
        public DragDropDescription DragDropDescription { get; private set; }


        public TweetCreationViewModel()
        {
            Source = new TweetCreation() { Text = "" };
            this.CompositeDisposable.Add(new PropertyChangedEventListener(Source,
                (sender, e) =>
                {
                    switch (e.PropertyName)
                    {
                        case "IsValid":
                            TweetCommand.RaiseCanExecuteChanged();
                            return;
                    }
                    RaisePropertyChanged(e.PropertyName);
                }));
            Media = new ReadOnlyDispatcherCollection<System.IO.Stream>(Source.Media);

            DragDropDescription = new DragDropDescription();
            DragDropDescription.DragOver += e =>
            {
                if (!e.AllowedEffects.HasFlag(System.Windows.DragDropEffects.Copy)) return;

                if (e.Data.GetDataPresent(System.Windows.DataFormats.FileDrop, true))
                    e.Effects = System.Windows.DragDropEffects.Copy;
                else
                    e.Effects = System.Windows.DragDropEffects.None;

                e.Handled = true;
            };
            DragDropDescription.Drop += e =>
            {
                var data = e.Data.GetData(System.Windows.DataFormats.FileDrop) as IEnumerable<string>;
                var files = data.Where(p => System.Text.RegularExpressions.Regex.IsMatch(p,
                    @"\.(png|jpe?g|gif)\Z",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase));

                foreach (string path in files)
                    Source.Media.Add(new System.IO.MemoryStream(System.IO.File.ReadAllBytes(path)));
            };
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
            return Source.IsValid && !IsSending;
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

        #region PasteFromClipboardCommand
        private ViewModelCommand _PasteFromClipboardCommand;

        public ViewModelCommand PasteFromClipboardCommand
        {
            get
            {
                if (_PasteFromClipboardCommand == null)
                {
                    _PasteFromClipboardCommand = new ViewModelCommand(PasteFromClipboard);
                }
                return _PasteFromClipboardCommand;
            }
        }

        public void PasteFromClipboard()
        {
            if (!System.Windows.Clipboard.ContainsImage()) return;

            var image = System.Windows.Clipboard.GetImage();
            var encoder = new System.Windows.Media.Imaging.BmpBitmapEncoder(); // PngBitmapEncoder doesn't work
            encoder.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(image));

            // convert to png
            using (var source = new System.IO.MemoryStream())
            {
                encoder.Save(source);

                using (var bmp = new System.Drawing.Bitmap(source))
                {
                    var stream = new System.IO.MemoryStream();
                    bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                    Source.Media.Add(stream);
                }
            }
        }
        #endregion

        #region RemoveMediaCommand
        private ListenerCommand<System.IO.Stream> _RemoveMediaCommand;

        public ListenerCommand<System.IO.Stream> RemoveMediaCommand
        {
            get
            {
                if (_RemoveMediaCommand == null)
                {
                    _RemoveMediaCommand = new ListenerCommand<System.IO.Stream>(RemoveMedia, CanRemoveMedia);
                }
                return _RemoveMediaCommand;
            }
        }

        public bool CanRemoveMedia()
        {
            return Source.HasMedia;
        }

        public void RemoveMedia(System.IO.Stream parameter)
        {
            Source.Media.Remove(parameter);
        }
        #endregion
    }
}
