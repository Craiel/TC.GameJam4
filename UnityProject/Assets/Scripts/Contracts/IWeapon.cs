namespace Assets.Scripts.Contracts
{
    using System.Collections.Generic;

    using Assets.Scripts.Logic;

    using UnityEngine;

    public interface IWeapon : IGear
    {
        long ShotsFired { get; }

        float LastShotFired { get; }
        
        long ProjectileLimit { get; }

        bool IsTargeted { get; set; }

        WeaponType WeaponType { get; set; }

        bool CanFire();

        void SetWeaponGearType(GearType type);

        IList<ProjectileBehavior> Fire(GameObject origin, ICharacter source);
    }
}
