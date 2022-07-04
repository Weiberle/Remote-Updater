using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;

namespace RemoteUpdater.Receiver
{
    public class SettingsViewModel : ViewModelBase
    {
        public uint Port
        {
            get
            {
                return Helper.SettingsHelper.Settings.HttpPort;
            }
            set
            {
                if (value != Helper.SettingsHelper.Settings.HttpPort)
                {
                    Helper.SettingsHelper.Settings.HttpPort = value;
                    OnPropertyChanged(nameof(Port));
                }
            }
        }

        public string Ip4Address
        {
            get
            {
                return IpAddressHelper.GetIp4Address();
            }
        }

        public bool BringToFrontOnError
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
                    OnPropertyChanged(nameof(BringToFrontOnError));
                }
            }
        }

        public uint TimeOut
        {
            get
            {
                return Helper.SettingsHelper.Settings.TimeOutInMinutes;
            }
            set
            {
                if (value >= 1 && value != Helper.SettingsHelper.Settings.TimeOutInMinutes)
                {
                    Helper.SettingsHelper.Settings.TimeOutInMinutes = value;
                    OnPropertyChanged(nameof(TimeOut));
                }
            }
        }
    }
}
