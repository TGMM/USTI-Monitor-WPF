using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace SerialMonitorWPF.ViewModels
{
    public class ShellViewModel : Screen
    {

        public ShellViewModel()
        {
            Temperatures = new ObservableCollection<string>();
            for (int i = 0; i < 6; i++)
            {
                Temperatures.Add("0");
            }
        }

        private ObservableCollection<string> _temperatures;

        public ObservableCollection<string> Temperatures
        {
            get
            { return _temperatures; }
            set
            { _temperatures = value; }
        }
    }
}
