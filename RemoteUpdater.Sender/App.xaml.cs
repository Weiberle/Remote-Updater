using RemoteUpdater.Sender.Helper;
using System.Windows;

namespace RemoteUpdater.Sender
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            if (!string.IsNullOrWhiteSpace(SettingsHelper.Settings.Langugage))
            {
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SettingsHelper.Settings.Langugage);
            }
        }
    }
}
