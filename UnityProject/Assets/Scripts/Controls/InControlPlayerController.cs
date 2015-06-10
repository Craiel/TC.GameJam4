namespace Assets.Scripts.Controls
{
    using System;
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public class InControlPlayerController : IMovementController
    {
        private const float DefaultSpeedMultiplier = 0.02f;

        private const float DefaultRotationMultiplier = 2f;
        
        private readonly GameObject target;
        
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public InControlPlayerController(GameObject target)
        {
            System.Diagnostics.Trace.Assert(target != null);

            this.target = target;
        }
        
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public float Velocity { get; set; }

        public float RotationSpeed { get; set; }

        public bool InvertRotationAxis { get; set; }

        public bool InvertAccellerationAxis { get; set; }

        public IInputDeviceMapping InputDevice { get; set; }

        public bool Update()
        {
            if (this.InputDevice == null)
            {
                return false;
            }
            
            bool changed = this.HandleMove();
            changed = this.HandleRotation() || changed;

            return changed;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private bool HandleMove()
        {
            float backwards = this.InputDevice.GetState(PlayerControl.MoveBackwards).Value;
            float forward = this.InputDevice.GetState(PlayerControl.MoveForward).Value;

            if (Math.Abs(backwards) < float.Epsilon
                && Math.Abs(forward) < float.Epsilon)
            {
                return false;
            }

            float direction = 0f;
            if (Math.Abs(backwards) > float.Epsilon)
            {
                direction = this.InvertAccellerationAxis ? backwards : -backwards;
                direction *= 0.5f; //Half-speed for backing up
            }
            else if (Math.Abs(forward) > float.Epsilon)
            {
                direction = -(this.InvertAccellerationAxis ? forward : -forward);
            }

            direction *= DefaultSpeedMultiplier * target.GetComponent<PlayerCharacterBehavior>().Character.GetCurrentStat(StatType.Velocity);

            this.target.transform.Translate(StaticSettings.DefaultMoveDirection * direction);
            return true;
        }

        private bool HandleRotation()
        {
            float left = this.InputDevice.GetState(PlayerControl.MoveRotateLeft).Value;
            float right = this.InputDevice.GetState(PlayerControl.MoveRotateRight).Value;

            if (Math.Abs(left) < float.Epsilon
                && Math.Abs(right) < float.Epsilon)
            {
                return false;
            }

            float rotate = 0f;
            if (Math.Abs(left) > float.Epsilon)
            {
                rotate = this.InvertAccellerationAxis ? -left : left;
            }
            else if (Math.Abs(right) > float.Epsilon)
            {
                rotate = -(this.InvertAccellerationAxis ? -right : right);
            }

            rotate *= DefaultRotationMultiplier * target.GetComponent<PlayerCharacterBehavior>().Character.GetCurrentStat(StatType.RotationSpeed);

            this.target.transform.Rotate(Vector3.forward, rotate);
            return true;
        }
    }
}
