using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace RandomCardAttribute.Patches
{
    [HarmonyPatch(typeof(Opponent), "ModifyTurnPlan")]
    public class Opponent_QueueCard
    {
        [HarmonyPostfix]
        public static void Postfix(Opponent __instance, ref List<List<CardInfo>> __result)
        {
            if (!Plugin.Configuration.Behaviour.Card.RandomizeOpponentCard.Value) return;
            if (SaveManager.SaveFile.IsPart2 && !Plugin.Configuration.Behaviour.RandomizeInActTwo.Value) return;
            if (SaveManager.SaveFile.IsPart3 && !Plugin.Configuration.Behaviour.RandomizeInActThree.Value) return;
            var turnPlan = __result;
            var newTurnPlan = new List<List<CardInfo>>();
            turnPlan.ForEach(cardInfoList =>
            {
                Plugin.Log.LogDebug($"Randomizing {__instance.OpponentType}'s card");
                var newCardList = new List<CardInfo>();
                
                cardInfoList.ForEach(cardInfo =>
                {
                    newCardList.Add(Utility.RandomCardInfo(cardInfo, true));
                });
                newTurnPlan.Add(newCardList);
            });
            __result = newTurnPlan;
        }
    }
}
