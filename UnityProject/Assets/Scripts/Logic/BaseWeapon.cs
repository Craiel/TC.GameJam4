namespace Assets.Scripts.Logic
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    public abstract class BaseWeapon : IWeapon
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected BaseWeapon()
        {
            this.Interval = 1.0f;
            this.Damage = 1.0f;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public long ShotsFired { get; private set; }

        public float Damage { get; protected set; }

        public float LastShotFired { get; private set; }

        public float Interval { get; protected set; }

        public float ProjectilesPerShot { get; protected set; }

        public long ProjectileLimit { get; protected set; }

        public float ProjectileLifetime { get; protected set; }

        public virtual bool CanFire()
        {
            float currentTime = Time.time;
            if (currentTime < this.LastShotFired + this.Interval)
            {
                return false;
            }

            return true;
        }

        public IList<IProjectile> Fire()
        {
            if (this.CanFire())
            {
                this.LastShotFired = Time.time;

                IList<IProjectile> projectiles = this.DoFire();
                this.ShotsFired += projectiles.Count;
                return projectiles;
            }

            return null;
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected abstract IList<IProjectile> DoFire();
    }
}
