using RemoteUpdater.Contracts;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace RemoteUpdater.Common.Helper
{
    public static class StatusColorHelper
    {
        public static Brush GetStatusColor(UpdateStatus status)
        {
            switch (status)
            {
                case UpdateStatus.WasUpdated:
                    return Brushes.Green;

                case UpdateStatus.WasNotUpdatedTargetNotSet:
                    return Brushes.Orange;

                case UpdateStatus.WasNotUpdatedError:
                    return Brushes.Red;

                default:
                    return Brushes.LightGray;
            }
        }

        public static Brush GetStatusColor(IEnumerable<UpdateStatus> status)
        {
            Brush statusColor = Brushes.LightGray;

            if (status.Any(s => s == UpdateStatus.WasNotUpdatedError))
            {
                statusColor = GetStatusColor(UpdateStatus.WasNotUpdatedError);
            }
            else if (status.Any(s => s == UpdateStatus.WasNotUpdatedTargetNotSet))
            {
                statusColor = GetStatusColor(UpdateStatus.WasNotUpdatedTargetNotSet);
            }
            else if (status.Any(s => s == UpdateStatus.WasUpdated))
            {
                statusColor = GetStatusColor(UpdateStatus.WasUpdated);
            }

            return statusColor;
        }
    }
}
