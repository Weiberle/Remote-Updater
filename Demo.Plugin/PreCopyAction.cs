using RemoteUpdater.Contracts;
using RemoteUpdater.Contracts.Interfaces;

namespace Demo.Plugin
{
    public class PreCopyAction : IPreCopyAction
    {
        private CopyActionSettings _copyActionSettings = new CopyActionSettings { new CopyActionSetting("Thow Exception", "true") };

        public string ActionName => "Demo PreCopy-Action";

        public bool HasSettings => true;

        public string Description => "Beispiel einer PreCopy-Action";

        public bool Execute()
        {
            if (_copyActionSettings.First().SettingValue.ToLower().Trim() == "true")
                    {
                throw new Exception("Das Ausführen der Action hat einen Fehler verursacht.");
            }
            return true;
        }

        public void Init(string settingsFolderPath)
        {
            //throw new NotImplementedException();
        }

        public CopyActionSettings LoadSettings()
        {
            return _copyActionSettings;
        }

        public void SaveSettings(CopyActionSettings settings)
        {
            _copyActionSettings = settings;
        }
    }
}