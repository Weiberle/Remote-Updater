using RemoteUpdater.Common;
using RemoteUpdater.Common.Helper;
using RemoteUpdater.Receiver.Communication;
using RemoteUpdater.Receiver.Helper;
using RemoteUpdater.Receiver.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using MessageBox = System.Windows.Forms.MessageBox;

namespace RemoteUpdater.Receiver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, System.Windows.Forms.IWin32Window
    {
        public IntPtr Handle
        {
            get { return new WindowInteropHelper(this).Handle; }
        }

        private MainWindowViewModel _dataContext;

        public MainWindow()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => MessageBox.Show(this, args.ExceptionObject.ToString(), "Fehler", MessageBoxButtons.OK);

            DispatcherHelper.Init();

            _dataContext = new MainWindowViewModel();

            ViewModelBase.MainWindow = this;
            ViewModelBase.MainWindow32 = this;

            DataContext = _dataContext;

            _dataContext.Restart += OnRestart;
            _dataContext.UpdateErrorOccured += OnUpdateErrorOccured;

            WebHostBuilder.Run();
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _dataContext.SelectedFiles = FilesListView.SelectedItems.Cast<SourceTargetViewModel>();
        }

        private void OnUpdateErrorOccured()
        {
            DispatcherHelper.CheckDispatcherAndRun(() =>
            {
                this.BringToFront();
                FilesTabItem.IsSelected = true;
            });
        }

        private async Task OnRestart()
        {
            await WebHostBuilder.Restart();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            IsEnabled = false;

            SettingsHelper.SaveSettings();

            PlugIns.Core.Helper.SettingsHelper.SaveSettings();

            Task.Run(async () => { await WebHostBuilder.Stop(); });

            while (!WebHostBuilder.IsStopped)
            {
                Thread.Sleep(50);
            }

            base.OnClosing(e);
        }
    }
}
