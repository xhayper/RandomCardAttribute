using BepInEx;
using BepInEx.Configuration;
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
        private const string PluginVersion = "1.0.0.0";

        internal static ManualLogSource Log;

        internal static ConfigEntry<bool> configRandomizeAllCost;
        internal static ConfigEntry<bool> configRandomizeOpponentCard;
        internal static ConfigEntry<bool> configRandomizePlayerCard;
        internal static ConfigEntry<bool> configRandomizeSquirrelCard;

        internal static ConfigEntry<int> configMinAttack;

        internal static ConfigEntry<int> configMaxAttack;

        internal static ConfigEntry<int> configMinHealth;

        internal static ConfigEntry<int> configMaxHealth;

        internal static ConfigEntry<int> configMinCost;

        internal static ConfigEntry<int> configMaxCost;

        private void Awake()
        {

            Plugin.Log = base.Logger;
            Log.LogInfo($"Oh boy, You made a huge mistake.");
            bindConfig();
            Harmony harmony = new Harmony(PluginGuid);
            harmony.PatchAll();
        }

        private void bindConfig()
        {
            configRandomizeAllCost = Config.Bind("Randomize", "randomize-all-three-cost", false, "By default, The mod only randomize one of the three cost, by turning this on, it will randomize all at the same time. Default: false");
            configRandomizeOpponentCard = Config.Bind("Randomize", "randomize-partner-card", true, "Should the mod randomize the opponent's card? Default: true");
            configRandomizePlayerCard = Config.Bind("Randomize", "randomize-player-card", true, "Should the mod randomize the player's card? Default: true");
            configRandomizeSquirrelCard = Config.Bind("Randomize", "randomize-squirrel-card", false, "Should the mod randomize the squirrel card's stat? Default: false");
            configMinAttack = Config.Bind("Range", "min-attack", 0, "The minimum attack value. Default: 0");
            configMaxAttack = Config.Bind("Range", "max-attack", 99, "The maximum attack value. Default: 99");
            configMinHealth = Config.Bind("Range", "min-health", 1, "The minimum health value. Default: 1");
            configMaxHealth = Config.Bind("Range", "max-health", 99, "The maximum health value. Default: 99");
            configMinCost = Config.Bind("Range", "min-cost", 0, "The minimum cost value. Default: 0");
            configMaxCost = Config.Bind("Range", "max-cost", 4, "The maximum cost value. Default: 4");
        }
    }
}