using Microsoft.AspNetCore.SignalR;
using RemoteUpdater.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemoteUpdater.Receiver.Communication
{
    public class CommunicationHub : Hub
    {
        public static event Func<List<TransferFileDto>, Task<List<UpdateResultDto>>> FilesReceived;

        public static CommunicationHub Instance { get; set; }

        public async Task<List<UpdateResultDto>> ReceiveFiles(List<TransferFileDto> transferFiles)
        {
            return await FilesReceived?.Invoke(transferFiles);
        }
    }
}
