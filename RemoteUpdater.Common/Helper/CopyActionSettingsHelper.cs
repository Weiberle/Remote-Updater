using System;
using System.IO;
using System.Diagnostics;
using System.Text.Json;
using RemoteUpdater.Contracts;

namespace RemoteUpdater.Common.Helper
{
    public class CopyActionSettingsHelper
    {
        private string _settingsFilePath;
        private CopyActionSettings _copyActionSettings;

        public CopyActionSettingsHelper(string settingsFolderPath, string copyActionName)
        {
            SetSettingsFilePath(settingsFolderPath, copyActionName);
        }

        public void SaveSettings(CopyActionSettings settings)
        {
            try
            {
                _copyActionSettings = settings;

                var text = JsonSerializer.Serialize(settings);

                File.WriteAllText(_settingsFilePath, text);
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

        }

        public CopyActionSettings LoadSettings()
        {
            try
            {
                var file = _settingsFilePath;

                if (_copyActionSettings == null && File.Exists(file))
                {
                    var text = File.ReadAllText(file);

                    _copyActionSettings = JsonSerializer.Deserialize<CopyActionSettings>(text);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }

            return _copyActionSettings;
        }

        private void SetSettingsFilePath(string settingsFolderPath, string copyActionName)
        {
            _settingsFilePath =  Path.Combine(settingsFolderPath, $"{copyActionName}.Settings.json");
        }
    }
}
