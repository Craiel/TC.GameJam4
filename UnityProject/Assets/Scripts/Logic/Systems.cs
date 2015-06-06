namespace Assets.Scripts.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Assets.Scripts.Armor;
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Weapons;

    using UnityEngine;

    using Random = UnityEngine.Random;

    public static class Systems
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static void ApplyDamage(GameObject target, float damage)
        {
            DestructibleTile destructibleTile = target.GetComponent<DestructibleTile>();
            if (destructibleTile != null)
            {
                destructibleTile.TakeDamage(damage);
                return;
            }

            PlayerCharacterBehavior characterBehavior = target.GetComponent<PlayerCharacterBehavior>();
            if (characterBehavior != null)
            {
                characterBehavior.Character.TakeDamage(damage);
            }
        }

        public static IGear GenerateRandomGear()
        {
            IList<GearType> types = Enum.GetValues(typeof(GearType)).Cast<GearType>().ToList();
            return GenerateRandomGear(types[Random.Range(0, types.Count)]);
        }

        public static IGear GenerateRandomGear(GearType type)
        {
            Debug.Log("Generating New " + type);

            switch (type)
            {
                case GearType.Head:
                    {
                        var rollData = new StatRollData { OptionalStatPicks = StaticSettings.HeadRollOptionalPicks };
                        rollData.RequiredStats.Add(StatType.Health);
                        rollData.OptionalStats.AddRange(StaticSettings.HeadRollMetaFlags);
                        rollData.BudgetValues.Merge(StaticSettings.HeadRollBudgets);
                        return new DefaultHeadArmor(null, GenerateRandomStats(rollData));
                    }

                case GearType.Chest:
                    {
                        var rollData = new StatRollData { OptionalStatPicks = StaticSettings.ChestRollOptionalPicks };
                        rollData.RequiredStats.Add(StatType.Health);
                        rollData.OptionalStats.AddRange(StaticSettings.ChestRollMetaFlags);
                        rollData.BudgetValues.Merge(StaticSettings.ChestRollBudgets);
                        return new DefaultChestArmor(null, GenerateRandomStats(rollData));
                    }

                case GearType.Legs:
                    {
                        var rollData = new StatRollData { OptionalStatPicks = StaticSettings.LegsRollOptionalPicks };
                        rollData.RequiredStats.Add(StatType.Health);
                        rollData.OptionalStats.AddRange(StaticSettings.LegsRollMetaFlags);
                        rollData.BudgetValues.Merge(StaticSettings.LegsRollBudgets);
                        return new DefaultLegArmor(null, GenerateRandomStats(rollData));
                    }

                case GearType.LeftWeapon:
                    {
                        return new WeaponColumn();
                    }

                case GearType.RightWeapon:
                    {
                        return new WeaponRanged();
                    }
            }
            return null;
        }

        /*public static IArmor GenerateRandomHead()
        {
            var data = new StatRollData();
            data.RequiredStats.Add(StatType.Health);
            data.OptionalStats.AddRange(StaticSettings.HeadRollMetaFlags);

            StatDictionary stats = GenerateRandomStats(data);
            var item = new DefaultHeadArmor(stats);
        }*/

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private static StatDictionary GenerateRandomStats(StatRollData data)
        {
            // First we choose which stats we will take
            IList<StatType> chosenStats = new List<StatType>(data.RequiredStats);
            IList<StatType> optionalStatsLeft = data.OptionalStats.Where(x => !chosenStats.Contains(x)).ToList();
            if (optionalStatsLeft.Count < data.OptionalStatPicks)
            {
                chosenStats.AddRange(optionalStatsLeft);
            }
            else
            {
                // We have enough optionals to pick so pick random
                for(var i = 0; i < data.OptionalStatPicks; i++)
                {
                    StatType type = optionalStatsLeft[Random.Range(0, optionalStatsLeft.Count)];
                    chosenStats.Add(type);
                    optionalStatsLeft.Remove(type);
                }
            }
            
            // Now we do a roll on those stats to figure out the distribution
            var baseRoll = chosenStats.ToDictionary(type => type, type => Random.Range(0.1f, 1f));
            var baseRollSum = baseRoll.Sum(x => x.Value);
            var normalizeMultiplier = 1.0f / baseRollSum;

            // Now we build the resulting dictionary based on the roll against a 1.0f margin of the budget
            var result = new StatDictionary();
            foreach (StatType type in baseRoll.Keys)
            {
                if (!data.BudgetValues.ContainsKey(type))
                {
                    continue;
                }

                float budget = data.BudgetValues[type];
                float normalized = baseRoll[type] * normalizeMultiplier;
                float score = budget * normalized;
                Debug.Log(string.Format("Roll:{0}  Normlized:{1}  Budget:{2}  Score:{3}", baseRoll[type], normalized, budget, score));
                result.SetStat(type, score);
            }

            return result;
        }
    }
}