﻿using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;

namespace RemoteUpdater.Receiver
{
    public class SettingsViewModel : ViewModelBase
    {
        public int Port
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
    }
}