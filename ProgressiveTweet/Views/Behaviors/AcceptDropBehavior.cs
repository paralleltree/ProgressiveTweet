using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace ProgressiveTweet.Views.Behaviors
{
    /// <summary>
    /// <see cref="System.Windows.FrameworkElement"/>へのドロップの受け入れを制御します。
    /// </summary>
    class AcceptDropBehavior : Behavior<FrameworkElement>
    {
        /// <summary>
        /// ドロップ制御を記述する<see cref="ProgressiveTweet.Views.Behaviors.DragDropDescription"/>を格納します。
        /// </summary>
        public DragDropDescription Description
        {
            get { return (DragDropDescription)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(DragDropDescription), typeof(AcceptDropBehavior), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewDragOver += OnDragOver;
            this.AssociatedObject.PreviewDrop += OnDrop;
        }


        private void OnDragOver(object sender, DragEventArgs e)
        {
            if (Description == null)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            Description.OnDragOver(e);
            e.Handled = true;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            if (Description == null)
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
                return;
            }
            Description.OnDrop(e);
            e.Handled = true;
        }
    }

    /// <summary>
    /// ドロップの受け入れ制御を記述します。
    /// </summary>
    public class DragDropDescription
    {
        public event Action<DragEventArgs> DragOver;
        public event Action<DragEventArgs> Drop;

        public void OnDragOver(DragEventArgs e)
        {
            if (DragOver != null) DragOver(e);
        }

        public void OnDrop(DragEventArgs e)
        {
            var handler = Drop;
            if (Drop != null) Drop(e);
        }
    }
}
