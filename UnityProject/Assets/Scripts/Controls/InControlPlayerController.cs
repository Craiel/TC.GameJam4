namespace Assets.Scripts.Controls
{
    using System;

    using Assets.Scripts.Contracts;

    using InControl;

    using UnityEngine;

    public class InControlPlayerController : IMovementController
    {
        private const float DefaultSpeedMultiplier = 0.02f;

        private const float DefaultRotateDelay = 0.5f;

        private const float RotateStep = 45.0f;

        private readonly GameObject target;

        private int activeVector;

        private float lastRotateTime;

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

            float currentTime = Time.time;
            
            bool changed = this.HandleMove();
            changed = changed || this.HandleRotation(currentTime);

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
            } 
            else if (Math.Abs(up) < float.Epsilon)
            {
                direction = this.InvertAccellerationAxis ? up : -up;
            }

            this.target.transform.Translate(StaticSettings.DefaultMoveDirection * direction);
            return true;
        }

        private bool HandleRotation(float currentTime)
        {
            float left = this.InputDevice.DPad.Left.Value;
            float right = this.InputDevice.DPad.Right.Value;

            if (Math.Abs(left) < float.Epsilon
                && Math.Abs(right) < float.Epsilon)
            {
                return false;
            }

            // Recalculate the rotation delay
            var rotationDelay = DefaultRotateDelay * this.RotationSpeed;
            rotationDelay = Mathf.Clamp(rotationDelay, StaticSettings.MinRotationDelay, StaticSettings.MaxRotationDelay);

            // Check if enough time has passed to rotate
            if (currentTime < this.lastRotateTime + rotationDelay)
            {
                return false;
            }

            // Check which direction we are rotating
            if (left > 0)
            {
                this.activeVector += this.InvertRotationAxis ? 1 : -1;
            }
            else if (right > 0)
            {
                this.activeVector += this.InvertRotationAxis ? -1 : 1;
            }

            // Make the vector loop over
            if (this.activeVector > 8)
            {
                this.activeVector = 0;
            } else if (this.activeVector < 0)
            {
                this.activeVector = 8;
            }

            // Reset the rotation and re-apply
            this.target.transform.rotation = Quaternion.identity;
            this.target.transform.Rotate(Vector3.forward, RotateStep * this.activeVector);
            this.lastRotateTime = currentTime;
            return true;
        }
    }
}
