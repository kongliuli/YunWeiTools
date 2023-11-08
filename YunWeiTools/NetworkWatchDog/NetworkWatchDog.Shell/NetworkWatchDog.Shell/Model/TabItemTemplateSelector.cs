using System.Windows;
using System.Windows.Controls;

namespace NetworkWatchDog.Shell.Model
{
    public class TabItemTemplateSelector:DataTemplateSelector
    {
        public DataTemplate Template1
        {
            get; set;
        } = new DataTemplate();
        public DataTemplate Template2
        {
            get; set;
        } = new DataTemplate();

        public override DataTemplate SelectTemplate(object item,DependencyObject container)
        {
            if(item is string header)
            {
                if(header=="Ip监听")
                {
                    return Template1;
                }
                else if(header=="Tab2")
                {
                    return Template2;
                }
            }
            return base.SelectTemplate(item,container);
        }
    }

}