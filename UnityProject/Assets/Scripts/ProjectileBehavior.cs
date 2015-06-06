namespace Assets.Scripts
{
    using System;

    using JetBrains.Annotations;

    using UnityEngine;

    public class ProjectileBehavior : MonoBehaviour
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public float Damage { get; set; }

        public Vector2 Direction { get; set; }

        public float Velocity { get; set; }

        public float LifeSpan { get; set; }
        
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

        [UsedImplicitly]
        private void Update()
        {
            this.transform.Translate(StaticSettings.DefaultMoveDirection * this.Velocity);
        }
    }
}
