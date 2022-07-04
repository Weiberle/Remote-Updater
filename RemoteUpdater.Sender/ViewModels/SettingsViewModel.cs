using RemoteUpdater.Common;

namespace RemoteUpdater.Sender.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public string ReceiverIp
        {
            get
            {
                return Helper.SettingsHelper.Settings.ReceiverIp;
            }
            set
            {
                if (value != Helper.SettingsHelper.Settings.ReceiverIp)
                {
                    Helper.SettingsHelper.Settings.ReceiverIp = value;
                    OnPropertyChanged(nameof(ReceiverIp));
                }
            }
        }

        public uint ReceiverPort
        {
            get
            {
                return Helper.SettingsHelper.Settings.ReceiverPort;
            }
            set
            {
                if (value != Helper.SettingsHelper.Settings.ReceiverPort)
                {
                    Helper.SettingsHelper.Settings.ReceiverPort = value;
                    OnPropertyChanged(nameof(ReceiverPort));
                }
            }
        }

        public static bool AutoUpdate
        {
            get
            {
                return Helper.SettingsHelper.Settings.AutoUpdate;
            }
            set
            {
                if (value != Helper.SettingsHelper.Settings.AutoUpdate)
                {
                    Helper.SettingsHelper.Settings.AutoUpdate = value;
                }
            }
        }

        public static uint AutoUpdateDelayInSec
        {
            get
            {
                return Helper.SettingsHelper.Settings.AutoUpdateDelayInSec;
            }
            set
            {
                if (value != Helper.SettingsHelper.Settings.AutoUpdateDelayInSec)
                {
                    Helper.SettingsHelper.Settings.AutoUpdateDelayInSec = value;
                }
            }
        }

        public static bool BringToFrontOnError
        {
            get
            {
                return Helper.SettingsHelper.Settings.BringToFrontOnError;
            }
            set
            {
                if (value != Helper.SettingsHelper.Settings.BringToFrontOnError)
                {
                    Helper.SettingsHelper.Settings.BringToFrontOnError = value;
                }
            }
        }
    }
}
