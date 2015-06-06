namespace Assets.Scripts.Logic
{
    using UnityEngine;

    public abstract class BaseProjectile : IProjectile
    {
        public float Damage { get; protected set; }

        public float LifeTimeRemaining { get; protected set; }

        public Vector2 Direction { get; protected set; }

        public float Velocity { get; protected set; }
    }
}
