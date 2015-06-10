namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.InputHandling;
    using Assets.Scripts.Logic.Enums;

    public interface IInputDeviceMapping
    {
        void Update();

        InputDeviceState GetState(PlayerControl control);

        void ResetState();
    }
}
