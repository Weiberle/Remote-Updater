namespace RemoteUpdater.Contracts
{
    public class TransferFileDto
    {
        public TransferFileDto()
        {

        }

        public string FilePath { get; set; }

        public byte[] Data { get; set; }        
    }
}
