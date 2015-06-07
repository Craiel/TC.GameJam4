namespace Assets.Scripts
{
    using JetBrains.Annotations;
    class MeleeProjectileBehavior : ProjectileBehavior
    {
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void Update()
        {
            this.transform.position = Origin.transform.position;
        }
    }
}
