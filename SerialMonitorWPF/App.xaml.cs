using System.Windows;

namespace SerialMonitorWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            //QuickConverter provides you with WPF markup that allows you to write inline converters,
            //multi - bindings, and event handlers using a C# like language directly in your xaml.
            // Setup Quick Converter.
            // Add the System namespace so we can use primitive types (i.e. int, etc.).
            QuickConverter.EquationTokenizer.AddNamespace(typeof(object));
            // Add the System.Windows namespace so we can use Visibility.Collapsed, etc.
            QuickConverter.EquationTokenizer.AddNamespace(typeof(System.Windows.Visibility));
        }
    }
}
