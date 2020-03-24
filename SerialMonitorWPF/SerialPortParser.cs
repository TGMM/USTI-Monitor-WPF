using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AxSPortLib;
using Caliburn.Micro;
using SerialMonitorWPF.Resources;

namespace SerialMonitorWPF
{
    public class SerialPortParser
    {
        public event EventHandler<UpdateDataEventArgs> OnDataUpdate;
        public event EventHandler OnPortsChange;
        private readonly AxSPortAx _axSPortAx = new AxSPortAx();
        private readonly MetroMessageBox _mMessageBox = new MetroMessageBox();

        private BindableCollection<int?> _parsedTemperatures;
        private BindableCollection<float?> _parsedVoltages;
        private double ParsedRpm { get; set; }
        private double ParsedAirSpeed { get; set; }
        private CompassValues ParsedCompassValues { get; set; }
        private byte[] _inputBuffer;
        private int _bufferCounter;

        public const int TemperatureCount = 6;
        public const int VoltageCount = 8;

        public SerialPortParser()
        {
            _inputBuffer = new byte[100];
            _bufferCounter = 0;
            _axSPortAx.CreateControl();
            _axSPortAx.OnRxChar += AxSPortAx_OnRxChar;
            _axSPortAx.OnChangePortsList += _axSPortAx_OnChangePortsList;

            _parsedTemperatures = new BindableCollection<int?>();
            _parsedVoltages = new BindableCollection<float?>();
            ParsedRpm = 0;
            ParsedCompassValues = new CompassValues();
        }

        public struct CompassValues
        {
            public float Latitude { get; set; }
            public float Altitude { get; set; }
            public float Longitude { get; set; }
            public float Orientation { get; set; }
            public Char DirectionNs { get; set; }
            public Char DirectionEw { get; set; }
        };

        public enum DataType
        {
            Voltages,
            Temperatures,
            EngineRpmFuel,
            Compass
        }

        public void OpenPort(object portName)
        {
            if (portName == null)
            {
                _mMessageBox.ShowDialog(UIStrings.Error, UIStrings.SelectAPort);
                return;
            }

            var config = "19200"; //BaudRate
            config += ",";
            config += "N";  //Parity
            config += ",";
            config += "8";   //DataBits
            config += ",";
            config += "1";  //StopBits

            _axSPortAx.InitString(config);
            _axSPortAx.Open((string)portName);
            if (_axSPortAx.IsOpened)
            {
                _mMessageBox.ShowDialog(UIStrings.Completed, UIStrings.SerialPortOpened);
            }
            else
            {
                _mMessageBox.ShowDialog(UIStrings.Error, UIStrings.SerialPortOpenedError);
            }
        }

        private async void AxSPortAx_OnRxChar(object sender, _ISPortAxEvents_OnRxCharEvent e)
        {
            var bytesToRead = 1;
            while (_axSPortAx.Read(out var newByte, out bytesToRead) > 0)
            {
                await Task.Run(() => ProcessNewByte(newByte));
            }
        }

        private void ProcessNewByte(byte newByte)
        {
            switch ((char)newByte)
            {
                case '$':
                    _bufferCounter = 0;
                    break;
                case '\r':
                case '\n':
                    DecodeInput(Encoding.ASCII.GetString(_inputBuffer, 0, _bufferCounter));
                    Array.Clear(_inputBuffer, 0, _bufferCounter);
                    _bufferCounter = 0;
                    break;
                default:
                    _inputBuffer[_bufferCounter] = newByte;
                    _bufferCounter++;
                    break;
            }
        }

        private void DecodeInput(string input)
        {
            var subStrings = input.Split(',');

            try
            {
                if (subStrings[0].Equals("Med1") && subStrings.Length >= VoltageCount + 1)
                {
                    _parsedVoltages.Clear();
                    for (var i = 1; i <= VoltageCount; i++)
                    {
                        _parsedVoltages.Add(float.Parse(subStrings[i]));
                    }
                    UpdateData(DataType.Voltages);
                }
                else if (subStrings[0].Equals("Med2") && subStrings.Length >= TemperatureCount + 1)
                {
                    _parsedTemperatures.Clear();
                    for (var i = 1; i <= TemperatureCount; i++)
                    {
                        _parsedTemperatures.Add((int)float.Parse(subStrings[i]));
                    }
                    UpdateData(DataType.Temperatures);
                }
                else if (subStrings[0].Equals("Med3") && subStrings.Length >= 7)
                {
                    //float temperature1, temperature2, temperature3;

                    //temperature1 = float.Parse(subStrings[1]);  //3
                    //temperature2 = float.Parse(subStrings[2]);  //1
                    //temperature3 = float.Parse(subStrings[3]);  //2

                    //motorDiagram1.setTemperatures(temperature1, temperature3, temperature2);

                    ParsedRpm = float.Parse(subStrings[4]);

                    ParsedAirSpeed = float.Parse(subStrings[5]);

                    //float mainGasVoltage, reserveGasVoltage;

                    //mainGasVoltage = float.Parse(subStrings[6]);
                    //reserveGasVoltage = float.Parse(subStrings[7]);

                    //fuelBar1.setLevelVoltages(mainGasVoltage, reserveGasVoltage);
                    UpdateData(DataType.EngineRpmFuel);
                }
                else if (subStrings[0].Equals("Med4") && subStrings.Length >= 5)
                {

                    //Med4,lat,NS,log,EW,alt,angulo
                    ParsedCompassValues = new CompassValues
                    {
                        Latitude = float.Parse(subStrings[1]),
                        DirectionNs = char.Parse(subStrings[2]),
                        Longitude = float.Parse(subStrings[3]),
                        DirectionEw = char.Parse(subStrings[4]),
                        Altitude = float.Parse(subStrings[5]),
                        Orientation = float.Parse(subStrings[6])
                    };
                    UpdateData(DataType.Compass);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            LogWriter.WriteDataToLog(input);

        }

        private void UpdateData(DataType dataType)
        {
            OnDataUpdate?.Invoke(this, new UpdateDataEventArgs((int)dataType));
        }

        public BindableCollection<int?> GetTemperatures()
        {
            return _parsedTemperatures;
        }
        public BindableCollection<float?> GetVoltages()
        {
            return _parsedVoltages;
        }

        public double? GetRpm()
        {
            return ParsedRpm;
        }

        public double? GetAirSpeed()
        {
            return ParsedAirSpeed;
        }

        public CompassValues GetCompassValues()
        {
            return ParsedCompassValues;
        }

        private void _axSPortAx_OnChangePortsList(object sender, EventArgs e)
        {
            OnPortsChange?.Invoke(_axSPortAx, EventArgs.Empty);
        }
    }
}

public class UpdateDataEventArgs : EventArgs
{
    public int DataType { get; set; }
    public UpdateDataEventArgs(int dataType)
    {
        DataType = dataType;
    }
}
