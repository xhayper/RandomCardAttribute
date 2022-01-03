using BepInEx.Configuration;

namespace RandomCardAttribute
{
    public class Configuration
    {
        public class BehaviourConfig
        {
            public ConfigEntry<bool> RandomizeInActTwo;

            public ConfigEntry<bool> RandomizeInActThree;

            public class CardConfig
            {
                public ConfigEntry<bool> RandomizeOpponentCard;

                public ConfigEntry<bool> RandomizePlayerCard;

                public ConfigEntry<bool> RandomizeSquirrelCard;

                public ConfigEntry<bool> RandomizeCorrectCost;

                public ConfigEntry<bool> RandomizeBoneCost;

                public ConfigEntry<bool> RandomizeBloodCost;

                public ConfigEntry<bool> RandomizeEnergyCost;

                public class ModificationConfig
                {
                    public ConfigEntry<bool> RandomizeSpecialAbility;

                    public ConfigEntry<bool> KeepTotemModification;

                    public ConfigEntry<bool> KeepLatchModification;

                    public ConfigEntry<bool> KeepOverclockModification;

                    public ConfigEntry<bool> KeepMergeModification;

                    public ConfigEntry<bool> KeepDuplicateModification;

                    public void Bind(ConfigFile config)
                    {
                        const string prefix = "Behaviour.Card.Modification";

                        RandomizeSpecialAbility = config.Bind(prefix, "randomizeSpecialAbility", true);
                        KeepTotemModification = config.Bind(prefix, "keepTotemModification", true);
                        KeepLatchModification = config.Bind(prefix, "keepLatchModification", true);
                        KeepOverclockModification = config.Bind(prefix, "keepOverclockModification", true);
                        KeepMergeModification = config.Bind(prefix, "keepMergeModification", false);
                        KeepDuplicateModification = config.Bind(prefix, "keepDuplicateModification", false);
                    }
                }

                public ModificationConfig Modification;

                public void Bind(ConfigFile config)
                {
                    const string prefix = "Behaviour.Card";

                    RandomizeOpponentCard = config.Bind(prefix, "randomizeOpponentCard", true);
                    RandomizePlayerCard = config.Bind(prefix, "randomizePlayerCard", true);
                    RandomizeSquirrelCard = config.Bind(prefix, "randomizeSquirrelCard", false);
                    RandomizeCorrectCost = config.Bind(prefix, "randomizeCorrectCost", true);
                    RandomizeBoneCost = config.Bind(prefix, "randomizeBoneCost", false);
                    RandomizeBloodCost = config.Bind(prefix, "randomizeBloodCost", false);
                    RandomizeEnergyCost = config.Bind(prefix, "randomizeEnergyCost", false);

                    Modification = new ModificationConfig();
                    Modification.Bind(config);
                }
            }

            public CardConfig Card;

            public void Bind(ConfigFile config)
            {
                const string prefix = "Behaviour";

                RandomizeInActTwo = config.Bind(prefix, "randomizeInActTwo", false);

                RandomizeInActThree = config.Bind(prefix, "randomizeInActThree", false);

                Card = new CardConfig();
                Card.Bind(config);
            }
        }

        public class RangeConfig
        {
            public class CardConfig
            {
                public ConfigEntry<int> MinAttack;

                public ConfigEntry<int> MaxAttack;

                public ConfigEntry<int> MinHealth;

                public ConfigEntry<int> MaxHealth;

                public ConfigEntry<int> MinCost;

                public ConfigEntry<int> MaxCost;

                public class ModificationConfig
                {
                    public ConfigEntry<int> MinBaseAbility;

                    public ConfigEntry<int> MaxBaseAbility;

                    public ConfigEntry<int> MinCustomAbility;

                    public ConfigEntry<int> MaxCustomAbility;

                    public void Bind(ConfigFile config)
                    {
                        const string prefix = "Range.Card.Modification";

                        MinBaseAbility = config.Bind(prefix, "minBaseAbility", 0);
                        MaxBaseAbility = config.Bind(prefix, "maxBaseAbility", 2);
                        MinCustomAbility = config.Bind(prefix, "minCustomAbility", 0);
                        MaxCustomAbility = config.Bind(prefix, "maxCustomAbility", 2);
                    }
                }

                public ModificationConfig Modification;

                public void Bind(ConfigFile config)
                {
                    const string prefix = "Range.Card";

                    Modification = new ModificationConfig();

                    MinAttack = config.Bind(prefix, "minAttack", 1);
                    MaxAttack = config.Bind(prefix, "maxAttack", 9);
                    MinHealth = config.Bind(prefix, "minHealth", 1);
                    MaxHealth = config.Bind(prefix, "maxHealth", 9);
                    MinCost = config.Bind(prefix, "minCost", 0);
                    MaxCost = config.Bind(prefix, "maxCost", 5);

                    Modification.Bind(config);
                }
            }

            public CardConfig Card;

            public void Bind(ConfigFile config)
            {
                Card = new CardConfig();
                Card.Bind(config);
            }
        }

        public BehaviourConfig Behaviour;

        public RangeConfig Range;

        public void Bind(ConfigFile config)
        {
            Behaviour = new BehaviourConfig();
            Behaviour.Bind(config);
            Range = new RangeConfig();
            Range.Bind(config);
        }
    }
}