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
        private float timeChanged;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public WeaponColumn(StatDictionary internalStats)
            : base(internalStats)
        {
            this.Name = "Beam";

            var stats = new StatDictionary
                {
                    { StatType.Velocity, 0.1f },
                    { StatType.ProjectileLifeSpan, .001f },
                    { StatType.Interval, 0.1f }
                };

            this.InternalStats.Merge(stats);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override IList<ProjectileBehavior> DoFire(GameObject origin, ICharacter source)
        {
            //this.target.transform.Translate(StaticSettings.DefaultMoveDirection * move);
            //this.target.transform.Rotate(Vector3.forward, rotate);
            Vector3 forward = origin.transform.rotation * StaticSettings.DefaultMoveDirection;
            origin.layer = 2;
            RaycastHit2D ray = Physics2D.Raycast(origin.transform.position, forward);
            Debug.DrawRay(origin.transform.position, forward, Color.red);
            origin.layer = 0;
            if (ray.collider != null)
            {
                //Vector3 depthOffset = new Vector3(0, 0, -1f);
                //LineRenderer line = origin.GetComponent<LineRenderer>();
                //line.SetPosition(0, origin.transform.position + depthOffset);
                //line.SetPosition(1, ray.transform.position + depthOffset);
                //Debug.Log(origin.name);
                GameObject instance = (GameObject)Object.Instantiate(Resources.Load("Projectiles/EnergyBullet"), Vector3.zero, origin.transform.rotation);
                Vector3 scale = new Vector3(1, ray.distance, 0);
                Vector3 directionOffset = new Vector3(0, ray.distance / 2, 0);
                instance.transform.position = origin.transform.position;
                instance.transform.Translate(directionOffset);
                instance.transform.localScale = scale;
                instance.AddComponent<BoxCollider2D>();
                instance.GetComponent<BoxCollider2D>().isTrigger = true;
                instance.GetComponent<BoxCollider2D>().size = new Vector2(1, 1);
                StaticProjectileBehavior behavior = instance.AddComponent<StaticProjectileBehavior>();
                behavior.DamageInfo = new CombatInfo
                {
                    Damage = this.GetInternalStat(StatType.Damage),
                    DamageType = this.DamageType
                };
                behavior.Type = ProjectileType.beam;
                behavior.LifeSpan = Time.time + .5f; //Time.time + this.GetInternalStat(StatType.ProjectileLifeSpan);
                behavior.Origin = origin;

                timeChanged = Time.time;
                return new List<ProjectileBehavior> { behavior };
            }
            return null;
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