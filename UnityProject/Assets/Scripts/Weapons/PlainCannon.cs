namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    using UnityEngine;

    public class PlainCannon : BaseWeapon
    {
        private readonly Object projectilePrefab;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public PlainCannon()
        {
            this.projectilePrefab = Resources.Load("Projectiles/Bullet");

            this.SetStat(StatType.Velocity, 0.1f);
            this.SetStat(StatType.ProjectileLifeSpan, 1f);
            this.SetStat(StatType.Interval, 0.1f);
            this.SetStat(StatType.HeatGeneration, 1.0f);
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override IList<ProjectileBehavior> DoFire(GameObject origin, ICharacter source)
        {
            GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, origin.transform.position, origin.transform.rotation);

            BulletProjectileBehavior behavior = instance.AddComponent<BulletProjectileBehavior>();
            behavior.Damage = this.GetStat(StatType.Damage);
            behavior.Velocity = this.GetStat(StatType.Velocity);
            behavior.LifeSpan = Time.time + this.GetStat(StatType.ProjectileLifeSpan);
            behavior.Origin = origin;

            return new List<ProjectileBehavior> { behavior };
        }
    }
}
