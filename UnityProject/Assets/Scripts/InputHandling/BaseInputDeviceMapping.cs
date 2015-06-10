namespace Assets.Scripts.InputHandling
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

    using InControl;

    public abstract class BaseInputDeviceMapping : IInputDeviceMapping
    {
        private readonly IDictionary<PlayerControl, InputDeviceState> keyState;
        
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected BaseInputDeviceMapping()
        {
            this.keyState = new Dictionary<PlayerControl, InputDeviceState>();
            foreach (PlayerControl control in EnumLists.PlayerControls)
            {
                this.keyState.Add(control, new InputDeviceState());
            }
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public virtual void Update()
        {
            this.ResetState();
        }

        public InputDeviceState GetState(PlayerControl control)
        {
            return this.keyState[control];
        }

        public void ResetState()
        {
            foreach (PlayerControl control in this.keyState.Keys)
            {
                this.keyState[control].Reset();
            }
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected void UpdateState(InputControl control, PlayerControl target)
        {
            InputDeviceState state = this.keyState[target];
            state.Value = control.Value;
            state.IsPressed = control.IsPressed;
        }

        protected void UpdateState(float value, bool pressed, PlayerControl target, bool isPositive)
        {
            InputDeviceState state = this.keyState[target];
            state.Value = isPositive ? value : -value;
            state.IsPressed = pressed;
        }
    }
}
