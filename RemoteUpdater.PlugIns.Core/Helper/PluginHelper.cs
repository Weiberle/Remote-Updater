using RemoteUpdater.Contracts.Interfaces;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace RemoteUpdater.PlugIns.Core.Helper
{
    internal static class PluginHelper
    {
        internal static Tuple<List<CopyActionModel>, List<CopyActionModel>> GetCopyActions()
        {
            var preCopyActions = new List<CopyActionModel>();
            var postCopyActions = new List<CopyActionModel>();

            foreach(var actionType in GetActionTypes())
            {
                preCopyActions.AddRange(CreateActionModels(actionType.PreCopyTypes, actionType.FilePath));
                postCopyActions.AddRange(CreateActionModels(actionType.PostCopyTypes, actionType.FilePath));
            }

            return new Tuple<List<CopyActionModel>, List<CopyActionModel>>(preCopyActions, postCopyActions);
        }

        private static T CreateInstance<T>(Type type)
        {
            try
            {
                return (T)Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Fehler beim Instanzieren vom Typ {type}. Exception {e}");
            }

            return default;
        }

        private static List<CopyActionModel> CreateActionModels(IEnumerable<Type> actionTypes, string filePath)
        {
            var result = new List<CopyActionModel>();

            if (actionTypes != null)
            {
                foreach (var type in actionTypes)
                {
                    var newInstance = CreateInstance<ICopyAction>(type);

                    if (newInstance != null)
                    {
                        result.Add(new CopyActionModel(newInstance, filePath));
                    }
                }
            }

            return result;
        }

        private static List<CopyActionType> GetActionTypes()
        {
            var result = new List<CopyActionType>();
            
            try
            {
                var pluginsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Plugins");

                if (Directory.Exists(pluginsFolder))
                {
                    foreach (var file in Directory.GetFiles(pluginsFolder, "*Plugin.dll", SearchOption.AllDirectories))
                    {
                        var assembly = Assembly.LoadFile(file);

                        result.Add(new CopyActionType
                        {
                            FilePath = file,
                            PreCopyTypes = GetTypes<IPreCopyAction>(assembly),
                            PostCopyTypes = GetTypes<IPostCopyAction>(assembly)
                        });
                    }
                }
                else
                {
                    Directory.CreateDirectory(pluginsFolder);
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Fehler beim Laden der Plug-Ins. Exception {e}");
            }

            return result;
        }

        private static IEnumerable<Type> GetTypes<T>(Assembly assembly) where T : ICopyAction
        {
            var result = new List<Type>();
            try
            {
                result.AddRange(assembly.GetTypes().Where(t => typeof(T).IsAssignableFrom(t)));
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Fehler beim Ermitteln der Typen aus dem Assembly {assembly.FullName}. Exception {e}");
            }
            return result;
        }

    }
}
