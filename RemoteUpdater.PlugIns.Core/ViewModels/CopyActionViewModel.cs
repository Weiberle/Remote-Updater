using Microsoft.Toolkit.Mvvm.Input;
using RemoteUpdater.Common;
using RemoteUpdater.Contracts;
using RemoteUpdater.PlugIns.Core.Language;
using RemoteUpdater.PlugIns.Core.Views;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace RemoteUpdater.PlugIns.Core.ViewModels
{
    public class CopyActionViewModel : ViewModelBase
    {
        private readonly CopyActionModel _copyAction;

        private CopyActionSettingsView _settingsView;

        private Brush _executionColor = Brushes.Black;

        public ICommand ShowSettingsCommand { get; set; }

        public ICommand SaveSettingsCommand { get; set; }

        public ICommand ExecuteActionCommand { get; set; }

        public string Description => _copyAction.Action.Description;

        public bool HasDescription => !string.IsNullOrWhiteSpace(_copyAction.Action.Description);

        public ObservableCollection<SettingItemViewModel> Settings { get; set; } = new ObservableCollection<SettingItemViewModel>();

        public CopyActionViewModel(CopyActionModel action, bool isEnabled = false)
        {
            _copyAction = action;
            _copyAction.Action.Init(_copyAction.SettingsFolderPath);
            ActionName = action.Action.ActionName;
            IsEnabled = isEnabled;
            LoadSettings();
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ExecuteActionCommand = new RelayCommand(ExecuteAction);
        }

        public bool Execute()
        {
            return ExecuteAction(false);
        }

        public string ActionName { get; set; }

        public string SettingsTitle => $"{ActionName} - {Resource.Lbl_ActionSettings}";

        public bool IsEnabled { get; set; }

        public bool HasSettings => _copyAction.Action.HasSettings;

        public Brush ExecutionColor 
        { 
            get => _executionColor;
            private set
            {
                if (_executionColor != value)
                {
                    _executionColor = value;
                    OnPropertyChanged(nameof(ExecutionColor));
                }
            }
        }

        private void ShowSettings()
        {
            _settingsView = new CopyActionSettingsView { DataContext = this };
            _settingsView.Owner = ViewModelBase.MainWindow;
            _settingsView.ShowDialog();
        }

        private void LoadSettings()
        {
            if (_copyAction.Action.HasSettings)
            {
                Settings.Clear();

                var settings = _copyAction.Action.LoadSettings();

                if (settings != null)
                {
                    foreach (var setting in _copyAction.Action.LoadSettings())
                    {
                        Settings.Add(new SettingItemViewModel { SettingName = setting.SettingName, SettingValue = setting.SettingValue });
                    }
                }
            }
        }

        private void SaveSettings()
        {
            _settingsView.Close();

            if (_copyAction.Action.HasSettings)
            {
                var settings = new CopyActionSettings();

                foreach (var setting in Settings)
                {
                    settings.Add(new CopyActionSetting(setting.SettingName, setting.SettingValue));
                }

                _copyAction.Action.SaveSettings(settings);
            }

            ExecutionColor = Brushes.Black;
        }

        private void ExecuteAction()
        {
            ExecuteAction(true);
        }

        private bool ExecuteAction(bool showError)
        {
            var success = false;

            ExecutionColor = Brushes.Black;

            try
            {
                success = _copyAction.Action.Execute();
            }
            catch (Exception exc)
            {
                if (showError)
                {
                    MessageBox.Show(ViewModelBase.MainWindow32, exc.ToString(), $"{Resource.Txt_Error} {_copyAction.Action.ActionName}", MessageBoxButtons.OK);
                }
            }

            ExecutionColor = success ? Brushes.Green : Brushes.Red;

            return success;
        }
    }
}
