using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace RemoteUpdater.Sender.ViewModels
{
    public class FilesViewModel : ObservableCollection<SourceViewModel>
    {
        public event Action Changed;

        public new void Clear()
        {
            base.Clear();
            Changed?.Invoke();
        }

        public new void Add(SourceViewModel item)
        {
            if(AddIfNotExists(item))
            {
                Sort();
                Changed?.Invoke();
            }
        }

        public void AddRange(IEnumerable<SourceViewModel> items)
        {
            var added = false;

            foreach (var item in items)
            {
                added |= AddIfNotExists(item);
            }

            if (added)
            {
                Sort();
                Changed?.Invoke();
            }
        }

        public new void Remove(SourceViewModel item)
        {
            RemovePrivate(item);
            Changed?.Invoke();
        }

        public void RemoveRange(IEnumerable<SourceViewModel> items)
        {
            foreach (var item in items)
            {
                RemovePrivate(item);
            }
            Changed?.Invoke();
        }

        private void RemovePrivate(SourceViewModel item)
        {
            item.PropertyChanged -= OnPropertyChanged;
            base.Remove(item);
        }

        private bool AddIfNotExists(SourceViewModel item)
        {
            bool added = false;

            if (!Items.Any(i => i.FilePath == item.FilePath))
            {
                item.PropertyChanged += OnPropertyChanged;
                base.Add(item);
                added = true;
            }

            return added;
        }

        private void Sort()
        {
            var orderedItems = Items.OrderBy(f => f.FilePath).ToList();

            Clear();

            foreach (var item in orderedItems)
            {
                base.Add(item);
            }
        }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SourceViewModel.IsSelected))
            {
                Changed?.Invoke();
            }
        }
    }
}
