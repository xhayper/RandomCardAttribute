using BepInEx;
using HarmonyLib;

namespace SteamPatch
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "io.github.xhayper.steampatch";
        private const string PluginName = "SteamPatch";
        private const string PluginVersion = "1.0.0.0";
        

        private void Awake()
        {
            var harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
            Logger.LogMessage("Steam has been patched.");
        }
    }
}