namespace Assets.Scripts
{
    using Assets.Scripts.Logic;
    
    using JetBrains.Annotations;

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

        public float LifeSpan { get; set; }

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

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == this.Origin)
            {
                return;
            }
            
            switch (this.Type)
            {
                case ProjectileType.bullet:
                    {
                        var data = new CombatResolve(this.DamageInfo)
                        {
                            Source = this.Origin,
                            Target = other.gameObject
                        };

                        Combat.Resolve(data);
                        this.ExpireProjectile();
                        break;
                    }

                case ProjectileType.bomb:
                    {
                        this.ExpireProjectile();
                        break;
                    }
            }
        }

        [UsedImplicitly]
        private void OnTriggerStay2D(Collider2D other)
        {
            //TODO: beam dmg cal on each mech and walls it hits
        }
    }
}
