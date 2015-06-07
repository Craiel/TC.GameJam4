namespace Assets.Scripts.Weapons
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public class WeaponGrapple : BaseWeapon
    {
        private readonly Object projectilePrefab;
        private float timeChanged;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public WeaponGrapple(StatDictionary internalStats)
            : base(internalStats)
        {
            this.Name = "Grapple";
            this.projectilePrefab = Resources.Load("Projectiles/GrappleHook");

            var stats = new StatDictionary
                {
                    { StatType.Interval, 0.1f },
                    { StatType.HeatGeneration, 1.0f },
                };

            this.SetBaseStats(stats);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public override void Update(GameObject origin)
        {
            if (this.timeChanged + 1 <= Time.time)
            {
                var a = origin.GetComponent<LineRenderer>();
                a.SetColors(new Color(0, 0, 0, 0), new Color(0, 0, 0, 0));
                a.SetPosition(0, Vector3.zero);
                a.SetPosition(1, Vector3.zero);
            }
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void DoFire(WeaponFireContext context)
        {
            Vector3 rayPosition;
            if (!this.RayHitTest(context.Origin, out rayPosition))
            {
                return;
            }

            Vector3 depthOffset = new Vector3(0, 0, -1f);
            LineRenderer line = context.Origin.GetComponent<LineRenderer>();
            line.SetPosition(0, context.Origin.transform.position + depthOffset);
            line.SetPosition(1, rayPosition + depthOffset);

            GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, rayPosition, context.Origin.transform.rotation);
            GrappleProjectileBehavior behavior = instance.AddComponent<GrappleProjectileBehavior>();
            behavior.DamageInfo = new CombatInfo
            {
                Damage = this.GetCurrentStat(StatType.Damage),
                DamageType = this.DamageType
            };
            behavior.Type = ProjectileType.grapple;
            behavior.LifeSpan = Time.time + .5f; //Time.time + this.GetInternalStat(StatType.ProjectileLifeSpan);
            behavior.Origin = context.Origin;

            this.timeChanged = Time.time;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private bool RayHitTest(GameObject origin, out Vector3 distance)
        {
            Vector3 forward = origin.transform.rotation * StaticSettings.DefaultMoveDirection;
            int originalLayer = origin.layer;
            origin.layer = 2;
            RaycastHit2D ray = Physics2D.Raycast(origin.transform.position, forward);
            Debug.DrawRay(origin.transform.position, forward, Color.red);
            origin.layer = originalLayer;
            distance = ray.transform.position;
            return ray.collider != null;
        }
    }
}
