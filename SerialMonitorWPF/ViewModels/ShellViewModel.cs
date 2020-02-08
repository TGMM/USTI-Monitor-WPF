using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Windows.Input;
using Caliburn.Micro;

namespace SerialMonitorWPF.ViewModels
{
    public class ShellViewModel : Screen
    {

        public BindableCollection<int?> Temperatures { get; set; }
        public ObservableCollection<string> AvailablePorts { get; set; }

        private readonly SerialPortParser _serialPortParser = new SerialPortParser();

        public ShellViewModel()
        {
            //Events
            _serialPortParser.OnDataUpdate += SerialPortParser_OnDataUpdate;
            _serialPortParser.OnPortsChange += SerialPortParser_OnPortsChange;
            //Assignments
            AvailablePorts = new ObservableCollection<string>();
            //Initialize TemperatureConsole data
            Temperatures = new BindableCollection<int?>();
            for (int i = 0; i < _serialPortParser.TemperatureCount; i++)
            {
                Temperatures.Add(null);
            }
            //Functions
            UpdatePorts();
        }

        private void UpdatePorts()
        {
            AvailablePorts.Clear();
            foreach (var port in SerialPort.GetPortNames())
            {
                AvailablePorts.Add(port);
            }
        }

        private void SerialPortParser_OnDataUpdate(object sender, EventArgs e)
        {
            var sp = (SerialPortParser)sender;
            OnUIThread(() =>
            {
                Temperatures = sp.GetTemperatures();
                NotifyOfPropertyChange(nameof(Temperatures));
            });
        }

        private void SerialPortParser_OnPortsChange(object sender, EventArgs e)
        {
            UpdatePorts();
        }

        public ICommand OpenPortCommand => new DelegateCommand(_serialPortParser.OpenPort);
    }
}
