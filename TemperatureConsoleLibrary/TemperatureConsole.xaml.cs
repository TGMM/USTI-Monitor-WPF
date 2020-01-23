using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public static readonly DependencyProperty Temperature1Property = DependencyProperty.Register(
            "Temperature1", typeof(string), typeof(TemperatureConsole), new PropertyMetadata(default(string)));

        public string Temperature1
        {
            get { return (string) GetValue(Temperature1Property); }
            set
            {
                SetValue(Temperature1Property, value);
            }
        }

        public static readonly DependencyProperty Temperature2Property = DependencyProperty.Register(
            "Temperature2", typeof(string), typeof(TemperatureConsole), new PropertyMetadata(default(string)));

        public string Temperature2
        {
            get { return (string) GetValue(Temperature2Property); }
            set { SetValue(Temperature2Property, value); }
        }

        public static readonly DependencyProperty Temperature3Property = DependencyProperty.Register(
            "Temperature3", typeof(string), typeof(TemperatureConsole), new PropertyMetadata(default(string)));

        public string Temperature3
        {
            get { return (string) GetValue(Temperature3Property); }
            set { SetValue(Temperature3Property, value); }
        }

        public static readonly DependencyProperty Temperature4Property = DependencyProperty.Register(
            "Temperature4", typeof(string), typeof(TemperatureConsole), new PropertyMetadata(default(string)));

        public string Temperature4
        {
            get { return (string) GetValue(Temperature4Property); }
            set { SetValue(Temperature4Property, value); }
        }

        public static readonly DependencyProperty Temperature5Property = DependencyProperty.Register(
            "Temperature5", typeof(string), typeof(TemperatureConsole), new PropertyMetadata(default(string)));

        public string Temperature5
        {
            get { return (string) GetValue(Temperature5Property); }
            set { SetValue(Temperature5Property, value); }
        }

        public static readonly DependencyProperty Temperature6Property = DependencyProperty.Register(
            "Temperature6", typeof(string), typeof(TemperatureConsole), new PropertyMetadata(default(string)));

        public string Temperature6
        {
            get { return (string) GetValue(Temperature6Property); }
            set { SetValue(Temperature6Property, value); }
        }

    }
}
