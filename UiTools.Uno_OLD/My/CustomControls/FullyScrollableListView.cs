using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace UiTools.Uno.My.CustomControls
{
    public partial class FullyScrollableListView : ListView
    {
        public FullyScrollableListView()
        {
            //leaving this on sometimes causes wrong ExtentHeight to be reported, especially when using End key to scroll to the end, hence incorrect scrolling.
            ShowsScrollingPlaceholders = false;

            SelectionChanged += (σ, ε) =>
            {
                ScrollListViewToSelectedItem(this);
            };
        }

        private void ScrollListViewToSelectedItem(ListView listView)
        {

            if (listView.SelectedItem == null) return;

            var scrollViewer = GetFirstChildOfType<ScrollViewer>(listView);
            if (scrollViewer == null) throw new Exception("Scroll viewer is null");


            var itemHeight = scrollViewer.ExtentHeight / listView.Items.Count;
            var selectedItemOffset = listView.SelectedIndex * itemHeight;
            var selectedItemVisible = selectedItemOffset > scrollViewer.VerticalOffset && selectedItemOffset < scrollViewer.VerticalOffset + scrollViewer.ViewportHeight - itemHeight;
            if (selectedItemVisible) return;

            var itemsInViewPort = (int)Math.Round(scrollViewer.ViewportHeight / itemHeight);
            var scrollableItems = listView.Items.Count - itemsInViewPort;
            if (scrollableItems <= 0) return;

            var selectedIndex = listView.SelectedIndex * 1d;

            //moving one line above the selected: this looks nicer and this also ensures that a newly added selecteditem is immediately visible
            //(it seems that newly added items are ignored by the scrollviewer until they are fully rendered (which is fuck knows when) so, technically,
            //we are scrtolling to a nearby item, not to the selected one. Adding extra line ensures visibility)
            if (selectedIndex < scrollableItems && selectedIndex > 1) selectedIndex -= 1;

            //can be >1 but this causes no problem: the view just scrolls to max
            var scrollPercentage = selectedIndex / scrollableItems;

            scrollViewer.ChangeView(0, scrollViewer.ScrollableHeight * scrollPercentage, null);
        }

        public static T? GetFirstChildOfType<T>(DependencyObject? depObj) where T : class
        {
            if (depObj == null) return default;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetFirstChildOfType<T>(child);
                if (result != null) return result;
            }

            return default;
        }
    }

}
