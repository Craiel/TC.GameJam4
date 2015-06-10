namespace Assets.Scripts.Weapons
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public class WeaponRanged : BaseWeapon
    {
        private readonly Object projectilePrefab;
        
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public WeaponRanged(StatDictionary internalStats)
            : base(internalStats)
        {
            this.Name = "Ranged";

            this.projectilePrefab = Resources.Load("Projectiles/Bullet");

            var stats = new StatDictionary
                {
                    { StatType.Velocity, 0.8f },
                    { StatType.ProjectileLifeSpan, 1f },
                    { StatType.Interval, 0.5f },
                    { StatType.HeatGeneration, 1.0f },
                };

            stats.Merge(internalStats);
            this.SetBaseStats(stats);
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void DoFire(WeaponFireContext context)
        {
            GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, context.Origin.transform.position, context.Origin.transform.rotation);
            instance.transform.SetParent(context.ProjectileParent.transform);

            BulletProjectileBehavior behavior = instance.AddComponent<BulletProjectileBehavior>();
            behavior.DamageInfo = new CombatInfo
                                      {
                                          Damage = this.GetCurrentStat(StatType.Damage),
                                          DamageType = this.DamageType,
                                          CombatType = CombatType.Ranged,
                                          ModValue = 0.75f,
                                          LogNMultiplier = 5f
                                      };
            behavior.Type = ProjectileType.bullet;
            behavior.Velocity = this.GetCurrentStat(StatType.Velocity);
            behavior.LifeSpan = Time.time + this.GetCurrentStat(StatType.ProjectileLifeSpan);
            behavior.Origin = context.Origin;
        }
    }
}
