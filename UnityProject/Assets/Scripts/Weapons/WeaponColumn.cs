namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

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
                    { StatType.ProjectileLifeSpan, 1f },
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
                Vector3 depthOffset = new Vector3(0, 0, -1f);
                LineRenderer line = origin.GetComponent<LineRenderer>();
                line.SetPosition(0, origin.transform.position + depthOffset);
                line.SetPosition(1, ray.transform.position + depthOffset);
                //line.SetPosition(1, new Vector3(0f,ray.transform.position.y,0f));
                timeChanged = Time.time;
            }

            //GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, origin.transform.position, origin.transform.rotation);

            //ProjectileBehavior behavior = instance.AddComponent<ProjectileBehavior>();
            //behavior.Damage = this.GetStat(StatType.Damage);
            //behavior.Velocity = this.GetStat(StatType.Velocity);
            //behavior.LifeSpan = Time.time + this.GetStat(StatType.ProjectileLifeSpan);

            return null;
        }

        public override void Update(GameObject origin)
        {
            if (timeChanged + .1 <= Time.time)
            {
                origin.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0f, 0f, 0f));
                origin.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0f, 0f, 0f));
            }
                
        }
    }
}