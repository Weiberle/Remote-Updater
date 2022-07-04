using System.Collections.Generic;

namespace RemoteUpdater.Receiver.DTOs
{
    public class ReceiverSettings
    {
        public string LastTargetFoler { get; set; }

        public int HttpPort { get; set; } = 5000;

        public List<SourceTargetSetting> SourceTargetMappings { get; set; } = new List<SourceTargetSetting>();
    }
}
