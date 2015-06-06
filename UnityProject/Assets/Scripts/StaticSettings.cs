namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Logic;

    using UnityEngine;

    public static class StaticSettings
    {
        public const float MinRotationDelay = 0.2f;

        public const float MaxRotationDelay = 2f;

        public const float DefaultProjectileLifespan = 5.0f;

        public static readonly Vector3 DefaultMoveDirection = new Vector3(0, 1, 0);

        public static readonly IDictionary<StatType, float> ArmorBaseStats = new Dictionary<StatType, float>();
 
        public static readonly IDictionary<StatType, float> WeaponBaseStats = new Dictionary<StatType, float>
        {
            { StatType.Damage, 1.0f },
            { StatType.Velocity, 1.0f },
            { StatType.Interval, 1.0f },
            { StatType.ProjectileLifeSpan, 5.0f }
        };

        public static IDictionary<StatType, float> PlayerBaseStats = new Dictionary<StatType, float>
        {
            { StatType.Health, 100.0f },
            { StatType.Velocity, 1.0f },
            { StatType.RotationSpeed, 1.0f },
            { StatType.HeatCoolingRate, 1.0f }
        };
    }
}
