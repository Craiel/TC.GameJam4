namespace Assets.Scripts.Weapons
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

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

            BombProjectileBehavior behavior = instance.AddComponent<BombProjectileBehavior>();
            behavior.DamageInfo = new CombatInfo
            {
                Damage = this.GetCurrentStat(StatType.Damage),
                DamageType = this.DamageType,
                ModValue = 1f,
                LogNMultiplier = 5f
            };
            behavior.Type = ProjectileType.bomb;
            behavior.LifeSpan = Time.time + 5f;
            behavior.Origin = context.Origin;
        }
    }
}
