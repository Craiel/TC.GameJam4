namespace Assets.Scripts
{
    using JetBrains.Annotations;
    using UnityEngine;

    public class BulletProjectileBehavior : ProjectileBehavior
    {
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Update()
        {
            this.transform.Translate(StaticSettings.DefaultMoveDirection * StaticSettings.DefaultProjectileMoveSpeed * this.Velocity * Time.deltaTime);
        }
    }
}