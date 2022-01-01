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
            if (!Plugin.configuration.behaviour.card.randomizeOpponentCard.Value) return;
            CardInfo cardInfo = __0;
            __0 = Utility.randomCardInfo(cardInfo);
        }
    }
}