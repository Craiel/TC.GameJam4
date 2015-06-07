using Assets.Scripts.Logic;
using System.Collections.Generic;

using Assets.Scripts.Logic.Enums;

public class MechLoadouts
{
    public static MechLoadout wilsonInterceptor = new MechLoadout
    {
        Name = "Wilson Interceptor",
        BasicStats = new StatDictionary
        {
            { StatType.Scale, 0.8f },
            { StatType.RangedAccuracy, 0.25f },
            { StatType.MeleeAccuracy, 0.8f},
            { StatType.TargetingDistance, 15f},
            { StatType.TargetingLockTime, 1f},
            { StatType.Armor, 0f},
            { StatType.Shield, 2f},
            { StatType.RotationSpeed, 1.25f},
            { StatType.Velocity, 1.25f},
            { StatType.HeatCoolingRate, 2f},
            { StatType.Health, 30f}
        }
    };

    public static MechLoadout blahutaPlatform = new MechLoadout
    {
        Name = "Blahuta Platform",
        BasicStats = new StatDictionary
        {
            { StatType.Scale, 0.9f },
            { StatType.RangedAccuracy, 0.75f },
            { StatType.MeleeAccuracy, 0.3f},
            { StatType.TargetingDistance, 30f},
            { StatType.TargetingLockTime, 1f},
            { StatType.Armor, 0f},
            { StatType.Shield, 0f},
            { StatType.RotationSpeed, 0.8f},
            { StatType.Velocity, 1.2f},
            { StatType.HeatCoolingRate, 1f},
            { StatType.Health, 40f}
        }
    };

    public static MechLoadout gustaevelMarkIV = new MechLoadout
    {
        Name = "Gustaevel MK.IV",
        BasicStats = new StatDictionary
        {
            { StatType.Scale, 1.0f },
            { StatType.RangedAccuracy, 0.5f },
            { StatType.MeleeAccuracy, 0.5f},
            { StatType.TargetingDistance, 20f},
            { StatType.TargetingLockTime, 3f},
            { StatType.Armor, 4f},
            { StatType.Shield, 4f},
            { StatType.RotationSpeed, 0.7f},
            { StatType.Velocity, 0.7f},
            { StatType.HeatCoolingRate, 2f},
            { StatType.Health, 60f}
        }
    };

    public static MechLoadout tWilliams = new MechLoadout
    {
        Name = "T-Williams",
        BasicStats = new StatDictionary
        {
            { StatType.Scale, 1.0f },
            { StatType.RangedAccuracy, 0.7f },
            { StatType.MeleeAccuracy, 0.25f},
            { StatType.TargetingDistance, 25f},
            { StatType.TargetingLockTime, 2f},
            { StatType.Armor, 2f},
            { StatType.Shield, 0f},
            { StatType.RotationSpeed, 1.2f},
            { StatType.Velocity, 1f},
            { StatType.HeatCoolingRate, 3f},
            { StatType.Health, 50f}
        }
    };

    public static List<MechLoadout> Loadouts = new List<MechLoadout> { wilsonInterceptor, blahutaPlatform, gustaevelMarkIV, tWilliams};

    public class MechLoadout
    {
        public string Name { get; set; }
        public StatDictionary BasicStats { get; set; }
    }
}