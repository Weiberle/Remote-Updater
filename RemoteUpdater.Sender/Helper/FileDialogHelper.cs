using RemoteUpdater.Sender.Language;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RemoteUpdater.Sender.Helper
{
    internal static class FileDialogHelper
    {
        internal static List<string> GetFiles()
        {
            var files = new  List<string>();

            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Title = Resource.FileDialog_Title;

            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                files.AddRange(openFileDialog.FileNames);
            }

            return files;
        }

    }
}
