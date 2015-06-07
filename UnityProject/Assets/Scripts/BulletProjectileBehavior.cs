namespace Assets.Scripts
{
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
    }
}