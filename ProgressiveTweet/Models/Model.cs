using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Reflection;

using Livet;
using CoreTweet;
using Newtonsoft.Json;

namespace ProgressiveTweet.Models
{
    public class Model : NotificationObject, IDisposable
    {
        private static readonly string TokenPath = "token.json";
        private Model()
        {
            // Load settings
            if (File.Exists(TokenPath))
            {
                var dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(TokenPath));
                CurrentToken = Tokens.Create(Authentication.ConsumerKey, Authentication.ConsumerSecret, dic["AccessToken"], dic["AccessTokenSecret"]);
            }
        }

        public void Dispose()
        {
            // Save settings
            if (CurrentToken != null)
            {
                File.WriteAllText(TokenPath, JsonConvert.SerializeObject(new Dictionary<string, string>()
                {
                    { "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString() },
                    { "AccessToken", CurrentToken.AccessToken },
                    { "AccessTokenSecret", CurrentToken.AccessTokenSecret }
                }));
            }
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
