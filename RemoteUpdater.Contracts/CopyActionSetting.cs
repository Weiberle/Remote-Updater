namespace RemoteUpdater.Contracts
{
    public class CopyActionSetting
    {
        public CopyActionSetting(string settingName, string settingValue)
        {
            SettingName = settingName;
            SettingValue = settingValue;
        }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }
    }
}
