namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    public interface IWeapon
    {
        long ShotsFired { get; }

        float LastShotFired { get; }

        float Interval { get; }

        float ProjectilesPerShot { get; }

        long ProjectileLimit { get; }

        float ProjectileLifetime { get; }

        bool CanFire();

        IList<IProjectile> Fire();
    }
}
