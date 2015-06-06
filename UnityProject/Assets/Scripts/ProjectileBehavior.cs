namespace Assets.Scripts
{
    using System;
    
    using UnityEngine;

    public abstract class ProjectileBehavior : MonoBehaviour
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public ProjectileBehavior()
        {
            IsAlive = true;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public float Damage { get; set; }

        public Vector2 Direction { get; set; }

        public float Velocity { get; set; }

        public float LifeSpan { get; set; }

        public GameObject Origin { get; set; }

        public bool IsAlive { get; set; }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                IsAlive = false;
                Destroy(this.gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject target = other.gameObject;

            if(target == Origin)
            {
                return;
            }

            DestructibleTile destructibleTile = target.GetComponent<DestructibleTile>();
            if(destructibleTile != null)
            {
                destructibleTile.TakeDamage(Damage);
            }
            Dispose();
        }
    }
}
