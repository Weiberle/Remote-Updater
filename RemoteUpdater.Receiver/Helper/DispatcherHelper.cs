using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace RemoteUpdater.Receiver.Helper
{
    internal static class DispatcherHelper
    {
        private static Dispatcher _dispatcher;

        public static void Init() => Init(Dispatcher.CurrentDispatcher);

        public static void Init(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public static void CheckDispatcherAndRun(Action runAction)
        {
            if (_dispatcher.CheckAccess())
            {
                runAction();
            }
            else
            {
                _dispatcher.Invoke(runAction);
            }
        }

        public static async Task CheckDispatcherAndRunAsync(Action runAction)
        {
            if (_dispatcher.CheckAccess())
            {
                runAction();
            }
            else
            {
                await _dispatcher.InvokeAsync(runAction);
            }
        }
    }
}
