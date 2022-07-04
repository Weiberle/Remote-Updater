using RemoteUpdater.Common.Helper;
using RemoteUpdater.Contracts;
using RemoteUpdater.Sender.Helper;
using RemoteUpdater.Sender.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using MessageBox = System.Windows.Forms.MessageBox;

namespace RemoteUpdater.Sender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, System.Windows.Forms.IWin32Window
    {
        private readonly MainWindowViewModel _dataContext;

        public IntPtr Handle
        {
            get { return new WindowInteropHelper(this).Handle; }
        }

        public MainWindow()
        {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += (sender, args) => MessageBox.Show(this, args.ExceptionObject.ToString(), "Fehler", MessageBoxButtons.OK);

            DispatcherHelper.Init();

            _dataContext = new MainWindowViewModel();

            DataContext = _dataContext;

            _dataContext.ShowMessage += OnShowMessage;
            _dataContext.UpdateErrorOccured += OnUpdateErrorOccured;
        }

        private void OnUpdateErrorOccured()
        {
            if (SettingsHelper.Settings.BringToFrontOnError)
            {
                DispatcherHelper.CheckDispatcherAndRun(() =>
                {
                    this.BringToFront();
                    FilesTabItem.IsSelected = true;
                });
            }
        }

        private void OnShowMessage(MessageDto message)
        {
            DispatcherHelper.CheckDispatcherAndRun(() =>
            {
                var icon = MessageBoxIcon.None;

                if (message.MessageType == MessageTypeEnum.Error)
                {
                    icon = MessageBoxIcon.Error;
                }
                else if (message.MessageType == MessageTypeEnum.Warning)
                {
                    icon = MessageBoxIcon.Warning;
                }
                else if (message.MessageType == MessageTypeEnum.Info)
                {
                    icon = MessageBoxIcon.Information;
                }

                MessageBox.Show(this, message.Message, message.Caption, MessageBoxButtons.OK, icon);
            });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SettingsHelper.SaveSettings();

            base.OnClosing(e);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _dataContext.SelectedFiles = FilesListView.SelectedItems.Cast<SourceViewModel>();
        }

        private void OnDrop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
            {
                try
                {
                    string[] files = (string[])e.Data.GetData(System.Windows.Forms.DataFormats.FileDrop);

                    if(files != null)
                    {
                        _dataContext.AddFiles(files);
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
