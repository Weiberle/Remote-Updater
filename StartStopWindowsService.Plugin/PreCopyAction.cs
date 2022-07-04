using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;
using RemoteUpdater.Contracts;
using RemoteUpdater.Contracts.Interfaces;

namespace StartStopWindowsService.Plugin
{
    public class PreCopyAction : ViewModelBase, IPreCopyAction
    {
        public string ActionName => "Windows-Dienst Stopper";

        public bool HasSettings => true;

        public string Description => "Diese Action stoppt einen Windows-Dienst";

        private CopyActionSettingsHelper _copyActionSettingsHelper;

        public bool Execute()
        {
            return WindowsServiceHelper.StopService(LoadSettings().First().SettingValue);
        }

        public void Init(string settingsFolderPath)
        {
            _copyActionSettingsHelper = new CopyActionSettingsHelper(settingsFolderPath, ActionName);
        }

        public CopyActionSettings LoadSettings()
        {
            var settings = _copyActionSettingsHelper.LoadSettings();

            if (settings == null)
            {
                settings = new CopyActionSettings();
                settings.Add(new CopyActionSetting(DefaultSettings.WindowsServiceSettingName, DefaultSettings.WindowsServiceName));
            }
            return settings;
        }

        public void SaveSettings(CopyActionSettings settings)
        {
            _copyActionSettingsHelper.SaveSettings(settings);
        }
    }
}
