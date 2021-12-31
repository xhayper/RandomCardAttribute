using APIPlugin;
using DiskCardGame;
using System.Collections.Generic;

namespace RandomCardAttribute
{
    public class Utility
    {
        public static Ability randomAbility()
        {
            List<Ability> values = AbilitiesUtil.GetAbilities(true);
            values.AddRange(AbilitiesUtil.GetAbilities(false));
            Ability randAbility = values[UnityEngine.Random.RandomRangeInt(0, values.Count - 1)];
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

        public static CardInfo randomCardInfo(CardInfo cardInfo)
        {
            Plugin.Log.LogDebug($"Randomizing card: {cardInfo.name}");
            if (!Plugin.configRandomizeSquirrelCard.Value && cardInfo.name == "Squirrel") return cardInfo;
            int costType = UnityEngine.Random.RandomRangeInt(0, 2);
            return new CustomCard(cardInfo.name)
            {
                baseAttack = UnityEngine.Random.RandomRangeInt(Plugin.configMinAttack.Value, Plugin.configMaxAttack.Value),
                baseHealth = UnityEngine.Random.RandomRangeInt(Plugin.configMinHealth.Value, Plugin.configMaxHealth.Value),
                cost = Plugin.configRandomizeAllCost.Value || costType == 0 ? UnityEngine.Random.RandomRangeInt(Plugin.configMinCost.Value, Plugin.configMaxCost.Value) : 0,
                bonesCost = Plugin.configRandomizeAllCost.Value || costType == 1 ? UnityEngine.Random.RandomRangeInt(Plugin.configMinCost.Value, Plugin.configMaxCost.Value) : 0,
                energyCost = Plugin.configRandomizeAllCost.Value || costType == 2 ? UnityEngine.Random.RandomRangeInt(Plugin.configMinCost.Value, Plugin.configMaxCost.Value) : 0,
                // abilities = randomAbilityList(),
            }.AdjustCard(cardInfo);
        }
    }
}
