namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
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
        protected override IList<ProjectileBehavior> DoFire(GameObject origin, ICharacter source)
        {
            Vector3 forward = origin.transform.rotation * StaticSettings.DefaultMoveDirection;
            origin.layer = 2;
            RaycastHit2D ray = Physics2D.Raycast(origin.transform.position, forward, Mathf.Infinity, LayerMask.GetMask("Wall", "Mech"));
            Debug.DrawRay(origin.transform.position, forward, Color.red);
            origin.layer = 0;
            if (ray.collider == null)
            {
                return null;
            }

            GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, Vector3.zero, origin.transform.rotation);
            Vector3 scale = new Vector3(1, ray.distance, 0);
            Vector3 directionOffset = new Vector3(0, ray.distance / 2, 0);
            instance.transform.position = origin.transform.position;
            instance.transform.Translate(directionOffset);
            instance.transform.localScale = scale;

            var collider = instance.AddComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.size = new Vector2(1, 1);

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
            behavior.Origin = origin;

            return new List<ProjectileBehavior> { behavior };
        }

        public override void Update(GameObject origin)
        {
            //if (timeChanged + .1 <= Time.time)
            //{
            //    origin.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0f, 0f, 0f));
            //    origin.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, 0f));
            //}
                
        }
    }
}