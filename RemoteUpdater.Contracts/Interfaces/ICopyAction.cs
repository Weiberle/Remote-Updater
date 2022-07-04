namespace RemoteUpdater.Contracts.Interfaces
{
    public interface ICopyAction
    {
        string ActionName { get; }

        bool HasSettings { get; }

        string Description { get; }

        CopyActionSettings LoadSettings();

        void SaveSettings(CopyActionSettings settings);

        void Init(string settingsFolderPath);

        bool Execute();
    }
}
