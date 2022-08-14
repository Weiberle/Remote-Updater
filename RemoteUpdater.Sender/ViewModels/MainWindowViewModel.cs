using Microsoft.Toolkit.Mvvm.Input;
using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;
using RemoteUpdater.Contracts;
using RemoteUpdater.Sender.Communication;
using RemoteUpdater.Sender.Helper;
using RemoteUpdater.Sender.Language;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RemoteUpdater.Sender.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private SignalRConnection _signalRConnection;

        private IEnumerable<SourceViewModel> _selectedFiles { get; set; } = new List<SourceViewModel>();

        public RelayCommand SendFilesCommand { get; set; }
        public RelayCommand AddFilesCommand { get; set; }
        public RelayCommand RemoveSelectedFilesCommand { get; set; }
        public RelayCommand<SourceViewModel> RemoveFileCommand { get; set; }
        public RelayCommand RemoveAllFilesCommand { get; set; }
        public RelayCommand SelectDeselectAllCommand { get; set; }
        public RelayCommand SelectAllCommand { get; set; }
        public RelayCommand DeselectAllCommand { get; set; }
        public RelayCommand SelectSelectedCommand { get; set; }
        public RelayCommand DeselectSelectedCommand { get; set; }

        public FilesViewModel Files { get; set; } = new FilesViewModel();

        public string ConnectionState { get; set; } = Resource.Txt_ConnectionStatusUnknown;

        public SettingsViewModel SettingsVm { get; set; }

        public bool SelectDeselectAllIsChecked { get; set; }

        public bool IsSending { get; set; }

        public bool IsEnabled { get; set; } = true;

        public bool ContextMenuVisible { get; set; }

        public string SelectDeselectText { get; private set; }

        public Brush StatusColor { get; private set; } = StatusColorHelper.GetStatusColor(UpdateStatus.WasIgnored);

        public event Action<MessageDto> ShowMessage;

        public event Action UpdateErrorOccured;

        private bool _ignoreSourceFilesEvents = false;

        private SignalRConnection SignalRConnection
        {
            get
            {
                InitSignalR();
                return _signalRConnection;
            }
        }

        public MainWindowViewModel()
        {
            SendFilesCommand = new RelayCommand(async () => { await SendFiles(); }, () => Files.Any() && IsSending == false);
            AddFilesCommand = new RelayCommand(AddFiles, () => IsSending == false);
            RemoveSelectedFilesCommand = new RelayCommand(RemoveSelectedFiles, () => SelectedFiles.Any() && IsSending == false);
            RemoveAllFilesCommand = new RelayCommand(() => Files.Clear(), () => Files.Any() && IsSending == false);
            RemoveFileCommand = new RelayCommand<SourceViewModel>((f) => Files.Remove(f),(f) => IsSending == false);
            SelectDeselectAllCommand = new RelayCommand(SelectDeselectAll, () => Files.Any() && IsSending == false);
            SelectAllCommand = new RelayCommand(() => Files.ToList().ForEach(f => f.IsSelected = true), () => Files.Any(f => !f.IsSelected) && IsSending == false);
            DeselectAllCommand = new RelayCommand(() => Files.ToList().ForEach(f => f.IsSelected = false), () => Files.Any(f => f.IsSelected) && IsSending == false);
            SelectSelectedCommand = new RelayCommand(() => SelectedFiles.ToList().ForEach(f => f.IsSelected = true), () => SelectedFiles.Any(f => !f.IsSelected) && IsSending == false);
            DeselectSelectedCommand = new RelayCommand(() => SelectedFiles.ToList().ForEach(f => f.IsSelected = false), () => SelectedFiles.Any(f => f.IsSelected) && IsSending == false);

            SettingsVm = new SettingsViewModel();
            SettingsVm.PropertyChanged += OnSettingsChanged;
            FileSystemWatcherHelper.Enabled = true;
            FileSystemWatcherHelper.FileChanged += OnFileChanged;
            SettingsHelper.UpdateSettings += UpdateSettings;
            Files.Changed += UpdateUI;

            LoadSettings();
        }

        public IEnumerable<SourceViewModel> SelectedFiles
        {
            get => _selectedFiles;
            set
            {
                _selectedFiles = value;
                UpdateButtonStates();
            }
        }

        private void InitSignalR()
        {
            if (_signalRConnection == null)
            {
                _signalRConnection = new SignalRConnection();
                _signalRConnection.ConnectionChanged += OnConnectionChanged;
                _signalRConnection.ConnectionError += OnConnectionError;
            }
        }

        private void OnConnectionError(string errorMessage)
        {
            IsSending = false;
            OnPropertyChanged(nameof(IsSending));
            ShowMessage?.Invoke(new MessageDto { Caption = Resource.Txt_Error, Message = errorMessage, MessageType = MessageTypeEnum.Error });
            IsEnabled = true;
            OnPropertyChanged(nameof(IsEnabled));
            UpdateButtonStates();
        }

        private async Task OnConnectionChanged(string connectionState)
        {
            await DispatcherHelper.CheckDispatcherAndRunAsync(() =>
            {
                ConnectionState = connectionState;
                OnPropertyChanged(nameof(ConnectionState));
            });
        }

        private async Task SendFiles()
        {
            IsSending = true;
            IsEnabled = false;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsSending));
            UpdateButtonStates();
            UpdateStatusColor(true);

            try
            {
                foreach (var file in Files)
                {
                    file.SetUpdateStatus(UpdateStatus.WasIgnored);
                }

                var transferFiles = new List<TransferFileDto>();

                foreach (var file in Files)
                {
                    transferFiles.Add(new TransferFileDto
                    {
                        FilePath = file.FilePath,
                        Data = file.IsSelected ? File.ReadAllBytes(file.FilePath) : null
                    });
                }

                var updateResults = await SignalRConnection.SendFiles(transferFiles);

                UpdateResults(updateResults);
            }
            catch (Exception e)
            {
                UpdateErrorOccured?.Invoke();
                ShowMessage(new MessageDto
                {
                    Caption = Resource.Error_WhileSendingFiles,
                    Message = e.ToString(),
                    MessageType = MessageTypeEnum.Error
                });
            }

            IsSending = false;
            IsEnabled = true;
            OnPropertyChanged(nameof(IsEnabled));
            OnPropertyChanged(nameof(IsSending));
            UpdateButtonStates();
            UpdateStatusColor();
        }

        private void UpdateResults(List<UpdateResultDto> updateResults)
        {
            foreach (var result in updateResults)
            {
                var foundFile = Files.FirstOrDefault(f => f.FilePath == result.FilePath);

                if (foundFile != null)
                {
                    foundFile.SetUpdateStatus(result.Status);
                }
            }

            if (updateResults.Any(ur => ur.Status == UpdateStatus.WasNotUpdatedError))
            {
                UpdateErrorOccured?.Invoke();
            }
        }

        private void SelectDeselectAll()
        {
            _ignoreSourceFilesEvents = true;

            SelectDeselectAllIsChecked = SelectDeselectAllIsChecked;

            Files.ToList().ForEach(f => f.IsSelected = SelectDeselectAllIsChecked);

            UpdateSelectDeselectText();

            _ignoreSourceFilesEvents = false;

            UpdateSelectionInformation();
        }

        private void RemoveSelectedFiles()
        {
            Files.RemoveRange(_selectedFiles.ToList());
        }

        internal void AddFiles(IEnumerable<string> files)
        {
            Files.AddRange(files.Select(f => new SourceViewModel(f)));
        }

        private void AddFiles()
        {
            var newFiles = FileDialogHelper.GetFiles();

            Files.AddRange(newFiles.Select(f => new SourceViewModel(f)));
        }

        private void OnSettingsChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingsViewModel.ReceiverIp) || e.PropertyName == nameof(SettingsViewModel.ReceiverPort))
            {
                SignalRConnection.Disconnect();
            }
        }

        private void UpdateSettings()
        {
            SettingsHelper.Settings.SourceFiles.Clear();

            foreach (var file in Files)
            {
                SettingsHelper.Settings.SourceFiles.Add(new Settings.SourceSetting { SourceFile = file.FilePath, IsSelected = file.IsSelected });
            }
        }

        private void LoadSettings()
        {
            _ignoreSourceFilesEvents = true;

            foreach (var sourceFileSetting in SettingsHelper.Settings.SourceFiles)
            {
                if (File.Exists(sourceFileSetting.SourceFile))
                {
                    Files.Add(new SourceViewModel(sourceFileSetting.SourceFile, sourceFileSetting.IsSelected));
                }
            }

            _ignoreSourceFilesEvents = false;

            UpdateUI();
        }

        private void UpdateFileWatcher()
        {
            FileSystemWatcherHelper.RegisterFiles(Files.Select(vm => vm.FilePath).ToList());
        }

        private async Task OnFileChanged()
        {
            if (SettingsViewModel.AutoUpdate)
            {
                await DispatcherHelper.CheckDispatcherAndRunAsync(async () =>
                {
                    await SendFiles();
                });
            }
        }

        private void UpdateUI()
        {
            UpdateButtonStates();
            UpdateFileWatcher();
            UpdateSelectionInformation();
            UpdateSelectDeselectText();
            UpdateStatusColor();
        }

        private void UpdateButtonStates()
        {
            AddFilesCommand.NotifyCanExecuteChanged();
            SendFilesCommand.NotifyCanExecuteChanged();
            RemoveAllFilesCommand.NotifyCanExecuteChanged();
            RemoveSelectedFilesCommand.NotifyCanExecuteChanged();
            SelectAllCommand.NotifyCanExecuteChanged();
            SelectSelectedCommand.NotifyCanExecuteChanged();
            DeselectSelectedCommand.NotifyCanExecuteChanged();
            DeselectAllCommand.NotifyCanExecuteChanged();
            SelectDeselectAllCommand.NotifyCanExecuteChanged();

            ContextMenuVisible = Files.Any();
            OnPropertyChanged(nameof(ContextMenuVisible));
        }

        private void UpdateSelectDeselectText()
        {
            var countInfo = $"({Files.Count(f => f.IsSelected)}/{Files.Count()})";

            if (SelectDeselectAllIsChecked)
            {
                SelectDeselectText = $"  {Resource.CMenue_DelslectAllFiles} {countInfo}";
            }
            else
            {
                SelectDeselectText = $"  {Resource.CMenue_SelectAllFiles} {countInfo}";
            }

            OnPropertyChanged(nameof(SelectDeselectText));
        }

        private void UpdateSelectionInformation()
        {
            if (_ignoreSourceFilesEvents == false)
            {
                if (Files.Count == 0 || Files.Any(f => !f.IsSelected))
                {
                    SelectDeselectAllIsChecked = false;
                }
                else
                {
                    SelectDeselectAllIsChecked = true;
                }
                OnPropertyChanged(nameof(SelectDeselectAllIsChecked));
            }
        }

        private void UpdateStatusColor(bool reset = false)
        {
            StatusColor = StatusColorHelper.GetStatusColor(UpdateStatus.WasIgnored);

            if (reset == false)
            {
                StatusColor = StatusColorHelper.GetStatusColor(Files.Select(f => f.UpdateStatus));
            }

            OnPropertyChanged(nameof(StatusColor));
        }
    }
}
