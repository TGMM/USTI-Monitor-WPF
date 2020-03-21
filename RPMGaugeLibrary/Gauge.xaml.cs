using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace RPMGaugeLibrary
{
    /// <summary>
    /// Interaction logic for Gauge.xaml
    /// </summary>
    public partial class Gauge
    {
        public Gauge()
        {
            InitializeComponent();
            Circle.SizeChanged += OnResize;
            RpmText.TargetUpdated += RpmText_TargetUpdated;
        }

        private void RpmText_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            UpdateGauge(GetRpmFromText(RpmText.Text));
        }

        private void OnResize(object sender, SizeChangedEventArgs e)
        {
            UpdateGauge(GetRpmFromText(RpmText.Text));
        }

        private void UpdateGauge(double rpm)
        {
            var circleRadius = (Circle.ActualWidth * Circle.RenderTransform.Value.M11) / 2;
            ResizeGauge(circleRadius, CalculateAngleFromRpm(rpm));
        }

        private void ResizeGauge(double radius, double angle)
        {
            var xPos = radius * Math.Sin(angle);
            var yPos = radius * Math.Cos(angle);
            var transformations = new TransformGroup();
            transformations.Children.Add(new ScaleTransform(0.075, 0.075));
            transformations.Children.Add(new RotateTransform(angle * (180 / Math.PI)));
            transformations.Children.Add(new TranslateTransform(xPos, -yPos));
            Needle.RenderTransform = transformations;
        }

        private static double CalculateAngleFromRpm(double rpm)
        {
            var usableRpm = Math.Min(rpm, 7000);
            usableRpm = Math.Max(0, usableRpm);
            return ((usableRpm / 1000) * Math.PI) / 4;
        }

        private static double GetRpmFromText(string rpm)
        {
            var dRpm = string.IsNullOrEmpty(rpm) ? 0 : double.Parse(rpm);
            dRpm = Math.Min(dRpm, 7000);
            dRpm = Math.Max(0, dRpm);
            return dRpm;
        }
    }

    public class RpmValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var numValue = (double?)value;
            return numValue == null ? "0" : numValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var stringValue = (string)value;

            return string.IsNullOrEmpty(stringValue) ? 0
                : double.Parse(stringValue);
        }
    }
}
