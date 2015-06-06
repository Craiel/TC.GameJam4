namespace Assets.Scripts.Contracts
{
    using UnityEngine;

    public interface IProjectile
    {
        float Damage { get; }

        float LifeTimeRemaining { get; }

        Vector2 Direction { get; }

        float Velocity { get; }
    }
}
