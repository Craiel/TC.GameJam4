namespace Assets.Scripts.Contracts
{
    using System.Collections.Generic;

    using UnityEngine;

    public interface IWeapon : IGear
    {
        long ShotsFired { get; }

        float LastShotFired { get; }
        
        long ProjectileLimit { get; }

        bool CanFire();

        IList<ProjectileBehavior> Fire(GameObject origin, ICharacter source);
    }
}
