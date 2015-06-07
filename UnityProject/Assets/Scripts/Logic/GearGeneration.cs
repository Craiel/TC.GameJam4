namespace Assets.Scripts.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Assets.Scripts.Armor;
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;
    
    public static class GearGeneration
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public static IGear GenerateRandomGear()
        {
            return GenerateRandomGear(EnumLists.GearTypes[UnityEngine.Random.Range(0, EnumLists.GearTypes.Count)]);
        }

        public static IWeapon GenerateRandomWeapon(GearType type, Type weaponType, DamageType? damageType = null)
        {
            switch (type)
            {
                case GearType.LeftWeapon:
                    {
                        IWeapon weapon = (IWeapon)Activator.CreateInstance(weaponType, GetRandomWeaponStats());
                        weapon.SetWeaponGearType(GearType.LeftWeapon);
                        weapon.DamageType = damageType ?? PickRandomWeaponType();
                        return weapon;
                    }

                case GearType.RightWeapon:
                    {
                        IWeapon weapon = (IWeapon)Activator.CreateInstance(weaponType, GetRandomWeaponStats());
                        weapon.IsTargeted = true;
                        weapon.SetWeaponGearType(GearType.RightWeapon);
                        weapon.DamageType = damageType ?? PickRandomWeaponType();
                        return weapon;
                    }

                default:
                    {
                        throw new InvalidOperationException();
                    }
            }
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
                        rollData.InternalStats.AddRange(StaticSettings.ArmorInternalStats);

                        return CreateRandomArmor<DefaultHeadArmor>(rollData);
                    }

                case GearType.Chest:
                    {
                        var rollData = new StatRollData { OptionalStatPicks = StaticSettings.ChestRollOptionalPicks };
                        rollData.RequiredStats.Add(StatType.Health);
                        rollData.OptionalStats.AddRange(StaticSettings.ChestRollMetaFlags);
                        rollData.BudgetValues.Merge(StaticSettings.ChestRollBudgets);
                        rollData.InternalStats.AddRange(StaticSettings.ArmorInternalStats);

                        return CreateRandomArmor<DefaultChestArmor>(rollData);
                    }

                case GearType.Legs:
                    {
                        var rollData = new StatRollData { OptionalStatPicks = StaticSettings.LegsRollOptionalPicks };
                        rollData.RequiredStats.Add(StatType.Health);
                        rollData.RequiredStats.Add(StatType.HeatGeneration);
                        rollData.RequiredStats.Add(StatType.Heat);
                        rollData.OptionalStats.AddRange(StaticSettings.LegsRollMetaFlags);
                        rollData.BudgetValues.Merge(StaticSettings.LegsRollBudgets);
                        rollData.BaseLineValues.Merge(StaticSettings.LegsRollBaseLineStats);
                        rollData.InternalStats.AddRange(StaticSettings.ArmorInternalStats);

                        return CreateRandomArmor<DefaultLegArmor>(rollData);
                    }

                case GearType.LeftWeapon:
                    {
                        Type weaponType = StaticSettings.LeftHandWeaponTypes[UnityEngine.Random.Range(0, StaticSettings.LeftHandWeaponTypes.Count)];
                        return GenerateRandomWeapon(type, weaponType);
                    }

                case GearType.RightWeapon:
                    {
                        Type weaponType = StaticSettings.RightHandWeaponTypes[UnityEngine.Random.Range(0, StaticSettings.RightHandWeaponTypes.Count)];
                        return GenerateRandomWeapon(type, weaponType);
                    }
            }
            return null;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private static IArmor CreateRandomArmor<T>(StatRollData rollData)
        {
            StatDictionary result = GenerateRandomStats(rollData);

            StatDictionary inheritedStats = new StatDictionary();
            StatDictionary internalStats = new StatDictionary();
            foreach (KeyValuePair<StatType, float> dataPair in result)
            {
                if (rollData.InternalStats.Contains(dataPair.Key))
                {
                    internalStats.Add(dataPair.Key, dataPair.Value);
                }
                else
                {
                    inheritedStats.Add(dataPair.Key, dataPair.Value);
                }
            }

            return (IArmor)Activator.CreateInstance(typeof(T), internalStats, inheritedStats);
        }

        private static StatDictionary GetRandomWeaponStats()
        {
            var rollData = new StatRollData();
            rollData.RequiredStats.AddRange(StaticSettings.WeaponRollFlags);
            rollData.BudgetValues.AddRange(StaticSettings.WeaponRollBudgets);
            rollData.BaseLineValues.AddRange(StaticSettings.WeaponRollBaseLineStats);
            return GenerateRandomStats(rollData);
        }

        private static DamageType PickRandomWeaponType()
        {
            return EnumLists.DamageTypes[UnityEngine.Random.Range(0, EnumLists.DamageTypes.Count)];
        }

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
                    StatType type = optionalStatsLeft[UnityEngine.Random.Range(0, optionalStatsLeft.Count)];
                    chosenStats.Add(type);
                    optionalStatsLeft.Remove(type);
                }
            }
            
            // Now we do a roll on those stats to figure out the distribution
            var baseRoll = chosenStats.ToDictionary(type => type, type => UnityEngine.Random.Range(0.1f, 1f));
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
                float score;
                if (data.BaseLineValues.ContainsKey(type))
                {
                    float baseLine = data.BaseLineValues[type];
                    score = baseLine + (budget * normalized);
                }
                else
                {
                    score = budget * normalized;
                }

                Debug.Log(string.Format("Roll:{0}  Normlized:{1}  Budget:{2}  Score:{3}", baseRoll[type], normalized, budget, score));
                result.SetStat(type, score);
            }

            return result;
        }
    }
}