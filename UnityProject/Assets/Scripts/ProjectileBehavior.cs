namespace Assets.Scripts
{
    using Assets.Scripts.Logic;
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
            if (other.gameObject == Origin)
            {
                return;
            }

            StatUtils.ApplyDamage(other.gameObject, Damage);
            Dispose();
        }
    }
}
