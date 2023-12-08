using System.Windows;
using System.Windows.Controls;

namespace NetworkWatchDog.littershell.Controls
{
    public class ScrollingListView:ListView
    {
        public static readonly DependencyProperty AutoRefreshProperty =
        DependencyProperty.Register("AutoRefresh",typeof(bool),typeof(ScrollingListView),new PropertyMetadata(false));

        public bool AutoRefresh
        {
            get
            {
                return (bool)GetValue(AutoRefreshProperty);
            }
            set
            {
                SetValue(AutoRefreshProperty,value);
            }
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(AutoRefresh)
            {
                if(e.NewItems==null)
                    return;
                var newItemCount = e.NewItems.Count;

                if(newItemCount>0)
                    this.ScrollIntoView(e.NewItems[newItemCount-1]);

                base.OnItemsChanged(e);
            }
        }
    }
}
