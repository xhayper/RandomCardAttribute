using DiskCardGame;
using HarmonyLib;

namespace RandomCardAttribute
{
    [HarmonyPatch(typeof(Opponent), "QueueCard")]
    public class Opponent_QueueCard
    {
        [HarmonyPrefix]
        public static void Prefix(ref CardInfo __0)
        {
            if (SaveManager.SaveFile.IsPart2 && !Plugin.configuration.behaviour.randomizeInActTwo.Value) return;
            if (SaveManager.SaveFile.IsPart3 && !Plugin.configuration.behaviour.randomizeInActThree.Value) return;
            if (!Plugin.configuration.behaviour.card.randomizeOpponentCard.Value) return;
            __0 = Utility.randomCardInfo(__0, true);
        }
    }
}