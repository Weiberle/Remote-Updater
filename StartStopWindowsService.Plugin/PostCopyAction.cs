using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;
using RemoteUpdater.Contracts;
using RemoteUpdater.Contracts.Interfaces;
using StartStopWindowsService.Plugin.Language;

namespace StartStopWindowsService.Plugin
{
    public class PostCopyAction : ViewModelBase, IPostCopyAction
    {
        public string ActionName => Resource.Txt_StarterActionName;

        public bool HasSettings => true;

        public string Description => Resource.Txt_StarterActionDescription;

        private CopyActionSettingsHelper _copyActionSettingsHelper;

        public bool Execute()
        {
            return WindowsServiceHelper.StartService(LoadSettings().First().SettingValue);
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
