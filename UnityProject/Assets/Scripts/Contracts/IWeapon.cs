namespace Assets.Scripts.Contracts
{
    using System.Collections.Generic;
    
    public interface IWeapon : IGear
    {
        long ShotsFired { get; }

        float LastShotFired { get; }
        
        float ProjectilesPerShot { get; }

        long ProjectileLimit { get; }

        bool CanFire();

        IList<IProjectile> Fire();
    }
}
