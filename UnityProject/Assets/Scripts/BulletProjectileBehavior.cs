namespace Assets.Scripts
{
    using Assets.Scripts.Logic;

    using JetBrains.Annotations;
    using UnityEngine;

    public class BulletProjectileBehavior : ProjectileBehavior
    {
        private readonly Object exploadPrefab = Resources.Load("Projectiles/Explode");
        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        [UsedImplicitly]
        protected override void Update()
        {
            base.Update();

            this.transform.Translate(StaticSettings.DefaultMoveDirection * StaticSettings.DefaultProjectileMoveSpeed * this.Velocity * Time.deltaTime);
        }

        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == this.Origin)
            {
                return;
            }

            var data = new CombatResolve(this.DamageInfo)
            {
                Source = this.Origin,
                Target = other.gameObject
            };
            Combat.Resolve(data);
            //GameObject instance = (GameObject)Object.Instantiate(this.projectilePrefab, context.Origin.transform.position, context.Origin.transform.rotation);
            GameObject instance = (GameObject)Object.Instantiate(this.exploadPrefab, this.transform.position, this.transform.rotation);
            this.ExpireProjectile();
        }
    }
}