namespace Assets.Scripts
{
    using JetBrains.Annotations;

    using UnityEngine;

    public class GrappleProjectileBehavior : ProjectileBehavior
    {
        private bool isTriggered;
        private float? destroyDelay;

        private Vector3 depthOffset = new Vector3(0, 0, -1f);
        private bool walk = true;
        private float distance;

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void ExpireProjectile()
        {
            if (!this.isTriggered)
            {
                this.destroyDelay = Time.time + .50f; // 1s for the walk
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

            if (this.walk)
            {
                LineRenderer line = this.Origin.GetComponent<LineRenderer>();
                line.SetPosition(0, this.Origin.transform.position + this.depthOffset);
                this.Origin.transform.position = Vector3.MoveTowards(this.Origin.transform.position, this.transform.position, .08f);
                distance = Vector3.Distance(this.transform.position,this.Origin.transform.position);
                if (distance <= .6f)
                {
                    this.DestroyProjectile();
                    line.SetPosition(0, this.Origin.transform.position);
                    line.SetPosition(1, this.Origin.transform.position);
                    walk = false;
                }
            }
        }
    }
}
