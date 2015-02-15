using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Livet;
using CoreTweet;

namespace ProgressiveTweet.Models
{
    public class Model : NotificationObject
    {
        private Model()
        {
        }

        #region Singleton
        private static Model _instance;
        public static Model Instance
        {
            get { return _instance ?? (_instance = new Model()); }
        }
        #endregion

        /*
         * NotificationObjectはプロパティ変更通知の仕組みを実装したオブジェクトです。
         */

        /// <summary>
        /// Twitterへのアクセストークンを取得、設定します。
        /// </summary>
        public Tokens CurrentToken { get; set; }
    }
}
