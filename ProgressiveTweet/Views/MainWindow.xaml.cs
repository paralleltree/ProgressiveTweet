using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ProgressiveTweet.ViewModels;

namespace ProgressiveTweet.Views
{
    /*
     * ViewModelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedWeakEventListenerや
     * CollectionChangedWeakEventListenerを使うと便利です。独自イベントの場合はLivetWeakEventListenerが使用できます。
     * クローズ時などに、LivetCompositeDisposableに格納した各種イベントリスナをDisposeする事でイベントハンドラの開放が容易に行えます。
     *
     * WeakEventListenerなので明示的に開放せずともメモリリークは起こしませんが、できる限り明示的に開放するようにしましょう。
     */

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard CompositeStoryboard;
        private DoubleAnimation TopAnimation;
        private DoubleAnimation LeftAnimation;
        private DoubleAnimation OpacityAnimation;

        private int TransitionCount = 0;
        private double DefaultTop;
        private double DefaultLeft;

        private bool IsCurrentRoot
        {
            get { return (DataContext as MainWindowViewModel).IsCurrentRoot; }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += (sender, e) => this.DragMove();

            // Animations
            double dr = 0.6;
            var du = TimeSpan.FromSeconds(0.4);
            CompositeStoryboard = new Storyboard();

            TopAnimation = new DoubleAnimation()
            {
                Duration = du,
                DecelerationRatio = dr,
                FillBehavior = FillBehavior.Stop
            };
            Storyboard.SetTarget(TopAnimation, this);
            Storyboard.SetTargetProperty(TopAnimation, new PropertyPath("Top"));
            CompositeStoryboard.Children.Add(TopAnimation);

            LeftAnimation = new DoubleAnimation()
            {
                Duration = du,
                DecelerationRatio = dr,
                FillBehavior = FillBehavior.Stop
            };
            Storyboard.SetTarget(LeftAnimation, this);
            Storyboard.SetTargetProperty(LeftAnimation, new PropertyPath("Left"));
            CompositeStoryboard.Children.Add(LeftAnimation);

            OpacityAnimation = new DoubleAnimation()
            {
                Duration = du,
                DecelerationRatio = dr,
                FillBehavior = FillBehavior.HoldEnd
            };
            Storyboard.SetTarget(OpacityAnimation, this);
            Storyboard.SetTargetProperty(OpacityAnimation, new PropertyPath("Opacity"));
            CompositeStoryboard.Children.Add(OpacityAnimation);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // @未来の自分
            // ここで何をしているのか分からなくなったら次をコメントアウトして実行してね☆
            //System.Diagnostics.Debug.WriteLine("IsCurrentRoot-> {0}", IsCurrentRoot);
            //System.Diagnostics.Debug.WriteLine("Content-> Width: {0:000}, Height: {1:000}", content.ActualWidth, content.ActualHeight);
            //System.Diagnostics.Debug.WriteLine("Border -> Width: {0:000}, Height: {1:000}", border.ActualWidth, border.ActualHeight);
            //System.Diagnostics.Debug.WriteLine("Window -> Width: {0:000}, Height: {1:000}", ActualWidth, ActualHeight);

            if (!IsCurrentRoot)
            {
                if (TransitionCount++ == 0)
                {
                    DefaultTop = Top;
                    DefaultLeft = Left;
                }
            }
            else
            {
                TransitionCount = 0;
            }

            TopAnimation.From = Top;
            TopAnimation.To = IsCurrentRoot ? DefaultTop : SystemParameters.WorkArea.Height / 2 - ActualHeight / 2;

            LeftAnimation.From = Left;
            LeftAnimation.To = IsCurrentRoot ? DefaultLeft : SystemParameters.WorkArea.Width / 2 - ActualWidth / 2;

            OpacityAnimation.To = IsCurrentRoot ? 0.6 : 0.9;
            CompositeStoryboard.Begin();
        }
    }
}
