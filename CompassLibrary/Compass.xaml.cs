using System;
using System.Linq;
using System.Windows.Data;

namespace CompassLibrary
{
    /// <summary>
    /// Interaction logic for Compass.xaml
    /// </summary>
    public partial class Compass
    {
        public Compass()
        {
            InitializeComponent();
        }
    }

    public class CompassValuesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length < 4)
                return string.Empty;
            
            var latitude = (float)values[0];
            var absLatitude = Math.Abs(latitude);
            var latDegrees = Math.Truncate(absLatitude / 100);
            var latResidue = absLatitude % 100;

            var directionNs = (char) values[1];
            
            var longitude = (float)values[2];
            var absLongitude = Math.Abs(longitude);
            var lonDegrees = Math.Truncate(absLongitude / 100);
            var lonResidue = absLongitude % 100;

            var directionEw = (char)values[3];

            return $"{latDegrees:0}° {latResidue:00.0000} {directionNs}, " +
                   $"{lonDegrees:0}° {lonResidue:00.0000} {directionEw}";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AltitudeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var numValue = (float?)value ?? 0;
            numValue = Math.Min(0, numValue);

            return $"{numValue}m";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = (string)value;

            return string.IsNullOrEmpty(stringValue) ? 0
                : int.Parse(new string(stringValue.Where(char.IsDigit).ToArray()));
        }
    }
}
