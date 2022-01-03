using HarmonyLib;

namespace SteamPatch.Patches
{
    [HarmonyPriority(1)]
    [HarmonyPatch(typeof(SteamManager), "Awake")]
    // ReSharper disable once InconsistentNaming
    public class SteamManager_Awake
    {
        [HarmonyPrefix]
        private static bool Prefix()
        {
            return false;
        }
    }
}