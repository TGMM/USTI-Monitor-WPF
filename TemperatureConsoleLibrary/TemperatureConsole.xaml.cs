using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TemperatureConsoleLibrary
{
    /// <summary>
    /// Interaction logic for TemperatureConsole.xaml
    /// </summary>
    public partial class TemperatureConsole : UserControl
    {
        public TemperatureConsole()
        {
            InitializeComponent();
        }
    }

    public class TemperatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var numValue = (int?)value;
            if (value == null) return "-- ";
            return numValue >= 0 ? numValue + "° " : "-- ";

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = (string)value;

            return string.IsNullOrEmpty(stringValue) ? 0 
                : int.Parse(new string(stringValue.Where(char.IsDigit).ToArray()));
        }
    }
}
