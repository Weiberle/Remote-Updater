using RemoteUpdater.Receiver.DTOs;
using System;
using System.IO;
using System.Text.Json;

namespace RemoteUpdater.Receiver.Helper
{
    internal class SettingsHelper
    {
        private static ReceiverSettings _settings;

        public static event Action UpdateSettings;

        private static string GetSettingsFilePath()
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "Receiver.Settings.json");
        }

        public static ReceiverSettings Settings
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

        private static ReceiverSettings LoadSettings()
        {
            var file = GetSettingsFilePath();

            if (File.Exists(file))
            {
                var text = File.ReadAllText(file);

                return JsonSerializer.Deserialize<ReceiverSettings>(text);
            }

            return new ReceiverSettings
            {
                LastTargetFoler = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
        }
    }
}
