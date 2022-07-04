namespace RemoteUpdater.PlugIns.Core.Settings
{
    public class PlugInsCoreSettings
    {
        public bool PreCopyActionsEnabled { get; set; } = false;

        public bool PostCopyActionsEnabled { get; set; } = false;

        public List<CopyActionBaseSetting> PreCopyActions { get; set; } = new List<CopyActionBaseSetting>();

        public List<CopyActionBaseSetting> PostCopyActions { get; set; } = new List<CopyActionBaseSetting>();
    }
}
