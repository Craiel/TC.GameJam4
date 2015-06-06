namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using UnityEngine;

    public abstract class BaseWeapon : BaseGear, IWeapon
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected BaseWeapon()
        {
            foreach (StatType type in StaticSettings.WeaponBaseStats.Keys)
            {
                this.SetStat(type, StaticSettings.WeaponBaseStats[type]);
            }
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public long ShotsFired { get; private set; }
        
        public float LastShotFired { get; private set; }
        
        public long ProjectileLimit { get; protected set; }

        public virtual bool CanFire()
        {
            float currentTime = Time.time;
            if (currentTime < this.LastShotFired + this.GetStat(StatType.Interval))
            {
                return false;
            }

            return true;
        }

        public IList<ProjectileBehavior> Fire(GameObject origin, ICharacter source)
        {
            if (this.CanFire())
            {
                this.LastShotFired = Time.time;

                IList<ProjectileBehavior> projectiles = this.DoFire(origin, source);
                this.ShotsFired++;
                return projectiles;
            }

            return null;
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected abstract IList<ProjectileBehavior> DoFire(GameObject origin, ICharacter source);
    }
}
