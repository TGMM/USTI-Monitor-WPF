using System;
using System.Windows.Data;

namespace BatteryConsoleLibrary
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class BatteryConsole
    {
        public BatteryConsole()
        {
            InitializeComponent();
        }
    }

    public class GreaterThanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null) return false;
            float floatValue, floatParam;
            try
            {
                floatValue = (float) value;
                floatParam = System.Convert.ToSingle(parameter);
            }
            catch (Exception)
            {
                return false;
            }
            var valueToReturn = floatValue >= floatParam;
            return valueToReturn;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("Can't get value out of a bool");
        }
    }

    public class VoltageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return "0.00V";
            var fVal = (float) value;
            return fVal.ToString("N") + "V";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return 0;
            var onlyNumber = value.ToString().Replace("V", "");
            return System.Convert.ToSingle(onlyNumber);
        }
    }
}
