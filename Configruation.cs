using BepInEx.Configuration;

namespace RandomCardAttribute
{
    public class Configuration
    {
        public class Behaviour
        {
            public class Card
            {
                public ConfigEntry<bool> randomizeOpponentCard;

                public ConfigEntry<bool> randomizePlayerCard;

                public ConfigEntry<bool> randomizeSquirrelCard;

                public ConfigEntry<bool> randomizeBoneCost;

                public ConfigEntry<bool> randomizeBloodCost;

                public ConfigEntry<bool> randomizeEnegryCost;

                public class Modification
                {

                    public ConfigEntry<bool> randomizeSpecialAbility;

                    public ConfigEntry<bool> keepTotemModification;

                    public ConfigEntry<bool> keepLatchModification;

                    public ConfigEntry<bool> keepOverclockModification;


                    public ConfigEntry<bool> keepMergeModification;

                    public ConfigEntry<bool> keepDuplicateModification;

                    public void bind(ConfigFile Config)
                    {
                        string prefix = "Behaviour.Card.Modification";

                        randomizeSpecialAbility = Config.Bind(prefix, "randomizeSpecialAbility", true);
                        keepTotemModification = Config.Bind(prefix, "keepTotemModification", true);
                        keepLatchModification = Config.Bind(prefix, "keepLatchModification", true);
                        keepOverclockModification = Config.Bind(prefix, "keepOverclockModification", true);
                        keepMergeModification = Config.Bind(prefix, "keepMergeModification", false);
                        keepDuplicateModification = Config.Bind(prefix, "keepDuplicateModification", false);
                    }
                }

                public Modification modification;

                public void bind(ConfigFile Config)
                {
                    string prefix = "Behaviour.Card";

                    randomizeOpponentCard = Config.Bind(prefix, "randomizeOpponentCard", true);
                    randomizePlayerCard = Config.Bind(prefix, "randomizePlayerCard", true);
                    randomizeSquirrelCard = Config.Bind(prefix, "randomizeSquirrelCard", false);
                    randomizeBoneCost = Config.Bind(prefix, "randomizeBoneCost", false);
                    randomizeBloodCost = Config.Bind(prefix, "randomizeBloodCost", true);
                    randomizeEnegryCost = Config.Bind(prefix, "randomizeEnegryCost", false);

                    modification = new Modification();
                    modification.bind(Config);
                }
            }

            public Card card;

            public void bind(ConfigFile Config)
            {
                card = new Card();
                card.bind(Config);
            }
        }

        public class Range
        {
            public class Card
            {
                public ConfigEntry<int> minAttack;

                public ConfigEntry<int> maxAttack;

                public ConfigEntry<int> minHealth;

                public ConfigEntry<int> maxHealth;

                public ConfigEntry<int> minCost;

                public ConfigEntry<int> maxCost;

                public class Modification
                {
                    public ConfigEntry<int> minBaseAbility;

                    public ConfigEntry<int> maxBaseAbility;

                    public ConfigEntry<int> minCustomAbility;

                    public ConfigEntry<int> maxCustomAbility;

                    public void bind(ConfigFile Config)
                    {
                        string prefix = "Range.Card.Modification";

                        minBaseAbility = Config.Bind(prefix, "minBaseAbility", 0);
                        maxBaseAbility = Config.Bind(prefix, "maxBaseAbility", 2);
                        minCustomAbility = Config.Bind(prefix, "minCustomAbility", 0);
                        maxCustomAbility = Config.Bind(prefix, "maxCustomAbility", 2);
                    }
                }

                public Modification modification;

                public void bind(ConfigFile Config)
                {
                    string prefix = "Range.Card";

                    modification = new Modification();

                    minAttack = Config.Bind(prefix, "minAttack", 1);
                    maxAttack = Config.Bind(prefix, "maxAttack", 9);
                    minHealth = Config.Bind(prefix, "minHealth", 1);
                    maxHealth = Config.Bind(prefix, "maxHealth", 9);
                    minCost = Config.Bind(prefix, "minCost", 0);
                    maxCost = Config.Bind(prefix, "maxCost", 3);

                    modification.bind(Config);
                }
            }

            public Card card;

            public void bind(ConfigFile Config)
            {
                card = new Card();
                card.bind(Config);
            }
        }

        public Behaviour behaviour;

        public Range range;

        public void bind(ConfigFile Config)
        {
            behaviour = new Behaviour();
            behaviour.bind(Config);
            range = new Range();
            range.bind(Config);
        }
    }
}