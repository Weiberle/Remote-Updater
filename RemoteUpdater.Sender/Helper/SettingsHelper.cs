using RemoteUpdater.Common.Helper;
using RemoteUpdater.Sender.Settings;
using System;
using System.IO;
using System.Text.Json;

namespace RemoteUpdater.Sender.Helper
{
    internal class SettingsHelper
    {
        public static event Action UpdateSettings;

        private static SenderSettings _settings;

        private static string GetSettingsFilePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Sender.Settings.json");
        }

        public static SenderSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = LoadSettings();

                    if(string.IsNullOrEmpty(_settings.ReceiverIp))
                    {
                        _settings.ReceiverIp = IpAddressHelper.GetIp4Address();
                    }
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

        private static SenderSettings LoadSettings()
        {
            SenderSettings settings = null;

            var file = GetSettingsFilePath();

            if (File.Exists(file))
            {
                var text = File.ReadAllText(file);

                settings =  JsonSerializer.Deserialize<SenderSettings>(text);
            }

            return settings != null ? settings : new SenderSettings();
        }
    }
}

