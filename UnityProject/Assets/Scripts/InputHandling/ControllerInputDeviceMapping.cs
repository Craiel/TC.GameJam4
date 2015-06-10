namespace Assets.Scripts.InputHandling
{
    using Assets.Scripts.Logic.Enums;

    using InControl;

    public class ControllerInputDeviceMapping : BaseInputDeviceMapping
    {
        private InputDevice device;
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public InputDevice Device
        {
            get
            {
                return this.device;
            }

            set
            {
                if (this.device != value)
                {
                    this.device = value;
                    this.ResetState();
                }
            }
        }

        public override void Update()
        {
            base.Update();

            if (this.device == null)
            {
                return;
            }

            // Get the state for all the axis we care about
            this.UpdateState(this.device.Command, PlayerControl.Start);
            this.UpdateState(this.device.DPadUp, PlayerControl.MoveForward);
            this.UpdateState(this.device.DPadDown, PlayerControl.MoveBackwards);
            this.UpdateState(this.device.DPadLeft, PlayerControl.MoveRotateLeft);
            this.UpdateState(this.device.DPadRight, PlayerControl.MoveRotateRight);
            this.UpdateState(this.device.Action1, PlayerControl.Fire);
            this.UpdateState(this.device.Action1, PlayerControl.Confirm);
            this.UpdateState(this.device.Action2, PlayerControl.Fire2);
            this.UpdateState(this.device.Action2, PlayerControl.Exit);
            this.UpdateState(this.device.Action3, PlayerControl.DropGear);
            this.UpdateState(this.device.LeftTrigger, PlayerControl.CycleItemsLeft);
            this.UpdateState(this.device.RightTrigger, PlayerControl.CycleItemsRight);
        }
    }
}
