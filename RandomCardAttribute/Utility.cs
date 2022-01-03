using DiskCardGame;
using System;
using System.Collections.Generic;

#pragma warning disable Publicizer001

namespace RandomCardAttribute
{
    public static class Utility
    {
        private static readonly RandomGenerator RandomGenerator = new();
        
        private static Array GetCorrectMetaCategories()
        {
            if (!Plugin.Configuration.Behaviour.UseCorrectAbilityPool.Value) return Enum.GetValues(typeof(AbilityMetaCategory));

            if (SaveManager.SaveFile.IsPart2) return new[] { AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook };
            if (SaveManager.SaveFile.IsPart3) return new[] { AbilityMetaCategory.BountyHunter, AbilityMetaCategory.Part3Modular, AbilityMetaCategory.Part3Rulebook, AbilityMetaCategory.Part3BuildACard };
            if (SaveManager.SaveFile.IsGrimora) return new[] { AbilityMetaCategory.GrimoraRulebook };
            if (SaveManager.SaveFile.IsMagnificus) return new[] { AbilityMetaCategory.MagnificusRulebook };
            
            return new[] {AbilityMetaCategory.Part1Modular, AbilityMetaCategory.Part1Rulebook} ;
        }
        
        private static List<Ability> GetAllAbilities(bool isOpponent = false)
        {
            var abilityList = new List<Ability>();
            foreach (AbilityMetaCategory category in GetCorrectMetaCategories())
            {
                // Have to do it like this because API plugin override .GetAbilities
                if (!isOpponent) abilityList.AddRange(AbilitiesUtil.GetAbilities(false, false, int.MinValue, int.MaxValue, category));
                abilityList.AddRange(AbilitiesUtil.GetAbilities(false, true, int.MinValue, int.MaxValue, category));
                if (!isOpponent) abilityList.AddRange(AbilitiesUtil.GetAbilities(true, false, int.MinValue, int.MaxValue, category));
                abilityList.AddRange(AbilitiesUtil.GetAbilities(false, true, int.MinValue, int.MaxValue, category));
            }
            return abilityList;
        }
        
        private static SpecialTriggeredAbility RandomSpecialTriggeredAbility()
        {
            while (true)
            {
                var specialTriggeredAbilities =
                    (SpecialTriggeredAbility[])Enum.GetValues(typeof(SpecialTriggeredAbility));
                var randSpecialTriggeredAbility =
                    specialTriggeredAbilities[
                        RandomGenerator.Next(0, specialTriggeredAbilities.Length - 1)];
                if (randSpecialTriggeredAbility is SpecialTriggeredAbility.None) continue;
                return randSpecialTriggeredAbility;
            }
        }

        private static Ability RandomAbility(bool isOpponent = false)
        {
            while (true)
            {
                var abilityList = GetAllAbilities(isOpponent);
                var randAbility = abilityList[RandomGenerator.Next(0, abilityList.Count - 1)];
                if (randAbility is Ability.None or Ability.NUM_ABILITIES or Ability.SquirrelOrbit or Ability.AllStrike) continue;
                return randAbility;
            }
        }

