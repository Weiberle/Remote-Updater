using RemoteUpdater.PlugIns.Core.Helper;
using RemoteUpdater.PlugIns.Core.Settings;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace RemoteUpdater.PlugIns.Core.ViewModels
{
    public class PlugInsViewModel
    {
        public ObservableCollection<CopyActionViewModel> PreCopyActions { get; set; } = new ObservableCollection<CopyActionViewModel>();
        public ObservableCollection<CopyActionViewModel> PostCopyActions { get; set; } = new ObservableCollection<CopyActionViewModel>();
        public bool PreCopyActionsEnabled { get => SettingsHelper.Settings.PreCopyActionsEnabled; set => SettingsHelper.Settings.PreCopyActionsEnabled = value; }
        public bool PostCopyActionsEnabled { get => SettingsHelper.Settings.PostCopyActionsEnabled; set => SettingsHelper.Settings.PostCopyActionsEnabled = value; }

        public bool HadPreCopyAction { get; private set; }

        public bool HadPostCopyAction { get; private set; }

        public PlugInsViewModel()
        {
            SettingsHelper.UpdateSettings += UpdateSettings;

            var copyActions = PluginHelper.GetCopyActions();

            AddActions(copyActions.Item1, PreCopyActions, SettingsHelper.Settings.PreCopyActions);
            AddActions(copyActions.Item2, PostCopyActions, SettingsHelper.Settings.PostCopyActions);
        }

        private static void AddActions(List<CopyActionModel> allCopyActions, ObservableCollection<CopyActionViewModel> copyActionVMs, List<CopyActionBaseSetting> actionSettings)
        {
            foreach (var actionSetting in actionSettings)
            {
                var foundAction = allCopyActions.FirstOrDefault(a => a.Action.ActionName == actionSetting.ActionName);

                if (foundAction != null)
                {
                    copyActionVMs.Add(new CopyActionViewModel(foundAction, actionSetting.IsEnabled));
                }
            }

            var oldCopyActions = copyActionVMs.ToList();

            // alle neuen Actions werden unten angehängt
            foreach (var action in allCopyActions.Where(a1 => !oldCopyActions.Any(a2 => a2.ActionName == a1.Action.ActionName)))
            {
                copyActionVMs.Add(new CopyActionViewModel(action));
            }
        }

        public bool ExecutePreCopyActions()
        {
            if (SettingsHelper.Settings.PreCopyActionsEnabled)
            {
                return ExecuteActions(PreCopyActions);
            }
            return true;
        }

        public bool ExecutePostCopActions()
        {
            if (SettingsHelper.Settings.PostCopyActionsEnabled)
            {
                return ExecuteActions(PostCopyActions);
            }
            return true;
        }

        private static bool ExecuteActions(ObservableCollection<CopyActionViewModel> copyActions)
        {
            var success = true;

            foreach (var action in copyActions.Where(a => a.IsEnabled))
            {
                try
                {
                    success &= action.Execute();
                }
                catch (Exception exc)
                {
                    Trace.WriteLine(exc); 
                }
            }

            return success;
        }

        private void UpdateSettings()
        {
            SettingsHelper.Settings.PreCopyActions.Clear();

            foreach (var action in PreCopyActions)
            {
                SettingsHelper.Settings.PreCopyActions.Add(new CopyActionBaseSetting { ActionName = action.ActionName, IsEnabled = action.IsEnabled });
            }

            SettingsHelper.Settings.PostCopyActions.Clear();

            foreach (var action in PostCopyActions)
            {
                SettingsHelper.Settings.PostCopyActions.Add(new CopyActionBaseSetting { ActionName = action.ActionName, IsEnabled = action.IsEnabled });
            }
        }
    }
}
