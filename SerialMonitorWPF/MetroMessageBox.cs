using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace SerialMonitorWPF
{
    // Helper class to use MahApps Dialog windows from classes other than ViewModels.
    class MetroMessageBox
    {
        private MetroWindow _metroWindow;
        private MetroDialogSettings _metroDialogSettings;

        public MetroMessageBox()
        {
            InitializeDialogSettings();
        }

        private void UpdateMainWindow()
        {
            _metroWindow = (Application.Current.MainWindow as MetroWindow);
        }

        public async void ShowDialog(string title, string message)
        {
            if (_metroWindow == null)
                UpdateMainWindow();
            if (_metroWindow != null)
                await _metroWindow?.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative,
                    _metroDialogSettings);
        }

        private void InitializeDialogSettings()
        {
            _metroDialogSettings = new MetroDialogSettings {AnimateShow = false, AnimateHide = false};
        }
    }
}
