namespace Assets.Scripts
{
    using Assets.Scripts.Logic;

    using JetBrains.Annotations;
    using UnityEngine;

    public class BulletProjectileBehavior : ProjectileBehavior
    {
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
            this.ExpireProjectile();
        }
    }
}