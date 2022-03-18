using System.Reflection;
using BepInEx.Logging;
using HarmonyLib;
using BepInEx;

namespace RandomCardAttribute
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;

        internal static Configuration Configuration;

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo($"Oh boy, You made a huge mistake.");
            Configuration = new Configuration();
            Configuration.Bind(Config);
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginInfo.PLUGIN_GUID);
        }
    }
}
