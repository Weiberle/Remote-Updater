using RemoteUpdater.Contracts.Interfaces;
using System.IO;

namespace RemoteUpdater.PlugIns.Core
{
    public class CopyActionModel
    {
        public CopyActionModel(ICopyAction action, string filePath)
        {
            Action = action;
            FilePath = filePath;
            SettingsFolderPath = Path.GetDirectoryName(filePath);
        }

        internal string FilePath { get; private set; }

        internal string SettingsFolderPath { get; private set; }

        internal ICopyAction Action { get; private set; }
    }
}
