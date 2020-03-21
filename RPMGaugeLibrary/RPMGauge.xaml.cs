using System;
using System.Linq;
using System.Windows.Data;

namespace RPMGaugeLibrary
{
    /// <summary>
    /// Interaction logic for RpmGauge.xaml
    /// </summary>
    public partial class RpmGauge
    {
        public RpmGauge()
        {
            InitializeComponent();
        }
    }

    public class AirSpeedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var numValue = (double?)value;
            return numValue == null ? "0 km/h" : numValue + " km/h";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = (string)value;

            return string.IsNullOrEmpty(stringValue) ? 0
                : int.Parse(new string(stringValue.Where(char.IsDigit).ToArray()));
        }
    }
}
