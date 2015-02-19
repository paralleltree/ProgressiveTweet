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
    /// <summary>
    /// 差し替えられることで画面遷移を行うViewModelの基底クラスです。
    /// </summary>
    public class NavigativeViewModel : ViewModel
    {
        /// <summary>
        /// ナビゲートを委譲するViewModelを格納します。
        /// </summary>
        private NavigativeViewModel HostViewModel { get; set; }

        /// <summary>
        /// このインスタンスがナビゲーションのルートであるViewModelかどうかを示す値を取得します。
        /// </summary>
        public bool IsRoot { get { return HostViewModel == null; } }


        /// <summary>
        /// ルートとなるNavigationViewModelの新しいインスタンスを初期化します。
        /// </summary>
        public NavigativeViewModel()
        {
        }

        /// <summary>
        /// 指定のViewModelでNavigationViewModelの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="root">ナビゲートを委譲するViewModel</param>
        public NavigativeViewModel(NavigativeViewModel host)
        {
            HostViewModel = host;
        }


        /// <summary>
        /// 履歴の最新の項目へ移動します。
        /// </summary>
        public virtual void GoBack()
        {
            if (IsRoot) throw new InvalidOperationException("ナビゲーションのルートに対して戻る操作を行おうとしました。");
            HostViewModel.GoBack();
        }

        /// <summary>
        /// 指定の項目へ移動します。
        /// </summary>
        /// <param name="dest">移動先のViewModel</param>
        public virtual void GoForward(NavigativeViewModel dest)
        {
            HostViewModel.GoForward(dest);
        }
    }
}
