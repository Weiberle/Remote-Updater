using System.Windows.Forms;

namespace RemoteUpdater.Receiver.Helper
{
    internal class FolderDialogHelper
    {
        internal static string GetFolder()
        {
            string newFolder = "";

            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = SettingsHelper.Settings.LastTargetFoler;

                dialog.Description = dialog.SelectedPath;

                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    newFolder = dialog.SelectedPath;
                    SettingsHelper.Settings.LastTargetFoler = newFolder;
                }
            }

            return newFolder;
        }
    }
}
