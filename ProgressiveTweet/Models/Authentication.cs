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
    /// Twitterへの認証を行う一連のプロセスを提供します。
    /// </summary>
    public class Authentication : NotificationObject
    {
        internal static readonly string ConsumerKey = "zo9w1VUwzTlF6JkjTKPMy26Ec";
        internal static readonly string ConsumerSecret = "437TzsrsjfYNGGbZ71sU3YdvX3tHjlDXqVon7kPU6vRKYdno0a";


        /// <summary>
        /// 生成されたセッションを取得します。
        /// </summary>
        public OAuth.OAuthSession CurrentSession { get; private set; }

        /// <summary>
        /// 生成されたトークンを取得します。
        /// </summary>
        public Tokens GeneratedToken { get; private set; }

        /// <summary>
        /// 認証に用いるPINコードを取得、設定します。
        /// </summary>
        public string Pin
        {
            get { return _pin; }
            set
            {
                _pin = value;
                RaisePropertyChanged();
            }
        }
        private string _pin;


        /// <summary>
        /// セッションが生成されたかどうかを示す値を取得します。
        /// </summary>
        public bool IsSessionCreated
        {
            get { return _isSessionCreated; }
            private set
            {
                _isSessionCreated = value;
                RaisePropertyChanged();
            }
        }
        private bool _isSessionCreated;

        /// <summary>
        /// トークンが生成されたかどうかを示す値を取得します。
        /// </summary>
        public bool IsTokenGenerated
        {
            get { return _isTokenGenerated; }
            private set
            {
                _isTokenGenerated = value;
                RaisePropertyChanged();
            }
        }
        private bool _isTokenGenerated;


        /// <summary>
        /// OAuth認証を行うセッションを生成します。
        /// </summary>
        public void CreateSession()
        {
            if (IsSessionCreated) throw new InvalidOperationException("セッションは既に生成されています。");

            try
            {
                CurrentSession = OAuth.Authorize(ConsumerKey, ConsumerSecret);
            }
            catch
            {
                return;
            }
            IsSessionCreated = true;
        }

        /// <summary>
        /// 生成されたセッションに対し入力されたPINコードを用いてトークンを生成します。
        /// </summary>
        public void GenerateToken()
        {
            if (!IsSessionCreated) throw new InvalidOperationException("セッションが生成されていません。");

            try
            {
                GeneratedToken = CurrentSession.GetTokens(Pin);
            }
            catch
            {
                return;
            }
            IsTokenGenerated = true;
        }
    }
}
