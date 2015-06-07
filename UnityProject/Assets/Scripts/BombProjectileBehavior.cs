namespace Assets.Scripts
{
    using Assets.Scripts.Logic.Enums;

    using JetBrains.Annotations;

    using UnityEngine;

    public class BombProjectileBehavior : ProjectileBehavior
    {
        private bool isTriggered;
        private float? destroyDelay;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public GameObject EffectParent { get; set; }
        public Object EffectPrefab { get; set; }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void ExpireProjectile()
        {
            if (!this.isTriggered)
            {
                GameObject instance = (GameObject)Instantiate(this.EffectPrefab, this.transform.position, this.transform.rotation);
                instance.transform.SetParent(this.EffectParent.transform);

                StaticProjectileBehavior behavior = instance.AddComponent<StaticProjectileBehavior>();
                behavior.DamageInfo = this.DamageInfo;
                behavior.Type = ProjectileType.beam;
                behavior.LifeSpan = Time.time + 0.1f;
                behavior.Origin = this.Origin;

                // - Start the animation
                this.gameObject.GetComponentInChildren<Animator>().SetTrigger("expload");
                this.destroyDelay = Time.time + 2.0f; // 2s for the animation
                this.isTriggered = true;
                return;
            }

            this.DestroyProjectile();
        }

        [UsedImplicitly]
        protected override void Update()
        {
            base.Update();

            // Check for the delayed destroy
            if (this.destroyDelay != null && Time.time > this.destroyDelay)
            {
                this.destroyDelay = null;
                this.DestroyProjectile();
            }
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

            this.ExpireProjectile();
        }
    }
}
