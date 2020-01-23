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

		public static readonly DependencyProperty TemperaturesProperty = DependencyProperty.Register(
			"Temperatures", typeof(ObservableCollection<string>), typeof(TemperatureConsole), new PropertyMetadata(default(ObservableCollection<string>)));

		public ObservableCollection<string> Temperatures
		{
			get
			{ return (ObservableCollection<string>) GetValue(TemperaturesProperty); }
			set { SetValue(TemperaturesProperty, value); }
		}

	}
}
