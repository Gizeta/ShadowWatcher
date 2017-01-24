using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ShadowWatcher
{
    public class AmountToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return new SolidColorBrush(Colors.Black);
                case 2:
                    return new SolidColorBrush(Colors.Orange);
                default:
                    return new SolidColorBrush(Colors.Red);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
