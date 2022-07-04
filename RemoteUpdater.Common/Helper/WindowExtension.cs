using System.Windows;

namespace RemoteUpdater.Common.Helper
{
    public static class WindowExtension
    {
        public static void BringToFront(this Window window)
        {
            try
            {
                if (!window.IsVisible)
                {
                    window.Show();
                }

                if (window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }

                window.Activate();
                window.Topmost = true;
                window.Topmost = false;
                window.Focus();
            }
            catch (Exception) { }
        }
    }
}
