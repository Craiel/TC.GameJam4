namespace Assets.Scripts
{
    using System;
    
    using UnityEngine;

    public abstract class ProjectileBehavior : MonoBehaviour
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public float Damage { get; set; }

        public Vector2 Direction { get; set; }

        public float Velocity { get; set; }

        public float LifeSpan { get; set; }

        public GameObject Origin { get; set; }
        
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
                Destroy(this.gameObject);
            }
        }
    }
}
