using RemoteUpdater.Contracts;
using RemoteUpdater.Contracts.Interfaces;

namespace Demo.Plugin
{
    public class PostCopyAction : IPostCopyAction
    {
        public string ActionName => "Demo PostCopy-Action";

        public bool HasSettings => false;

        public string Description => "Beispiel einer PostCopy-Action";

        public bool Execute()
        {
            return true;
        }

        public void Init(string settingsFolderPath)
        {
            //throw new NotImplementedException();
        }

        public CopyActionSettings LoadSettings()
        {
            return null;
        }

        public void SaveSettings(CopyActionSettings settings)
        {
            //throw new NotImplementedException();
        }
    }
}
