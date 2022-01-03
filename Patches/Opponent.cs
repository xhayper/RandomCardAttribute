using DiskCardGame;
using HarmonyLib;

namespace RandomCardAttribute.Patches
{
    [HarmonyPatch(typeof(Opponent), "QueueCard")]
    // ReSharper disable once InconsistentNaming
    public class Opponent_QueueCard
    {
        [HarmonyPrefix]
        public static void Prefix(ref CardInfo __0)
        {
            if (SaveManager.SaveFile.IsPart2 && !Plugin.Configuration.Behaviour.RandomizeInActTwo.Value) return;
            if (SaveManager.SaveFile.IsPart3 && !Plugin.Configuration.Behaviour.RandomizeInActThree.Value) return;
            if (!Plugin.Configuration.Behaviour.Card.RandomizeOpponentCard.Value) return;
            __0 = Utility.RandomCardInfo(__0, true);
        }
    }
}