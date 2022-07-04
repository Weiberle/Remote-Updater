using System.Collections.ObjectModel;
using System.Linq;

namespace RemoteUpdater.Receiver.ViewModels
{
    public class FilesViewModel : ObservableCollection<SourceTargetViewModel>
    {
        public new void Add(SourceTargetViewModel item)
        {
            base.Add(item);
            Sort();
        }

        private void Sort()
        {
            var orderedItems = Items.OrderBy(f => f.SourceFile).ToList();

            Clear();

            foreach (var item in orderedItems)
            {
                base.Add(item);
            }
        }
    }
}
