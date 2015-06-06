namespace Assets.Scripts.Contracts
{
    public interface IMovementController
    {
        float Velocity { get; set; }

        float RotationSpeed { get; set; }

        bool InvertRotationAxis { get; set; }

        bool InvertAccellerationAxis { get; set; }

        void Update();
    }
}
