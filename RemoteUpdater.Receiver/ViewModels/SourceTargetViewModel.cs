using Microsoft.Toolkit.Mvvm.Input;
using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;
using RemoteUpdater.Contracts;
using RemoteUpdater.Receiver.Helper;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;

namespace RemoteUpdater.Receiver.ViewModels
{
    public class SourceTargetViewModel : ViewModelBase
    {
        private TransferFileDto _transferFile;
        private string _targetFolder = string.Empty;

        public ICommand SelectTargetCommand { get; set; }

        public bool WasUpdated { get; private set; }

        public bool SourceWasSet { get; private set; }

        public string UpdateError { get; private set; }

        public UpdateStatus UpdateStatus { get; private set; }

        public Brush StatusColor { get; private set; }

        public SourceTargetViewModel(TransferFileDto transferFile, string targetFolder = null)
        {
            _transferFile = transferFile;
            TargetFolder = targetFolder;
            SourceWasSet = true;

            SelectTargetCommand = new RelayCommand(() =>
            {
                var tartetFolder = FolderDialogHelper.GetFolder();

                if (!string.IsNullOrWhiteSpace(tartetFolder))
                {
                    TargetFolder = tartetFolder;
                    OnPropertyChanged(nameof(TargetFolder));
                }
            });
        }

        public void ResetStatus()
        {
            SourceWasSet = false;
        }

        public void UpdateSource(TransferFileDto transferFile)
        {
            _transferFile = transferFile;
            SourceWasSet = true;
            OnPropertyChanged(nameof(IsEnabled));
            UpdateStatusInfos();
        }

        public void UpdateTarget()
        {
            WasUpdated = false;
            UpdateError = string.Empty;

            if (IsEnabled && TargetIsValid)
            {
                if (CanWriteFile(TargetFile))
                {
                    try
                    {
                        File.WriteAllBytes(TargetFile, _transferFile.Data);
                        WasUpdated = true;
                    }
                    catch (Exception exc)
                    {
                        UpdateError = $"Die Datei: {TargetFile} konnte nicht aktuallisiert werden. Exception: {exc}";
                        Trace.WriteLine(UpdateError);
                    }
                }
                else 
                {
                    UpdateError = $"Die Datei: {TargetFile} konnte nicht aktuallisiert werden.";
                    Trace.WriteLine(UpdateError);
                }
            }

            UpdateStatusInfos();
        }

        public string SourceFile => _transferFile.FilePath;

        public string TargetFolder
        {
            get => _targetFolder;
            set
            {
                if (_targetFolder != value)
                {
                    _targetFolder = value;                    
                    OnPropertyChanged(nameof(TargetFolder));
                    OnPropertyChanged(nameof(IsEnabled));
                    OnPropertyChanged(nameof(TargetIsValid));
                    UpdateStatusInfos();
                }
            }
        }
        public string TargetFile => Path.Combine(TargetFolder, Path.GetFileName(SourceFile));

        public bool IsEnabled => _transferFile.Data != null;

        public bool TargetIsValid => !string.IsNullOrWhiteSpace(TargetFolder);

        private bool CanWriteFile(string filePath)
        {
            var canWrite = true;

            if (File.Exists(filePath))
            {
                canWrite = !new FileInfo(TargetFile).Attributes.HasFlag(FileAttributes.ReadOnly);
            }

            return canWrite;
        }

        private void UpdateStatusInfos()
        {
            if (WasUpdated)
            {
                UpdateStatus = UpdateStatus.WasUpdated;
            }
            else if (!string.IsNullOrWhiteSpace(UpdateError))
            {
                UpdateStatus = UpdateStatus.WasNotUpdatedError;
            }
            else if (IsEnabled && !TargetIsValid)
            {
                UpdateStatus = UpdateStatus.WasNotUpdatedTargetNotSet;
            }
            else
            {
                UpdateStatus = UpdateStatus.WasIgnored;
            }

            StatusColor = StatusColorHelper.GetStatusColor(UpdateStatus);

            OnPropertyChanged(nameof(StatusColor));
        }
    }
}
