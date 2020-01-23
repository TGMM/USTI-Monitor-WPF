using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace SerialMonitorWPF.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region TemperatureProperties
        private string _temperature1;
        private string _temperature2;
        private string _temperature3;
        private string _temperature4;
        private string _temperature5;
        private string _temperature6;

        public string Temperature1
        {
            get { return _temperature1 + "°"; }
            set
            {
                _temperature1 = value;
                NotifyOfPropertyChange(() => Temperature1);
            }
        }

        public string Temperature2
        {
            get { return _temperature2 + "°"; }
            set
            {
                _temperature2 = value;
                NotifyOfPropertyChange(() => Temperature2);
            }
        }

        public string Temperature3
        {
            get { return _temperature3 + "°"; }
            set
            {
                _temperature3 = value;
                NotifyOfPropertyChange(() => Temperature3);
            }
        }

        public string Temperature4
        {
            get { return _temperature4 + "°"; }
            set
            {
                _temperature4 = value;
                NotifyOfPropertyChange(() => Temperature4);
            }
        }

        public string Temperature5
        {
            get { return _temperature5 + "°"; }
            set
            {
                _temperature5 = value;
                NotifyOfPropertyChange(() => Temperature5);
            }
        }

        public string Temperature6
        {
            get { return _temperature6 + "°"; }
            set
            {
                _temperature6 = value;
                NotifyOfPropertyChange(() => Temperature6);
            }
        }
        #endregion
    }
}
