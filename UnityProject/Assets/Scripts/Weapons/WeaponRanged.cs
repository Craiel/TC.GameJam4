namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
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
                    { StatType.Velocity, 0.1f },
                    { StatType.ProjectileLifeSpan, 1f },
                    { StatType.Interval, 0.1f },
                    { StatType.HeatGeneration, 1.0f },
                };

            this.InternalStats.Merge(stats);
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override IList<ProjectileBehavior> DoFire(GameObject origin, ICharacter source)
        {
            GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, origin.transform.position, origin.transform.rotation);

            BulletProjectileBehavior behavior = instance.AddComponent<BulletProjectileBehavior>();
            behavior.DamageInfo = new CombatInfo
                                      {
                                          Damage = this.GetInternalStat(StatType.Damage),
                                          DamageType = this.DamageType,
                                          CombatType = CombatType.Ranged
                                      };
            behavior.Velocity = this.GetInternalStat(StatType.Velocity);
            behavior.LifeSpan = Time.time + this.GetInternalStat(StatType.ProjectileLifeSpan);
            behavior.Origin = origin;

            return new List<ProjectileBehavior> { behavior };
        }
    }
}
