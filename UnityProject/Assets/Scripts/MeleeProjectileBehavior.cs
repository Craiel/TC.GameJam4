namespace Assets.Scripts
{
    using JetBrains.Annotations;
    class MeleeProjectileBehavior : ProjectileBehavior
    {
        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        [UsedImplicitly]
        protected override void Update()
        {
            base.Update();

            this.transform.position = this.Origin.transform.position;
        }
    }
}
