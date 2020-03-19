using System.Windows;

namespace BatteryConsoleLibrary
{
    /// <summary>
    /// Interaction logic for BatteryIndicator.xaml
    /// </summary>
    public partial class BatteryIndicator
    {
        public BatteryIndicator()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IsLitProperty = DependencyProperty.Register(
            "IsLit", typeof(bool), typeof(BatteryIndicator), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty VoltageProperty = DependencyProperty.Register("Voltage", typeof(bool), typeof(BatteryIndicator), new PropertyMetadata(default(bool)));

        public bool IsLit
        {
            get => (bool) GetValue(IsLitProperty);
            set => SetValue(IsLitProperty, value);
        }
    }
}
