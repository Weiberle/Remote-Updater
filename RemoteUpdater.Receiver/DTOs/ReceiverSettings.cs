using System.Collections.Generic;

namespace RemoteUpdater.Receiver.DTOs
{
    public class ReceiverSettings
    {
        public string LastTargetFoler { get; set; }

        public uint HttpPort { get; set; } = 5000;

        public uint TimeOutInMinutes { get; set; } = 5;

        public bool BringToFrontOnError { get; set; } = true;

        public List<SourceTargetSetting> SourceTargetMappings { get; set; } = new List<SourceTargetSetting>();
    }
}
