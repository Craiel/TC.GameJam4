namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    using UnityEngine;

    public class WeaponBomb : BaseWeapon
    {
        private readonly Object projectilePrefab;
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public WeaponBomb(StatDictionary internalStats)
            : base(internalStats)
        {
            this.Name = "Bomb";
            this.projectilePrefab = Resources.Load("Projectiles/Bomb");

            var stats = new StatDictionary
                {
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

            StaticProjectileBehavior behavior = instance.AddComponent<StaticProjectileBehavior>();
            behavior.DamageInfo = new CombatInfo
            {
                Damage = this.GetInternalStat(StatType.Damage),
                Type = this.DamageType
            };
            behavior.LifeSpan = Time.time + 10f;
            behavior.Origin = origin;

            return new List<ProjectileBehavior> { behavior };
        }
    }
}
