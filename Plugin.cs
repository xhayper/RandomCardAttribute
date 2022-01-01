using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

#pragma warning disable 169

namespace RandomCardAttribute
{

    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "io.github.xhayper.randomcardattribute";
        private const string PluginName = "RandomCardAttribute";
        private const string PluginVersion = "1.1.0.0";

        internal static ManualLogSource Log;

        internal static Configuration configuration;

        private void Awake()
        {

            Plugin.Log = base.Logger;
            Log.LogInfo($"Oh boy, You made a huge mistake.");
            configuration = new Configuration();
            configuration.bind(Config);
            Harmony harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
        }
    }
}