namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public class WeaponMelee : BaseWeapon
    {
        private readonly Object meleePrefab;
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public WeaponMelee(StatDictionary internalStats)
            : base(internalStats)
        {
            this.Name = "Melee";

            this.meleePrefab = Resources.Load("Projectiles/EnergySaber");

            var stats = new StatDictionary
                {
                    { StatType.Interval, 0.1f },
                    { StatType.HeatGeneration, 1.0f },
                };

            this.SetBaseStats(stats);
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void DoFire(WeaponFireContext context)
        {
            GameObject instance = (GameObject)Object.Instantiate(this.meleePrefab, context.Origin.transform.position, context.Origin.transform.rotation);
            instance.GetComponent<Animator>().SetTrigger("Sword360");
            MeleeProjectileBehavior behavior = instance.AddComponent<MeleeProjectileBehavior>();
            behavior.DamageInfo = new CombatInfo
            {
                Damage = this.GetCurrentStat(StatType.Damage),
                DamageType = this.DamageType,
                CombatType = CombatType.Ranged
            };
            behavior.Type = ProjectileType.melee;
            behavior.LifeSpan = Time.time + .5f;
            behavior.Origin = context.Origin;
        }
    }
}
