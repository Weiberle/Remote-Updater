using Microsoft.AspNetCore.SignalR.Client;
using RemoteUpdater.Contracts;
using RemoteUpdater.Sender.Language;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RemoteUpdater.Sender.Communication
{
    public class SignalRConnection
    {
        private HubConnection _connection;

        public event Func<string, Task> ConnectionChanged;

        public event Action<string> ConnectionError;

        public SignalRConnection()
        {
            CreateConnection();
        }

        private void CreateConnection()
        {
            _connection = new HubConnectionBuilder().WithUrl($"http://{Helper.SettingsHelper.Settings.ReceiverIp}:{Helper.SettingsHelper.Settings.ReceiverPort}/CommunicationHub").Build();

            _connection.Closed += OnClosed;

            _connection.Reconnected += OnReconnected;
        }

        private void DestroyConneciton()
        {
            _connection.Closed -= OnClosed;

            _connection.Reconnected -= OnReconnected;

            _connection.DisposeAsync();

            _connection = null;

            ConnectionChanged?.Invoke(Resource.Txt_ConnectionStatusNotConnected);
        }

        private async Task OnClosed(Exception arg)
        {
            await ConnectionChanged?.Invoke(Resource.Txt_ConnectionStatusNotConnected);
        }

        private async Task OnReconnected(string arg)
        {
            await ConnectionChanged?.Invoke($"{Resource.Txt_ConnectionStatusConnected} {Helper.SettingsHelper.Settings.ReceiverIp}:{Helper.SettingsHelper.Settings.ReceiverPort}");
        }

        private async Task<bool> Connect()
        {

            if (_connection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _connection.StartAsync();

                    await ConnectionChanged($"{Resource.Txt_ConnectionStatusConnected} {Helper.SettingsHelper.Settings.ReceiverIp}:{Helper.SettingsHelper.Settings.ReceiverPort}");
                }
                catch (Exception e)
                {
                    ConnectionError.Invoke(Resource.Error_ConnectionNotPossible);
                }
            }

            return _connection.State == HubConnectionState.Connected;
        }

        public void Disconnect()
        {
            if (_connection.State != HubConnectionState.Disconnected)
            {
                _connection.StopAsync();
            }

            DestroyConneciton();

            CreateConnection();
        }


        public async Task<List<UpdateResultDto>> SendFiles(List<TransferFileDto> transferFiles)
        {
            var result = new List<UpdateResultDto>();

            if (await Connect())
            {
                result = await _connection.InvokeAsync<List<UpdateResultDto>>("ReceiveFiles", transferFiles);
            }

            return result;
        }
    }
}
