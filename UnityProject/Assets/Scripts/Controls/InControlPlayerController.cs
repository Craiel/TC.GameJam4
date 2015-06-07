namespace Assets.Scripts.Controls
{
    using System;
    using Assets.Scripts.Contracts;
    using InControl;
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

        public InputDevice InputDevice { get; set; }

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
            float down = this.InputDevice.DPad.Down.Value;
            float up = this.InputDevice.DPad.Up.Value;

            if (Math.Abs(down) < float.Epsilon
                && Math.Abs(up) < float.Epsilon)
            {
                return false;
            }

            float direction = 0f;
            if (Math.Abs(down) > float.Epsilon)
            {
                direction = this.InvertAccellerationAxis ? down : -down;
                direction *= 0.5f; //Half-speed for backing up
            } 
            else if (Math.Abs(up) > float.Epsilon)
            {
                direction = -(this.InvertAccellerationAxis ? up : -up);
            }

            direction *= DefaultSpeedMultiplier * target.GetComponent<PlayerCharacterBehavior>().Character.GetCurrentStat(Logic.StatType.Velocity);

            this.target.transform.Translate(StaticSettings.DefaultMoveDirection * direction);
            return true;
        }

        private bool HandleRotation()
        {
            float left = this.InputDevice.DPad.Left.Value;
            float right = this.InputDevice.DPad.Right.Value;

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

            rotate *= DefaultRotationMultiplier * target.GetComponent<PlayerCharacterBehavior>().Character.GetCurrentStat(Logic.StatType.RotationSpeed);

            this.target.transform.Rotate(Vector3.forward, rotate);
            return true;
        }
    }
}
