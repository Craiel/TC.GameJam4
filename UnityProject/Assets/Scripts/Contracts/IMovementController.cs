namespace Assets.Scripts.Contracts
{
    using InControl;

    public interface IMovementController
    {
        float Velocity { get; set; }

        float RotationSpeed { get; set; }

        bool InvertRotationAxis { get; set; }

        bool InvertAccellerationAxis { get; set; }

        InputDevice InputDevice { get; set; }

        bool Update();
    }
}
