using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Livet;
using CoreTweet;

namespace ProgressiveTweet.Models
{
    /// <summary>
    /// ツイートを投稿するプロセスを提供します。
    /// </summary>
    public class TweetCreation : NotificationObject
    {
        /// <summary>
        /// 投稿するテキストを取得、設定します。
        /// </summary>
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValid");
                RaisePropertyChanged("RemainingLength");
            }
        }
        private string _text;

        /// <summary>
        /// 投稿する画像を取得、設定します。
        /// </summary>
        public DispatcherCollection<System.IO.Stream> Media { get; set; }

        /// <summary>
        /// 投稿するツイートにメディアが含まれるかどうかを示す値を取得します。
        /// </summary>
        public bool HasMedia { get { return Media.Count > 0; } }

        /// <summary>
        /// 入力可能な残り文字数を取得します。
        /// </summary>
        public int RemainingLength
        {
            get { return TwitterText.GetRemainingTweetLength(Text, HasMedia); }
        }

        /// <summary>
        /// 現在のツイートが有効であるかどうかを示す値を取得します。
        /// </summary>
        public bool IsValid
        {
            get { return TwitterText.IsValidTweet(Text, HasMedia); }
        }

        /// <summary>
        /// ツイートが送信されたかどうかを示す値を取得します。
        /// </summary>
        public bool IsSent
        {
            get { return _isSent; }
            private set
            {
                _isSent = value;
                RaisePropertyChanged();
            }
        }
        private bool _isSent;


        public TweetCreation()
        {
            Media = new DispatcherCollection<System.IO.Stream>(DispatcherHelper.UIDispatcher);
            Media.CollectionChanged += (sender, e) =>
            {
                RaisePropertyChanged("IsValid");
                RaisePropertyChanged("HasMedia");
                RaisePropertyChanged("RemainingLength");
            };
        }


        /// <summary>
        /// ツイートを投稿します。
        /// </summary>
        public void Tweet()
        {
            try
            {
                var token = Model.Instance.CurrentToken;
                if (HasMedia)
                {
                    foreach (var stream in Media) stream.Seek(0, System.IO.SeekOrigin.Begin); // sent data will be 0 byte without this
                    var response = Media.Select(p => token.Media.Upload(media => p));
                    token.Statuses.Update(status => Text, media_ids => response.Select(p => p.MediaId));
                }
                else
                {
                    token.Statuses.Update(status => Text);
                }
            }
            catch
            {
                return;
            }
            IsSent = true;
        }
    }
}
