using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BatteryConsoleLibrary
{
    /// <summary>
    /// Interaction logic for BatteryBar.xaml
    /// </summary>
    public partial class BatteryBar
    {
        public BatteryBar()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty VoltageProperty = DependencyProperty.Register(
            "Voltage", typeof(float), typeof(BatteryBar), new PropertyMetadata(default(float)));

        public float Voltage
        {
            get => (float) GetValue(VoltageProperty);
            set => SetValue(VoltageProperty, value);
        }
    }
}
