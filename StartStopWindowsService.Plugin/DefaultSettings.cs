using StartStopWindowsService.Plugin.Language;

namespace StartStopWindowsService.Plugin
{
    internal static class DefaultSettings
    {
        internal static string WindowsServiceName { get; set; } = Resource.Txt_DefaultWindowsServiceName;

        internal static string WindowsServiceSettingName { get; set; } = Resource.Txt_DefaultWindowsServiceDescription;
    }
}
