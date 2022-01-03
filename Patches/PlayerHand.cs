using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace RandomCardAttribute.Patches
{
    [HarmonyPatch(typeof(PlayerHand), "AddCardToHand")]
    [HarmonyPatch(new[] { typeof(PlayableCard), typeof(Vector3), typeof(float) })]
    // ReSharper disable once InconsistentNaming
    public class PlayerHand_AddCardToHand
    {
        [HarmonyPrefix]
        public static void Prefix(ref PlayableCard __0)
        {
            if (SaveManager.SaveFile.IsPart2 && !Plugin.Configuration.Behaviour.RandomizeInActTwo.Value) return;
            if (SaveManager.SaveFile.IsPart3 && !Plugin.Configuration.Behaviour.RandomizeInActThree.Value) return;
            if (!Plugin.Configuration.Behaviour.Card.RandomizePlayerCard.Value) return;
            var playableCard = __0;
            if (!Plugin.Configuration.Behaviour.Card.RandomizeSquirrelCard.Value && __0.Info.name == "Squirrel") return;
            playableCard.Info.Abilities.ForEach(ability =>
            {
                playableCard.TriggerHandler.RemoveAbility(ability);
            });
            playableCard.TemporaryMods.Clear();
            playableCard.Info = Utility.RandomCardInfo(playableCard.Info);
            playableCard.Info.Abilities.ForEach(ability =>
            {
                playableCard.TriggerHandler.AddAbility(ability);
            });
        }
    }
}