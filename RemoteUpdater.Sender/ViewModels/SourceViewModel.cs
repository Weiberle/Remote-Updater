
using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;
using RemoteUpdater.Contracts;
using RemoteUpdater.Sender.Helper;
using System.Windows.Media;

namespace RemoteUpdater.Sender.ViewModels
{
    public class SourceViewModel : ViewModelBase
    {
        private bool _isSelected;

        public SourceViewModel(string filePath, bool isSelected = true)
        {
            FilePath = filePath;

            Size = $"({FileSizeHelper.GetSize(filePath)})";

            IsSelected = isSelected;
        }

        public bool IsSelected 
        { 
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string FilePath { get; private set; }

        public string Size { get; private set; }

        public Brush StatusColor { get; private set; } = Brushes.LightGray;

        internal UpdateStatus UpdateStatus { get; private set; }

        internal void SetUpdateStatus(UpdateStatus status)
        {
            UpdateStatus = status;

            StatusColor = StatusColorHelper.GetStatusColor(status);

            OnPropertyChanged(nameof(StatusColor));
        }
    }
}
