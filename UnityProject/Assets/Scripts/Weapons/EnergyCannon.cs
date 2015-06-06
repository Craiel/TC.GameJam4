namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    using UnityEngine;

    public class EnergyCannon : BaseWeapon
    {
        private readonly Object projectilePrefab;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public EnergyCannon()
        {
            this.SetStat(StatType.Velocity, 0.1f);
            this.SetStat(StatType.ProjectileLifeSpan, 1f);
            this.SetStat(StatType.Interval, 0.1f);
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override IList<ProjectileBehavior> DoFire(GameObject origin, ICharacter source)
        {
            RaycastHit2D ray = Physics2D.Raycast(origin.transform.position, -Vector2.up);
            if (ray.collider != null)
            {
                Debug.Log("I hit something");
            }
            //GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, origin.transform.position, origin.transform.rotation);

            //ProjectileBehavior behavior = instance.AddComponent<ProjectileBehavior>();
            //behavior.Damage = this.GetStat(StatType.Damage);
            //behavior.Velocity = this.GetStat(StatType.Velocity);
            //behavior.LifeSpan = Time.time + this.GetStat(StatType.ProjectileLifeSpan);

            return null;
        }
    }
}