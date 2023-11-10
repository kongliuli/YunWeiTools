using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace DbReaderDemo.Shell.Model
{
    public class RowToIndexConverter:IValueConverter
    {
        public object Convert(object value,Type targetType,object parameter,CultureInfo culture)
        {
            DataGridRow row = value as DataGridRow;
            if(row!=null)
            {
                return row.GetIndex()+1;
            }
            return null;
        }

        public object ConvertBack(object value,Type targetType,object parameter,CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
