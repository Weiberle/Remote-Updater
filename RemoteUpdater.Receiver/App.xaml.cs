using RemoteUpdater.Receiver.Helper;
using System.Windows;

namespace RemoteUpdater.Receiver
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
