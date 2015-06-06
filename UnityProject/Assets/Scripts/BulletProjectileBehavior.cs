namespace Assets.Scripts
{
    using JetBrains.Annotations;

    public class BulletProjectileBehavior : ProjectileBehavior
    {
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Update()
        {
            this.transform.Translate(StaticSettings.DefaultMoveDirection * this.Velocity);
        }
    }
}
