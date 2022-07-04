namespace RemoteUpdater.Contracts
{
    public class UpdateResultDto
    {
        public string FilePath { get; set; }

        public UpdateStatus Status { get; set; }
    }
}
