using System.Collections.Generic;

namespace RemoteUpdater.Sender.Settings
{
    public class SenderSettings
    {
        public string ReceiverIp { get; set; } 

        public uint ReceiverPort { get; set; } = 5000;

        public bool AutoUpdate { get; set; } = false;

        public uint AutoUpdateDelayInSec { get; set; } = 5;

        public bool BringToFrontOnError { get; set; } = true;

        public List<SourceSetting> SourceFiles { get; set; } = new List<SourceSetting>();
    }
}
