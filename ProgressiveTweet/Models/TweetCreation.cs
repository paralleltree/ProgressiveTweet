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
            }
        }
        private string _text;

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


        /// <summary>
        /// ツイートを投稿します。
        /// </summary>
        public void Tweet()
        {
            try
            {
                Model.Instance.CurrentToken.Statuses.Update(status => Text);
            }
            catch
            {
                return;
            }
            IsSent = true;
        }
    }
}
