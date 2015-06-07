using UnityEngine;
namespace Assets.Scripts
{
    using JetBrains.Annotations;
    class StaticProjectileBehavior : ProjectileBehavior
    {
        Vector3 depthOffset = new Vector3(0, 0, -1f);
        bool walk = true;
        [UsedImplicitly]
        private void Update()
        {
            if(walk)
            {
                LineRenderer line = Origin.GetComponent<LineRenderer>();
                line.SetPosition(0, Origin.transform.position + depthOffset);
                Origin.transform.position = Vector3.MoveTowards(Origin.transform.position, this.transform.position, .5f);
            }
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject == this.Origin)
            {
                return;
            }
            walk = false;
        }
    }

    
}
