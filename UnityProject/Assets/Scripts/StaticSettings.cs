namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Logic;

    using UnityEngine;

    public static class StaticSettings
    {
        public const int MaxPlayerCount = 4;
        public const float MinRotationDelay = 0.2f;

        public const float MaxRotationDelay = 2f;

        public const float DefaultProjectileLifespan = 5.0f;

        public const string MapFileFilter = "/Resources/Maps/{0}.txt";

        public static readonly Vector3 DefaultMoveDirection = new Vector3(0, 1, 0);

        public static readonly StatDictionary ArmorBaseStats = new StatDictionary();

        public static readonly StatDictionary WeaponBaseStats = new StatDictionary
        {
            { StatType.Damage, 1.0f },
            { StatType.Velocity, 1.0f },
            { StatType.Interval, 1.0f },
            { StatType.ProjectileLifeSpan, 5.0f }
        };

        public static StatDictionary PlayerBaseStats = new StatDictionary
        {
            { StatType.Health, 100.0f },
            { StatType.Velocity, 1.0f },
            { StatType.RotationSpeed, 1.0f },
            { StatType.HeatCoolingRate, 1.0f }
        };



        public static bool EnableInControl = false;

        public static IList<StatType> HeadRollMetaFlags = new List<StatType>
                                                          {
                                                              StatType.RangedAccuracy,
                                                              StatType.MeleeAccuracy,
                                                              StatType.TargetingDistance,
                                                              StatType.TargetingLockTime
                                                          };

        public static StatDictionary HeadRollBudgets = new StatDictionary
                                                           {
                                                               { StatType.RangedAccuracy, 0.75f },
                                                               { StatType.MeleeAccuracy, 0.75f },
                                                               { StatType.TargetingDistance, 30f },
                                                               { StatType.TargetingLockTime, -1f },
                                                               { StatType.Health, 100f }
                                                           };

        public static IList<StatType> ChestRollMetaFlags = new List<StatType>
                                                          {
                                                              StatType.RangedAccuracy,
                                                              StatType.Armor,
                                                              StatType.Shield,
                                                              StatType.RotationSpeed,
                                                              StatType.Velocity,
                                                              StatType.HeatCoolingRate,
                                                          };

        public static StatDictionary ChestRollBudgets = new StatDictionary
                                                           {
                                                               { StatType.RangedAccuracy, 0.75f },
                                                               { StatType.Armor, 10f },
                                                               { StatType.Shield, 20f },
                                                               { StatType.RotationSpeed, 1f },
                                                               { StatType.Velocity, 1f },
                                                               { StatType.HeatCoolingRate, 5f },
                                                               { StatType.Health, 200f }
                                                           };

        public static IList<StatType> LegsRollMetaFlags = new List<StatType>
                                                          {
                                                              StatType.MeleeAccuracy,
                                                              StatType.RotationSpeed,
                                                              StatType.Velocity,
                                                              StatType.HeatCoolingRate
                                                          };

        public static StatDictionary LegsRollBudgets = new StatDictionary
                                                           {
                                                               { StatType.MeleeAccuracy, 0.75f },
                                                               { StatType.RotationSpeed, 1f },
                                                               { StatType.Velocity, 1f },
                                                               { StatType.HeatCoolingRate, 5f },
                                                               { StatType.Health, 100f },
                                                               { StatType.HeatGeneration, -9f }
                                                           };

        public const int HeadRollOptionalPicks = 3;

        public const int ChestRollOptionalPicks = 3;

        public const int LegsRollOptionalPicks = 3;

        public static int NumGearDropsPerCharacterAtStart = 2;
    }
}
