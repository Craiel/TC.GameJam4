namespace Assets.Scripts
{
    using System;
    using System.Collections.Generic;

    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;
    using Assets.Scripts.Weapons;

    using UnityEngine;

    public static class StaticSettings
    {
        public const int MaxPlayerCount = 4;
        public const float MinRotationDelay = 0.2f;

        public const float MaxRotationDelay = 2f;

        public const float DefaultProjectileLifespan = 5.0f;

        public const string MapFileFilter = "/Resources/Maps/{0}.txt";

        public static readonly Vector3 DefaultMoveDirection = new Vector3(0, 1, 0);

        public static readonly float DefaultProjectileMoveSpeed = 10f;

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
            { StatType.Scale, 1.0f },
            { StatType.Health, 100.0f },
            { StatType.Velocity, 1.0f },
            { StatType.RotationSpeed, 1.0f },
            { StatType.HeatCoolingRate, 1.0f },
            { StatType.TargetingLockTime, 2f},
            { StatType.TargetingDistance, 20f},
        };

        public static bool EnableInControl = false;

        public static IList<StatType> ArmorInternalStats = new List<StatType>
                                                          {
                                                              StatType.Health,
                                                              StatType.Heat,
                                                              StatType.HeatGeneration
                                                          };

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
                                                               { StatType.Heat, 100f },
                                                               { StatType.HeatGeneration, -9f }
                                                           };

        public static StatDictionary LegsRollBaseLineStats = new StatDictionary
                                                           {
                                                               { StatType.HeatGeneration, 10f },
                                                           };

        public static IList<StatType> WeaponRollFlags = new List<StatType>
                                                          {
                                                              StatType.Damage,
                                                              StatType.Health,
                                                              StatType.HeatGeneration
                                                          };

        public static StatDictionary WeaponRollBudgets = new StatDictionary
                                                           {
                                                               { StatType.Damage, 50f },
                                                               { StatType.Health, 200f },
                                                               { StatType.HeatGeneration, -19f },
                                                           };

        public static StatDictionary WeaponRollBaseLineStats = new StatDictionary
                                                           {
                                                               { StatType.HeatGeneration, 20f },
                                                           };

        public const int HeadRollOptionalPicks = 3;

        public const int ChestRollOptionalPicks = 3;

        public const int LegsRollOptionalPicks = 3;

        public static int NumGearDropsPerCharacterAtStart = 2;
        
        public static IList<Type> LeftHandWeaponTypes = new List<Type>
                                                            {
                                                                typeof(WeaponColumn),
                                                                typeof(WeaponRanged),
                                                                //typeof(WeaponMelee),
                                                                //typeof(WeaponHeal),
                                                                //typeof(WeaponSpeedBoost),
                                                                typeof(WeaponBomb),
                                                                //typeof(WeaponInvisibility)
                                                            };

        public static IList<Type> RightHandWeaponTypes = new List<Type>
                                                            {
                                                                //typeof(WeaponRanged),
                                                                //typeof(WeaponMelee),
                                                                typeof(WeaponGrapple),
                                                                //typeof(WeaponSlow),
                                                                //typeof(WeaponHeat),
                                                                //typeof(WeaponHoming)
                                                            };

        public static IList<StatType> PersistentStats = new List<StatType>
                                                          {
                                                              StatType.Health,
                                                              StatType.Heat
                                                          };
    }
}
