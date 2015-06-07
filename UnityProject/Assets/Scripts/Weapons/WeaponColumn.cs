namespace Assets.Scripts.Weapons
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public class WeaponColumn : BaseWeapon
    {
        private readonly Object projectilePrefab;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public WeaponColumn(StatDictionary internalStats)
            : base(internalStats)
        {
            this.Name = "Beam";

            this.projectilePrefab = Resources.Load("Projectiles/EnergyBullet");

            var stats = new StatDictionary
                {
                    { StatType.Velocity, 0.1f },
                    { StatType.ProjectileLifeSpan, .9f },
                    { StatType.Interval, 0.85f }
                };

            stats.Merge(internalStats);
            this.SetBaseStats(stats);
        }
        
        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void DoFire(WeaponFireContext context)
        {
            float distance;
            if (!this.RayHitTest(context.Origin, out distance))
            {
                return;
            }

            GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, Vector3.zero, context.Origin.transform.rotation);
            Vector3 scale = new Vector3(1, distance, 0);
            Vector3 directionOffset = new Vector3(0, distance / 2.0f, 0);
            instance.transform.position = context.Origin.transform.position;
            instance.transform.Translate(directionOffset);
            instance.transform.localScale = scale;

            var collider = instance.AddComponent<PolygonCollider2D>();
            collider.isTrigger = true;

            StaticProjectileBehavior behavior = instance.AddComponent<StaticProjectileBehavior>();
            behavior.DamageInfo = new CombatInfo
            {
                Damage = this.GetCurrentStat(StatType.Damage),
                DamageType = this.DamageType,
                ModValue = 0.33f,
                LogNMultiplier = 5f
            };

            behavior.Type = ProjectileType.beam;
            behavior.LifeSpan = Time.time + this.GetCurrentStat(StatType.ProjectileLifeSpan);
            behavior.Origin = context.Origin;
        }

        private bool RayHitTest(GameObject origin, out float distance)
        {
            Vector3 forward = origin.transform.rotation * StaticSettings.DefaultMoveDirection;
            int originalLayer = origin.layer;
            origin.layer = 2;
            RaycastHit2D ray = Physics2D.Raycast(origin.transform.position, forward, Mathf.Infinity, LayerMask.GetMask("Wall", "Mech"));
            Debug.DrawRay(origin.transform.position, forward, Color.red);
            origin.layer = originalLayer;
            distance = ray.distance;
            return ray.collider != null;
        }
    }
}