        public static CardInfo RandomCardInfo(CardInfo cardInfo, bool isOpponent = false)
        {
            if (!Plugin.Configuration.Behaviour.Card.RandomizeSquirrelCard.Value && cardInfo.name == "Squirrel")
                return cardInfo;
            Plugin.Log.LogDebug($"Randomizing card: {cardInfo.name}");
            var newCardInfo = (CardInfo) cardInfo.Clone();
            var costType = SaveManager.SaveFile.IsPart2 || !Plugin.Configuration.Behaviour.Card.RandomizeCorrectCost.Value ? RandomGenerator.Next(0, 2) :
                SaveManager.SaveFile.IsPart1 ? RandomGenerator.Next(0, 1) : 2;
            newCardInfo.baseAttack = RandomGenerator.Next(Plugin.Configuration.Range.Card.MinAttack.Value,
                Plugin.Configuration.Range.Card.MaxAttack.Value);
            newCardInfo.baseHealth = RandomGenerator.Next(Plugin.Configuration.Range.Card.MinHealth.Value,
              Plugin.Configuration.Range.Card.MaxHealth.Value);

            newCardInfo.cost =
                Plugin.Configuration.Behaviour.Card.RandomizeCorrectCost.Value && costType == 0 ||
                Plugin.Configuration.Behaviour.Card.RandomizeBloodCost.Value
                    ? RandomGenerator.Next(Plugin.Configuration.Range.Card.MinCost.Value,
                        Plugin.Configuration.Range.Card.MaxCost.Value)
                    : 0;
            newCardInfo.bonesCost =
                Plugin.Configuration.Behaviour.Card.RandomizeCorrectCost.Value && costType == 1 ||
                Plugin.Configuration.Behaviour.Card.RandomizeBoneCost.Value
                    ? RandomGenerator.Next(Plugin.Configuration.Range.Card.MinCost.Value,
                        Plugin.Configuration.Range.Card.MaxCost.Value)
                    : 0;
            newCardInfo.energyCost =
                Plugin.Configuration.Behaviour.Card.RandomizeCorrectCost.Value && costType == 2 ||
                Plugin.Configuration.Behaviour.Card.RandomizeEnergyCost.Value
                    ? RandomGenerator.Next(Plugin.Configuration.Range.Card.MinCost.Value,
                        Plugin.Configuration.Range.Card.MaxCost.Value)
                    : 0;
            if (Plugin.Configuration.Behaviour.Card.Modification.RandomizeSpecialAbility.Value &&
                RandomGenerator.Next(0, 1) == 1)
                newCardInfo.specialAbilities = new List<SpecialTriggeredAbility> { RandomSpecialTriggeredAbility() };
            var nameReplacementList = new List<string>();
            var deathCardInfoList = new List<DeathCardInfo>();
            var bountyHunterInfoList = new List<BountyHunterInfo>();
            newCardInfo.mods.RemoveAll(cardModificationInfo =>
            {
                nameReplacementList.Add(cardModificationInfo.nameReplacement);
                deathCardInfoList.Add(cardModificationInfo.deathCardInfo);
                bountyHunterInfoList.Add(cardModificationInfo.bountyHunterInfo);
                return !(Plugin.Configuration.Behaviour.Card.Modification.KeepDuplicateModification.Value &&
                         cardModificationInfo.fromDuplicateMerge) &&
                       !(Plugin.Configuration.Behaviour.Card.Modification.KeepLatchModification.Value &&
                         cardModificationInfo.fromLatch) &&
                       !(Plugin.Configuration.Behaviour.Card.Modification.KeepMergeModification.Value &&
                         cardModificationInfo.fromCardMerge) &&
                       !(Plugin.Configuration.Behaviour.Card.Modification.KeepOverclockModification.Value &&
                         cardModificationInfo.fromOverclock) &&
                       !(Plugin.Configuration.Behaviour.Card.Modification.KeepTotemModification.Value &&
                         cardModificationInfo.fromTotem);
            });
            newCardInfo.abilities.Clear();
            for (var i = 0;
                 i < RandomGenerator.Next(
                     Plugin.Configuration.Range.Card.Modification.MinBaseAbility.Value,
                     Plugin.Configuration.Range.Card.Modification.MaxBaseAbility.Value + 1);
                 i++)
            {
                var item = new CardModificationInfo();
                item.abilities.Add(RandomAbility(isOpponent));
                newCardInfo.mods.Add(item);
            }

            if (SaveManager.SaveFile.IsPart1)
            {
                for (var i = 0;
                     i < RandomGenerator.Next(
                         Plugin.Configuration.Range.Card.Modification.MinCustomAbility.Value,
                         Plugin.Configuration.Range.Card.Modification.MaxCustomAbility.Value + 1);
                     i++)
                {
                    var item = new CardModificationInfo();
                    item.abilities.Add(RandomAbility(isOpponent));
                    item.fromCardMerge = true;
                    newCardInfo.mods.Add(item);
                }
            }

            nameReplacementList.ForEach(nameReplacement =>
            {
                var item = new CardModificationInfo
                {
                    nameReplacement = nameReplacement
                };
                newCardInfo.mods.Add(item);
            });

            deathCardInfoList.ForEach(deathCardInfo =>
            {
                var item = new CardModificationInfo
                {
                    deathCardInfo = deathCardInfo
                };
                newCardInfo.mods.Add(item);
            });

            bountyHunterInfoList.ForEach(bountyHunterInfo =>
            {
                var item = new CardModificationInfo
                {
                    bountyHunterInfo = bountyHunterInfo
                };
                newCardInfo.mods.Add(item);
            });

            return newCardInfo;
        }
    }
}