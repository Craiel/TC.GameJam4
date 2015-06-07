namespace Assets.Scripts
{
    using Assets.Scripts.Logic;
    
    using UnityEngine;
    using Assets.Scripts.Logic.Enums;

    public abstract class ProjectileBehavior : MonoBehaviour
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected ProjectileBehavior()
        {
            this.IsAlive = true;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public ProjectileType Type { get; set; }

        public CombatInfo DamageInfo { get; set; }

        public Vector2 Direction { get; set; }

        public float Velocity { get; set; }

        public float? LifeSpan { get; set; }

        public GameObject Origin { get; set; }

        public bool IsAlive { get; set; }

        public bool IsBouncing { get; set; }
        
        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected virtual void ExpireProjectile()
        {
            // Called when the lifetime of the projectile has expired
            this.DestroyProjectile();
        }

        protected virtual void DestroyProjectile()
        {
            this.IsAlive = false;
            Destroy(this.gameObject);
        }

        protected virtual void Update()
        {
            if (this.LifeSpan != null && Time.time > this.LifeSpan)
            {
                this.ExpireProjectile();
                this.LifeSpan = null;
            }
        }
    }
}
