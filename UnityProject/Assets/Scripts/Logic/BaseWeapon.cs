namespace Assets.Scripts.Logic
{
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

        public void Fire(WeaponFireContext context)
        {
            if (this.CanFire())
            {
                float maxHeat = this.GetMaxStat(StatType.Heat);
                if (maxHeat > 0)
                {
                    float heatIncrease = this.GetCurrentStat(StatType.HeatGeneration);
                    this.ModifyStat(StatType.Heat, heatIncrease);
                }

                this.LastShotFired = Time.time;

                this.DoFire(context);
                this.ShotsFired++;
            }
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected abstract void DoFire(WeaponFireContext context);
    }
}
