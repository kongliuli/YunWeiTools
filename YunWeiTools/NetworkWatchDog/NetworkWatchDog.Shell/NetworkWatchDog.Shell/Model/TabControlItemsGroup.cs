using System.Collections.Generic;
using System.Windows.Controls;

namespace NetworkWatchDog.Shell.Model
{
    public class TabControlItemsGroup
    {
        public string GroupTitle
        {
            get; set;
        } = "defaut";
        public List<TabItem> Items
        {
            get; set;
        } = new();
    }

    public class TabItem
    {
        public string? Header
        {
            get; set;
        }

        public UserControl? Content
        {
            get; set;
        } = null;

        public StringStreamInfomation Info
        {
            get; set;
        } = new();
    }
}
