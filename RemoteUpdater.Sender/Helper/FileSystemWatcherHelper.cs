using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace RemoteUpdater.Sender.Helper
{
    internal static class FileSystemWatcherHelper
    {
        internal static List<string> _files = new();

        internal static List<FileSystemWatcher> _watchers = new();

        internal static event Func<Task> FileChanged;
        private static uint DelayInSec => SettingsHelper.Settings.AutoUpdateDelayInSec;

        internal static object _lock = new();

        internal static Timer _timer;

        private static bool _enabled;

        public static bool Enabled
        {
            get => _enabled;
            set
            {
                lock (_lock)
                {
                    _enabled = value;
                }
            }
        }

        internal static void RegisterFiles(List<string> fileNames)
        {
            lock (_lock)
            {
                Unregister();

                _files = fileNames;

                var folders = new List<string>();

                foreach (var fileName in fileNames)
                {
                    var folder = Path.GetDirectoryName(fileName);

                    if (!folders.Contains(folder))
                    {
                        _watchers.Add(Register(folder));
                    }
                }
            }
        }

        private static FileSystemWatcher Register(string folder)
        {
            FileSystemWatcher fileSystemWatcher = new()
            {
                Path = folder,
                EnableRaisingEvents = true
            };

            fileSystemWatcher.Changed += OnFileSystemWatcherChanged;

            return fileSystemWatcher;
        }

        private static void Unregister()
        {
            foreach (var watcher in _watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Changed -= OnFileSystemWatcherChanged;
                watcher.Dispose();
            }

            _watchers.Clear();
        }

        private static void OnFileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                if (_timer == null && _enabled)
                {
                    if (_files.Contains(e.FullPath))
                    {
                        _timer = new Timer(DelayInSec * 1000);
                        _timer.Elapsed += OnTimerElapsed;
                        _timer.Start();
                    }
                }
            }
        }

        private static void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            FileChanged?.Invoke();

            lock (_lock)
            {
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Elapsed -= OnTimerElapsed;
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }
    }
}
