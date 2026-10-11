using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KindergartenCRUD.Converters
{
    public class RemainingToBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is decimal remaining)
                return remaining > 0 ? Brushes.Crimson : Brushes.SeaGreen;
            return Brushes.Black;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
