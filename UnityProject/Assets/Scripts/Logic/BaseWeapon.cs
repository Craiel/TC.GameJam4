namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public abstract class BaseWeapon : BaseGear, IWeapon
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected BaseWeapon(StatDictionary stats)
        {
            // Make it all left by default and sort it out later
            this.Type = GearType.LeftWeapon;
            this.DamageType = DamageType.Projectile;

            StatDictionary baseStats = new StatDictionary();
            baseStats.Merge(StaticSettings.WeaponBaseStats);
            baseStats.Merge(stats);
            this.SetBaseStats(baseStats);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public long ShotsFired { get; private set; }
        
        public float LastShotFired { get; private set; }
        
        public long ProjectileLimit { get; protected set; }

        public DamageType DamageType { get; set; }

        public bool IsTargeted { get; set; }

        public virtual bool CanFire()
        {
            float currentTime = Time.time;
            if (currentTime < this.LastShotFired + this.GetCurrentStat(StatType.Interval))
            {
                return false;
            }

            return true;
        }

        public void SetWeaponGearType(GearType type)
        {
            this.Type = type;
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
