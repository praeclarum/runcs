using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace RunCSharp.Controls
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentException("Cannot convert a null value to a visibility!");
            var sourceType = value.GetType();
            if (!(sourceType.Equals(typeof(bool)) && targetType.Equals(typeof(Visibility))))
                throw new ArgumentException(string.Format("Cannot convert type '{0}' to '{1}'. Only can convert boolean to Visibility.", sourceType.Name, targetType.Name));
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) throw new ArgumentException("Cannot back convert a null value to a boolean!");
            var sourceType = value.GetType();
            if (!(sourceType.Equals(typeof(Visibility)) && targetType.Equals(typeof(bool))))
                throw new ArgumentException(string.Format("Cannot convert type '{0}' to '{1}'. Only can convert Visibility to boolean.", sourceType.Name, targetType.Name));
            return (Visibility)value == Visibility.Visible;
        }
    }
}
