using RemoteUpdater.PlugIns.Core.Settings;
using System.IO;
using System.Text.Json;

namespace RemoteUpdater.PlugIns.Core.Helper
{
    public static class SettingsHelper
    {
        private static PlugInsCoreSettings _settings;

        internal static event Action UpdateSettings;

        private static string GetSettingsFilePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "PlugIns.Core.Settings.json");
        }

        internal static PlugInsCoreSettings Settings
        {
            get
            {
                if(_settings == null)
                {
                    _settings = LoadSettings();
                }

                return _settings;
            }
        }

        public static void SaveSettings()
        {
            UpdateSettings?.Invoke();

            var text = JsonSerializer.Serialize(_settings);

            var file = GetSettingsFilePath();

            File.WriteAllText(file, text);
        }

        private static PlugInsCoreSettings LoadSettings()
        {
            var file = GetSettingsFilePath();

            if (File.Exists(file))
            {
                var text = File.ReadAllText(file);

                return JsonSerializer.Deserialize<PlugInsCoreSettings>(text);
            }

            return new PlugInsCoreSettings();
        }
    }
}
