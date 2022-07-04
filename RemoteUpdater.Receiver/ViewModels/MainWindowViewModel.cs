using Microsoft.Toolkit.Mvvm.Input;
using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;
using RemoteUpdater.Contracts;
using RemoteUpdater.PlugIns.Core.ViewModels;
using RemoteUpdater.Receiver.Communication;
using RemoteUpdater.Receiver.Helper;
using RemoteUpdater.Receiver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace RemoteUpdater.Receiver
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IEnumerable<SourceTargetViewModel> _selectedFiles = new List<SourceTargetViewModel>();
        private bool _isEnabled = true;

        public event Func<Task> Restart;

        public event Action UpdateErrorOccured;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            internal set
            {
                DispatcherHelper.CheckDispatcherAndRun(() =>
                {
                    _isEnabled = value;
                    OnPropertyChanged(nameof(IsEnabled));
                });
            }
        }

        public Brush StatusColor { get; private set; } = StatusColorHelper.GetStatusColor(UpdateStatus.WasIgnored);

        public FilesViewModel Files { get; set; } = new FilesViewModel();

        public IEnumerable<SourceTargetViewModel> SelectedFiles
        {
            get => _selectedFiles;
            set
            {
                _selectedFiles = value;
                UpdateButtons();
            }
        }

        public SettingsViewModel SettingsVm { get; set; }

        public PlugInsViewModel PlugInsVm { get; set; }

        public RelayCommand SelectTarget4AllCommand { get; set; }

        public RelayCommand SelectTarget4SelectedCommand { get; set; }

        public MainWindowViewModel()
        {
            CommunicationHub.FilesReceived += OnFilesReceived;
            SettingsVm = new SettingsViewModel();
            PlugInsVm = new PlugInsViewModel();
            SettingsVm.PropertyChanged += OnSettingsVmChanged;
            LoadTargetFolderSettings();
            SettingsHelper.UpdateSettings += SaveTargetFolderSettings;

            SelectTarget4AllCommand = new RelayCommand(() => SelectTargetFolder(Files), () => Files.Any());
            SelectTarget4SelectedCommand = new RelayCommand(() => SelectTargetFolder(SelectedFiles), () => SelectedFiles.Any());
        }

        private async Task<List<UpdateResultDto>> OnFilesReceived(List<TransferFileDto> transferFiles)
        {
            IsEnabled = false;

            foreach (var file in transferFiles)
            {
                await AddOrUpdateFile(file);
            }

            var result = ExecuteUpdateAndActions();

            IsEnabled = true;

            return result;
        }

        private void SelectTargetFolder(IEnumerable<SourceTargetViewModel> vms)
        {
            if (vms.Any())
            {
                var targetFolder = FolderDialogHelper.GetFolder();

                if (!string.IsNullOrWhiteSpace(targetFolder))
                {
                    foreach (var file in vms)
                    {
                        file.TargetFolder = targetFolder;
                    }
                }
            }
        }

        private List<UpdateResultDto> ExecuteUpdateAndActions()
        {
            var preCopyActionSuccess = PlugInsVm.ExecutePreCopyActions();

            UpdateFiles();

            var postCopyActionSuccess = PlugInsVm.ExecutePostCopActions();

            return AfterUpdateFiles(preCopyActionSuccess, postCopyActionSuccess);
        }

        private void UpdateFiles()
        {
            DispatcherHelper.CheckDispatcherAndRun(() =>
            {
                // Dateien die nicht übertragen wurden werden gelöscht
                foreach (var file in Files.Where(f => !f.SourceWasSet).ToList())
                {
                    Files.Remove(file);
                }

                // Dateien updaten
                foreach (var file in Files)
                {
                    file.UpdateTarget();
                }
            });
        }

        private List<UpdateResultDto> AfterUpdateFiles(bool preCopyActionSuccess, bool postCopyActionSuccess)
        {
            var result = new List<UpdateResultDto>();

            DispatcherHelper.CheckDispatcherAndRun(() =>
            {
                result = GetUpdateResults();

                //Status zurücksetzen
                foreach (var file in Files)
                {
                    file.ResetStatus();
                }

                UpdateButtons();
                UpdateStatusColor();

                CheckForErrors(preCopyActionSuccess, postCopyActionSuccess, result);
            });

            return result;
        }

        private List<UpdateResultDto> GetUpdateResults()
        {
            return Files.Select(f => new UpdateResultDto { FilePath = f.SourceFile, Status = f.UpdateStatus }).ToList();
        }

        private void OnSettingsVmChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SettingsViewModel.Port))
            {
                Restart?.Invoke();
            }
            if (e.PropertyName == nameof(SettingsViewModel))
            {
                Restart?.Invoke();
            }
        }

        private void SaveTargetFolderSettings()
        {
            SettingsHelper.Settings.SourceTargetMappings.Clear();

            foreach (var file in Files.Where(f => f.TargetIsValid))
            {
                SettingsHelper.Settings.SourceTargetMappings.Add(new DTOs.SourceTargetSetting { SourceFile = file.SourceFile, TargetFolder = file.TargetFolder });
            }
        }

        private void LoadTargetFolderSettings()
        {
            foreach (var mapping in SettingsHelper.Settings.SourceTargetMappings)
            {
                var newFile = new SourceTargetViewModel(new TransferFileDto { FilePath = mapping.SourceFile }, mapping.TargetFolder);
                newFile.ResetStatus();
                Files.Add(newFile);
            }
        }

        private async Task AddOrUpdateFile(TransferFileDto transferFile)
        {
            var foundFile = Files.FirstOrDefault(f => f.SourceFile == transferFile.FilePath);

            if (foundFile != null)
            {
                foundFile.UpdateSource(transferFile);
            }
            else
            {
                await DispatcherHelper.CheckDispatcherAndRunAsync(() =>
                {
                    Files.Add(new SourceTargetViewModel(transferFile));
                });
            }
        }

        private void UpdateButtons()
        {
            SelectTarget4AllCommand.NotifyCanExecuteChanged();
            SelectTarget4SelectedCommand.NotifyCanExecuteChanged();
        }

        private void UpdateStatusColor()
        {
            StatusColor = StatusColorHelper.GetStatusColor(Files.Select(f => f.UpdateStatus));
            OnPropertyChanged(nameof(StatusColor));
        }

        private void CheckForErrors(bool preCopyActions, bool postCopyActions, List<UpdateResultDto> updateResults)
        {
            if (!preCopyActions || !postCopyActions || updateResults.Any(r => r.Status == UpdateStatus.WasNotUpdatedError))
            {
                UpdateErrorOccured?.Invoke();
            }
        }
    }
}
