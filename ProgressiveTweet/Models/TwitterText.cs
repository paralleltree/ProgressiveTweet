using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

using CoreTweet;

namespace ProgressiveTweet.Models
{
    /// <summary>
    /// ツイートについての情報を返す静的メソッドを提供します。
    /// </summary>
    public static class TwitterText
    {
        public const int MAX_TWEET_LENGTH = 140;
        public static int ShortUrlLength = 22;
        public static int ShortUrlLengthHttps = 23;
        public static int CharactersReservedPerMedia = 23;

        /// <summary>
        /// テキストがツイートとして有効であるかどうかを示す値を返します。
        /// </summary>
        /// <param name="text">判定するテキスト</param>
        /// <param name="hasMedia">メディアを含むかどうかを示すbool値</param>
        /// <returns>有効なツイートであるかどうかを示すbool値</returns>
        public static bool IsValidTweet(string text, bool hasMedia)
        {
            if ((text == null || text.Length == 0) && !hasMedia) return false;
            return GetTweetLength(text, hasMedia) <= MAX_TWEET_LENGTH;
        }

        /// <summary>
        /// ツイートとしてのテキストの文字数を返します。
        /// </summary>
        /// <param name="text">文字数を計測するテキスト</param>
        /// <param name="hasMedia">メディアを含むかどうかを示すbool値</param>
        /// <returns>ツイートとしての文字数</returns>
        public static int GetTweetLength(string text, bool hasMedia)
        {
            int length = new StringInfo(text.Replace("\r", "")).LengthInTextElements;

            foreach (Match m in Regex.Matches(text, URL_PATTERN, RegexOptions.CultureInvariant))
            {
                length -= m.Length;
                length += m.Value.ToLower().StartsWith("https://") ? ShortUrlLengthHttps : ShortUrlLength;
            }

            if (hasMedia) length += CharactersReservedPerMedia;
            return length;
        }

        /// <summary>
        /// ツイートとしてのテキストに対して入力可能な残り文字数を返します。
        /// </summary>
        /// <param name="text">残り文字数を計測するテキスト</param>
        /// <param name="hasMedia">メディアを含むかどうかを示すbool値</param>
        /// <returns>入力可能な残り文字数</returns>
        public static int GetRemainingTweetLength(string text, bool hasMedia)
        {
            return MAX_TWEET_LENGTH - GetTweetLength(text, hasMedia);
        }


        #region Regex Pattern Definitions
        const string URL_CHARS = @"\w*%#!()~\'-";
        const string URL_DOMAIN = "[" + URL_CHARS + @"]+\.";
        const string URL_DOMAIN_ENDING = "[" + URL_CHARS + "]+";

        const string URL_PORT = ":[0-9]+";
        const string URL_PATH = @"\/[" + URL_CHARS + ".]+";
        const string URL_QUERY_PARAM = URL_DOMAIN_ENDING + "=" + URL_DOMAIN_ENDING; // ガバガバ
        const string URL_QUERY = @"\?" + URL_QUERY_PARAM + "(&" + URL_QUERY_PARAM + ")*";
        const string URL_HASH = @"#[!\w]+";

        const string URL_PATTERN = "(https?://)?" +
            "(" + URL_DOMAIN + ")+" + URL_DOMAIN_ENDING +
            "(" + URL_PORT + ")?" +
            "(" + URL_PATH + ")*" +
            "(" + URL_QUERY + ")?" +
            "(" + URL_HASH + ")?";
        #endregion
    }
}
