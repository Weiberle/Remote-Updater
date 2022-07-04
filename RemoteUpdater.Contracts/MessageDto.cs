namespace RemoteUpdater.Contracts
{
    public class MessageDto
    {
        public MessageDto() {}

        public string Message { get; set; }

        public string Caption { get; set; }

        public MessageTypeEnum MessageType { get; set; }
    }
}
