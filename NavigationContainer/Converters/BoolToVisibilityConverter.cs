using System.Windows.Data;
using System.Windows;
using System.Globalization;
using System;

namespace NavigationContainer.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public BoolToVisibilityConverter()
        {
            FalseEquivalent = Visibility.Collapsed;
        }

        /// <summary>
        /// FalseEquivalent (default : Visibility.Collapsed)
        /// </summary>
        /// <value>The false equivalent.</value>
        public Visibility FalseEquivalent { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var bValue = (bool?)value;

            if (parameter is bool)
            {
                if ((bool)parameter)
                {
                    bValue = !bValue;
                }
            }

            return bValue.GetValueOrDefault() ? (Visibility.Visible) : FalseEquivalent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vValue = ((Visibility)value);
            if (vValue == Visibility.Hidden)
            {
                vValue = Visibility.Collapsed;
            }

            if (parameter is bool)
            {
                if ((bool)parameter)
                {
                    vValue = vValue == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                }
            }
            return (vValue == Visibility.Visible);
        }
    }

}
