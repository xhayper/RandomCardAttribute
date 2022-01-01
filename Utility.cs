using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;
namespace RandomCardAttribute
{
    public class Utility
    {
        public static SpecialTriggeredAbility randomSpecialTriggeredAbility()
        {
            SpecialTriggeredAbility[] specialTriggeredAbilities = (SpecialTriggeredAbility[])Enum.GetValues(typeof(SpecialTriggeredAbility));
            SpecialTriggeredAbility randSpecialTriggeredAbility = specialTriggeredAbilities[UnityEngine.Random.RandomRangeInt(0, specialTriggeredAbilities.Length - 1)];
            if (randSpecialTriggeredAbility == SpecialTriggeredAbility.None) return randomSpecialTriggeredAbility();
            return randSpecialTriggeredAbility;
        }

        public static Ability randomAbility()
        {
            List<Ability> abilityList = AbilitiesUtil.GetAbilities(true);
            abilityList.AddRange(AbilitiesUtil.GetAbilities(false));
            Ability randAbility = abilityList[UnityEngine.Random.RandomRangeInt(0, abilityList.Count - 1)];
            if (randAbility == Ability.None || randAbility == Ability.NUM_ABILITIES) return randomAbility();
            return randAbility;
        }

        public static List<Ability> randomAbilityList(int listSize = 3)
        {
            List<Ability> abilities = new List<Ability>();
            for (int i = 0; i < listSize; i++)
            {
                Ability randAbility = randomAbility();
                if (abilities.Contains(randAbility)) continue;
                abilities.Add(randAbility);
            }
            return abilities;
        }

        public static CardInfo randomCardInfo(CardInfo cardInfo, bool isOpponent = false)
        {
            Plugin.Log.LogDebug($"Randomizing card: {cardInfo.name}");
            if (!Plugin.configuration.behaviour.card.randomizeSquirrelCard.Value && cardInfo.name == "Squirrel") return cardInfo;
            CustomCard customCard = new CustomCard(cardInfo.name)
            {
                baseAttack = UnityEngine.Random.RandomRangeInt(Plugin.configuration.range.card.minAttack.Value, Plugin.configuration.range.card.maxAttack.Value),
                baseHealth = UnityEngine.Random.RandomRangeInt(Plugin.configuration.range.card.minHealth.Value, Plugin.configuration.range.card.maxHealth.Value),
                cost = Plugin.configuration.behaviour.card.randomizeBloodCost.Value ? UnityEngine.Random.RandomRangeInt(Plugin.configuration.range.card.minCost.Value, Plugin.configuration.range.card.maxCost.Value) : 0,
                bonesCost = Plugin.configuration.behaviour.card.randomizeBoneCost.Value ? UnityEngine.Random.RandomRangeInt(Plugin.configuration.range.card.minCost.Value, Plugin.configuration.range.card.maxCost.Value) : 0,
                energyCost = Plugin.configuration.behaviour.card.randomizeEnegryCost.Value ? UnityEngine.Random.RandomRangeInt(Plugin.configuration.range.card.minCost.Value, Plugin.configuration.range.card.maxCost.Value) : 0,
            };
            if (Plugin.configuration.behaviour.card.modification.randomizeSpecialAbility.Value && UnityEngine.Random.RandomRangeInt(0, 1) == 1) customCard.specialAbilities = new List<SpecialTriggeredAbility>() { randomSpecialTriggeredAbility() };
            CardInfo newCardInfo = customCard.AdjustCard(cardInfo);
            List<CardModificationInfo> cardModificationInfoList = new List<CardModificationInfo>();
            cardInfo.Mods.ForEach(cardModificationInfo =>
            {
                if (Plugin.configuration.behaviour.card.modification.keepDuplicateModification.Value && cardModificationInfo.fromDuplicateMerge) cardModificationInfoList.Add(cardModificationInfo);
                else if (Plugin.configuration.behaviour.card.modification.keepLatchModification.Value && cardModificationInfo.fromLatch) cardModificationInfoList.Add(cardModificationInfo);
                else if (Plugin.configuration.behaviour.card.modification.keepMergeModification.Value && cardModificationInfo.fromCardMerge) cardModificationInfoList.Add(cardModificationInfo);
                else if (Plugin.configuration.behaviour.card.modification.keepOverclockModification.Value && cardModificationInfo.fromOverclock) cardModificationInfoList.Add(cardModificationInfo);
                else if (Plugin.configuration.behaviour.card.modification.keepTotemModification.Value && cardModificationInfo.fromTotem) cardModificationInfoList.Add(cardModificationInfo);
            });
            for (int i = 0; i < UnityEngine.Random.RandomRangeInt(Plugin.configuration.range.card.modification.minBaseAbility.Value, Plugin.configuration.range.card.modification.maxBaseAbility.Value + 1); i++)
            {
                cardModificationInfoList.Add(new CardModificationInfo()
                {
                    abilities = { Utility.randomAbility() },
                });
            }
            if (!isOpponent)
            {
                for (int i = 0; i < UnityEngine.Random.RandomRangeInt(Plugin.configuration.range.card.modification.minCustomAbility.Value, Plugin.configuration.range.card.modification.maxCustomAbility.Value + 1); i++)
                {
                    cardModificationInfoList.Add(new CardModificationInfo()
                    {
                        abilities = { Utility.randomAbility() },
                        fromCardMerge = true
                    });
                }
            }
            newCardInfo.Mods = cardModificationInfoList;
            return newCardInfo;
        }
    }
}
