namespace Assets.Scripts
{
    using JetBrains.Annotations;

    using UnityEngine;

    public class BombProjectileBehavior : ProjectileBehavior
    {
        private bool isTriggered;
        private float? destroyDelay;

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void ExpireProjectile()
        {
            if (!this.isTriggered)
            {
                // - Start the animation
                this.gameObject.GetComponentInChildren<Animator>().SetTrigger("expload");
                this.destroyDelay = Time.time + 2.0f; // 2s for the animation
                this.isTriggered = true;
                return;
            }

            this.DestroyProjectile();
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Update()
        {
            // Check for the delayed destroy
            if (this.destroyDelay != null && Time.time > this.destroyDelay)
            {
                this.destroyDelay = null;
                this.DestroyProjectile();
            }
        }
    }
}
