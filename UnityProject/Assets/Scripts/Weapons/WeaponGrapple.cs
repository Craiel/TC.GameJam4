namespace Assets.Scripts.Weapons
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;
    using System.Collections.Generic;
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
                    { StatType.ProjectileLifeSpan, 10f},
                    { StatType.Interval, 5f },
                    { StatType.HeatGeneration, 1.0f },
                };

            this.SetBaseStats(stats);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public override void Update(GameObject origin)
        {
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
            behavior.LifeSpan = Time.time + this.GetCurrentStat(StatType.ProjectileLifeSpan); //Time.time + this.GetInternalStat(StatType.ProjectileLifeSpan);
            behavior.Origin = context.Origin;

            this.timeChanged = Time.time;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private bool RayHitTest(GameObject origin, out Vector3 distance)
        {
            int index = 0;
            float d = 0f;
            Vector3 forward = origin.transform.rotation * StaticSettings.DefaultMoveDirection;
            Transform[] T = origin.GetComponentsInChildren<Transform>();
            List<RaycastHit2D> ray = new List<RaycastHit2D>();
            int originalLayer = origin.layer;
            origin.layer = 2;
            ray.Add(Physics2D.Raycast(origin.transform.position, forward));
            for (int i = 0; i < T.Length; i++ )
            {
                ray.Add(Physics2D.Raycast(T[i].transform.position,forward));
            }
            origin.layer = originalLayer;
            for(int j = 0; j < ray.Count; j++)
            {
                if(ray[j].distance > d)
                {
                    d = ray[j].distance;
                    index = j;
                }
            }
            if(ray[index])
            {
                distance = ray[index].transform.position;
                return ray[index].collider != null;
            }
            else
            {
                distance = Vector3.zero;
                return false;
            }
            
        }
    }
}
