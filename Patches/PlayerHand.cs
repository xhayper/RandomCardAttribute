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
        public static void Prefix(ref PlayableCard __0)
        {
            if (!Plugin.configuration.behaviour.card.randomizePlayerCard.Value) return;
            PlayableCard playableCard = __0;
            if (!Plugin.configuration.behaviour.card.randomizeSquirrelCard.Value && __0.Info.name == "Squirrel") return;
            playableCard.Info.Abilities.ForEach((ability) =>
            {
                playableCard.TriggerHandler.RemoveAbility(ability);
            });
            playableCard.TemporaryMods.Clear();
            playableCard.Info = Utility.randomCardInfo(playableCard.Info);
            playableCard.Info.Abilities.ForEach((ability) =>
            {
                playableCard.TriggerHandler.AddAbility(ability);
            });
        }
    }
}