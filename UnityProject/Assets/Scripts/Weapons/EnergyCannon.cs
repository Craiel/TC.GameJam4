namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    using UnityEngine;

    public class EnergyCannon : BaseWeapon
    {
        private readonly Object projectilePrefab;
        private float timeChanged;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public EnergyCannon()
        {
            this.SetStat(StatType.Velocity, 0.1f);
            this.SetStat(StatType.ProjectileLifeSpan, 1f);
            this.SetStat(StatType.Interval, 0.1f);
        }

        // -------------------------------------------------------------------);
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
                LineRenderer line = origin.GetComponent<LineRenderer>();
                line.SetPosition(0, origin.transform.position);
                line.SetPosition(1, ray.transform.position);
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