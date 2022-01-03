using APIPlugin;
using DiskCardGame;
using System;
using System.Collections.Generic;

namespace RandomCardAttribute
{
    public static class Utility
    {
        private static SpecialTriggeredAbility RandomSpecialTriggeredAbility()
        {
            while (true)
            {
                var specialTriggeredAbilities =
                    (SpecialTriggeredAbility[])Enum.GetValues(typeof(SpecialTriggeredAbility));
                var randSpecialTriggeredAbility =
                    specialTriggeredAbilities[
                        UnityEngine.Random.RandomRangeInt(0, specialTriggeredAbilities.Length - 1)];
                if (randSpecialTriggeredAbility is SpecialTriggeredAbility.None) continue;
                return randSpecialTriggeredAbility;
            }
        }

        private static Ability RandomAbility()
        {
            while (true)
            {
                var abilityList = AbilitiesUtil.GetAbilities(true);
                abilityList.AddRange(AbilitiesUtil.GetAbilities(false));
                var randAbility = abilityList[UnityEngine.Random.RandomRangeInt(0, abilityList.Count - 1)];
                if (randAbility is Ability.None or Ability.NUM_ABILITIES) continue;
                return randAbility;
            }
        }

        public static CardInfo RandomCardInfo(CardInfo cardInfo, bool isOpponent = false)
        {
            Plugin.Log.LogDebug($"Randomizing card: {cardInfo.name}");
            if (!Plugin.Configuration.Behaviour.Card.RandomizeSquirrelCard.Value && cardInfo.name == "Squirrel")
                return cardInfo;
            var costType = UnityEngine.Random.RandomRangeInt(0, 2);
            var customCard = new CustomCard(cardInfo.name)
            {
                baseAttack = UnityEngine.Random.RandomRangeInt(Plugin.Configuration.Range.Card.MinAttack.Value,
                    Plugin.Configuration.Range.Card.MaxAttack.Value),
                baseHealth = UnityEngine.Random.RandomRangeInt(Plugin.Configuration.Range.Card.MinHealth.Value,
                    Plugin.Configuration.Range.Card.MaxHealth.Value),
                cost =
                    Plugin.Configuration.Behaviour.Card.RandomizeCorrectCost.Value && costType == 0 &&
                    (SaveManager.SaveFile.IsPart1 || SaveManager.SaveFile.IsPart2) ||
                    Plugin.Configuration.Behaviour.Card.RandomizeBloodCost.Value
                        ? UnityEngine.Random.RandomRangeInt(Plugin.Configuration.Range.Card.MinCost.Value,
                            Plugin.Configuration.Range.Card.MaxCost.Value)
                        : cardInfo.BloodCost,
                bonesCost =
                    Plugin.Configuration.Behaviour.Card.RandomizeCorrectCost.Value && costType == 1 &&
                    (SaveManager.SaveFile.IsPart1 || SaveManager.SaveFile.IsPart2) ||
                    Plugin.Configuration.Behaviour.Card.RandomizeBoneCost.Value
                        ? UnityEngine.Random.RandomRangeInt(Plugin.Configuration.Range.Card.MinCost.Value,
                            Plugin.Configuration.Range.Card.MaxCost.Value)
                        : cardInfo.BonesCost,
                energyCost =
                    Plugin.Configuration.Behaviour.Card.RandomizeCorrectCost.Value && costType == 2 &&
                    (SaveManager.SaveFile.IsPart2 || SaveManager.SaveFile.IsPart3) ||
                    Plugin.Configuration.Behaviour.Card.RandomizeEnergyCost.Value
                        ? UnityEngine.Random.RandomRangeInt(Plugin.Configuration.Range.Card.MinCost.Value,
                            Plugin.Configuration.Range.Card.MaxCost.Value)
                        : cardInfo.EnergyCost,
            };
            if (Plugin.Configuration.Behaviour.Card.Modification.RandomizeSpecialAbility.Value &&
                UnityEngine.Random.RandomRangeInt(0, 1) == 1)
                customCard.specialAbilities = new List<SpecialTriggeredAbility> { RandomSpecialTriggeredAbility() };
            var newCardInfo = customCard.AdjustCard(cardInfo);
            var cardModificationInfoList = new List<CardModificationInfo>();
            var nameReplacementList = new List<string>();
            var deathCardInfoList = new List<DeathCardInfo>();
            var bountyHunterInfoList = new List<BountyHunterInfo>();
            cardInfo.Mods.ForEach(cardModificationInfo =>
            {
                bountyHunterInfoList.Add(cardModificationInfo.bountyHunterInfo);
                deathCardInfoList.Add(cardModificationInfo.deathCardInfo);
                nameReplacementList.Add(cardModificationInfo.nameReplacement);
                if (Plugin.Configuration.Behaviour.Card.Modification.KeepDuplicateModification.Value &&
                    cardModificationInfo.fromDuplicateMerge) cardModificationInfoList.Add(cardModificationInfo);
                else if (Plugin.Configuration.Behaviour.Card.Modification.KeepLatchModification.Value &&
                         cardModificationInfo.fromLatch) cardModificationInfoList.Add(cardModificationInfo);
                else if (Plugin.Configuration.Behaviour.Card.Modification.KeepMergeModification.Value &&
                         cardModificationInfo.fromCardMerge) cardModificationInfoList.Add(cardModificationInfo);
                else if (Plugin.Configuration.Behaviour.Card.Modification.KeepOverclockModification.Value &&
                         cardModificationInfo.fromOverclock) cardModificationInfoList.Add(cardModificationInfo);
                else if (Plugin.Configuration.Behaviour.Card.Modification.KeepTotemModification.Value &&
                         cardModificationInfo.fromTotem) cardModificationInfoList.Add(cardModificationInfo);
            });
            for (var i = 0;
                 i < UnityEngine.Random.RandomRangeInt(
                     Plugin.Configuration.Range.Card.Modification.MinBaseAbility.Value,
                     Plugin.Configuration.Range.Card.Modification.MaxBaseAbility.Value + 1);
                 i++)
            {
                var item = new CardModificationInfo();
                item.abilities.Add(RandomAbility());
                cardModificationInfoList.Add(item);
            }

            if (!isOpponent && SaveManager.SaveFile.IsPart1)
            {
                for (var i = 0;
                     i < UnityEngine.Random.RandomRangeInt(
                         Plugin.Configuration.Range.Card.Modification.MinCustomAbility.Value,
                         Plugin.Configuration.Range.Card.Modification.MaxCustomAbility.Value + 1);
                     i++)
                {
                    var item = new CardModificationInfo();
                    item.abilities.Add(RandomAbility());
                    item.fromCardMerge = true;
                    cardModificationInfoList.Add(item);
                }
            }

            nameReplacementList.ForEach(nameReplacement =>
            {
                var item = new CardModificationInfo
                {
                    nameReplacement = nameReplacement
                };
                cardModificationInfoList.Add(item);
            });

            deathCardInfoList.ForEach(deathCardInfo =>
            {
                var item = new CardModificationInfo
                {
                    deathCardInfo = deathCardInfo
                };
                cardModificationInfoList.Add(item);
            });

            bountyHunterInfoList.ForEach(bountyHunterInfo =>
            {
                var item = new CardModificationInfo
                {
                    bountyHunterInfo = bountyHunterInfo
                };
                cardModificationInfoList.Add(item);
            });

            newCardInfo.Mods = cardModificationInfoList;
            return newCardInfo;
        }
    }
}