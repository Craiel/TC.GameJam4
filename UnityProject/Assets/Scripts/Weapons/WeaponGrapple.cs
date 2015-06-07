namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
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
        // Protected
        // -------------------------------------------------------------------
        protected override IList<ProjectileBehavior> DoFire(GameObject origin, ICharacter source)
        {
            Vector3 forward = origin.transform.rotation * StaticSettings.DefaultMoveDirection;
            origin.layer = 2;
            RaycastHit2D ray = Physics2D.Raycast(origin.transform.position, forward);
            Debug.DrawRay(origin.transform.position, forward, Color.red);
            origin.layer = 0;
            if (ray.collider != null)
            {
                Vector3 depthOffset = new Vector3(0, 0, -1f);
                GameObject instance = (GameObject)Object.Instantiate(projectilePrefab, ray.transform.position, origin.transform.rotation);
                LineRenderer line = instance.GetComponent<LineRenderer>();
                line.SetPosition(0, instance.transform.position + depthOffset);
                line.SetPosition(1, ray.transform.position + depthOffset);

                
                StaticProjectileBehavior behavior = instance.AddComponent<StaticProjectileBehavior>();
                behavior.DamageInfo = new CombatInfo
                {
                    Damage = this.GetCurrentStat(StatType.Damage),
                    DamageType = this.DamageType
                };
                behavior.Type = ProjectileType.grapple;
                behavior.LifeSpan = Time.time + .5f; //Time.time + this.GetInternalStat(StatType.ProjectileLifeSpan);
                behavior.Origin = origin;
            }
            timeChanged = Time.time;
            return null;
        }

        public override void Update(GameObject origin)
        {
            if (timeChanged + 1 <= Time.time)
            {
                var a = origin.GetComponent<LineRenderer>();
                //a.enabled = false;
                a.SetPosition(0, Vector3.zero);
                a.SetPosition(1, Vector3.zero);
                //a.enabled = true;
            }

        }
    }
}
