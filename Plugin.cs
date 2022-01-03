using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

#pragma warning disable 169

namespace RandomCardAttribute
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api")]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "io.github.xhayper.randomcardattribute";
        private const string PluginName = "RandomCardAttribute";
        private const string PluginVersion = "1.1.1.0";

        internal static ManualLogSource Log;

        internal static Configuration Configuration;

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo($"Oh boy, You made a huge mistake.");
            Configuration = new Configuration();
            Configuration.Bind(Config);
            var harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
        }
    }
}