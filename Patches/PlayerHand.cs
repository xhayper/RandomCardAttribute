using DiskCardGame;
using HarmonyLib;
using System;
using UnityEngine;

namespace RandomCardAttribute
{
    [HarmonyPatch(typeof(PlayerHand), "AddCardToHand")]
    [HarmonyPatch(new Type[] { typeof(PlayableCard), typeof(Vector3), typeof(float) })]
    public class PlayerHand_AddCardToHand
    {
        [HarmonyPrefix]
        public static void Prefix(PlayableCard __0)
        {
            if (!Plugin.configRandomizePlayerCard.Value) return;
            CardInfo cardInfo = __0.Info;
            __0.Info = Utility.randomCardInfo(cardInfo);
        }
    }
}