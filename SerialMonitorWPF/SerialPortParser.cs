using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AxSPortLib;
using Caliburn.Micro;
using SerialMonitorWPF.Resources;

namespace SerialMonitorWPF
{
    internal class SerialPortParser
    {
        public event EventHandler OnDataUpdate;
        public event EventHandler OnPortsChange;
        private readonly AxSPortAx _axSPortAx = new AxSPortAx();
        private readonly MetroMessageBox _mMessageBox = new MetroMessageBox();

        private BindableCollection<int?> _parsedTemperatures;

        private byte[] _inputBuffer;
        private int _bufferCounter;

        public int TemperatureCount = 6;

        public SerialPortParser()
        {
            _inputBuffer = new byte[100];
            _bufferCounter = 0;
            _axSPortAx.CreateControl();
            _axSPortAx.OnRxChar += AxSPortAx_OnRxChar;
            _axSPortAx.OnChangePortsList += _axSPortAx_OnChangePortsList;
            
            _parsedTemperatures = new BindableCollection<int?>();
        }

        public void OpenPort(object portName)
        {
            if ( portName == null)
            {
                _mMessageBox.ShowDialog(UIStrings.Error, UIStrings.SelectAPort);
                return;
            }

            _axSPortAx.Open((string)portName);
            if (_axSPortAx.IsOpened)
            {
                _mMessageBox.ShowDialog(UIStrings.Completed,UIStrings.SerialPortOpened);
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
                if (subStrings[0].Equals("Med2") && subStrings.Length >= TemperatureCount + 1)
                {
                    _parsedTemperatures.Clear();
                    for (int i = 1; i <= TemperatureCount; i++)
                    {
                        _parsedTemperatures.Add((int)float.Parse(subStrings[i]));
                    }
                    UpdateData();
                }
            }
            catch (Exception)
            {
                // ignored
            }

            WriteToLogfile(input);

        }

        private static void WriteToLogfile(string input)
        {
            const string ustiFolderPath = @"C:\USTI\";
            var logFilePath = Path.Combine(ustiFolderPath, "Log.txt");
            StreamWriter logFile;

            //Load configuration parameters
            if (File.Exists(logFilePath))
            {
                logFile = new StreamWriter(new FileStream(logFilePath, FileMode.Append, FileAccess.Write));
            }
            else
            {
                Directory.CreateDirectory(ustiFolderPath);
                logFile = new StreamWriter(new FileStream(logFilePath, FileMode.Create, FileAccess.Write));
            }

            logFile.WriteLine(input);
            logFile.Close();
        }

        private void UpdateData()
        {
            OnDataUpdate?.Invoke(this, EventArgs.Empty);
        }

        public BindableCollection<int?> GetTemperatures()
        {
            return _parsedTemperatures;
        }

        private void _axSPortAx_OnChangePortsList(object sender, EventArgs e)
        {
            OnPortsChange?.Invoke(_axSPortAx, EventArgs.Empty);
        }
    }
}
