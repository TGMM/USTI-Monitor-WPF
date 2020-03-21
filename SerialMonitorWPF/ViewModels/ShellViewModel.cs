using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows.Input;
using Caliburn.Micro;

namespace SerialMonitorWPF.ViewModels
{
    public class ShellViewModel : Screen
    {

        public BindableCollection<int?> Temperatures { get; set; }
        public BindableCollection<float?> Voltages { get; set; }
        public ObservableCollection<string> AvailablePorts { get; set; }
        public double? Rpm { get; set; }
        public double? AirSpeed { get; set; }

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
            for (var i = 0; i < SerialPortParser.TemperatureCount; i++)
            {
                Temperatures.Add(null);
            }
            //Initialize BatteryConsoleData
            Voltages = new BindableCollection<float?>();
            for (var i = 0; i < SerialPortParser.VoltageCount; i++)
            {
                Voltages.Add(null);
            }
            //Initialize Rpm
            Rpm = 0;
            //Initialize AirSpeed
            AirSpeed = 0;
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

        private void SerialPortParser_OnDataUpdate(object sender, UpdateDataEventArgs e)
        {
            var sp = (SerialPortParser)sender;
            OnUIThread(() =>
            {
                switch (e.DataType)
                {
                    case (int)SerialPortParser.DataType.Voltages:
                        Voltages = sp.GetVoltages();
                        NotifyOfPropertyChange(nameof(Voltages));
                        break;
                    case (int)SerialPortParser.DataType.Temperatures:
                        Temperatures = sp.GetTemperatures();
                        NotifyOfPropertyChange(nameof(Temperatures));
                        break;
                    case (int)SerialPortParser.DataType.EngineRpmFuel:
                        Rpm = sp.GetRpm();
                        NotifyOfPropertyChange(nameof(Rpm));
                        AirSpeed = sp.GetAirSpeed();
                        NotifyOfPropertyChange(nameof(AirSpeed));
                        break;
                }
            });
        }

        private void SerialPortParser_OnPortsChange(object sender, EventArgs e)
        {
            UpdatePorts();
        }

        public ICommand OpenPortCommand => new DelegateCommand(_serialPortParser.OpenPort);
    }
}
