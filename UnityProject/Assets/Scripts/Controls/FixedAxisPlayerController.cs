namespace Assets.Scripts.Controls
{
    using System;

    using Assets.Scripts.Contracts;

    using InControl;

    using UnityEngine;

    public class FixedAxisPlayerController : IMovementController
    {
        private const float DefaultSpeedMultiplier = 0.02f;

        private const float DefaultRotateDelay = 0.5f;

        private const float RotateStep = 45.0f;

        private readonly GameObject target;

        private int activeVector;

        private float lastRotateTime;

        private float _zAxis = 0f;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public FixedAxisPlayerController(GameObject target)
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

            bool changed = false; 
            float move = (Input.GetAxis("Move")) * this.Velocity * DefaultSpeedMultiplier;
            float rotate = Input.GetAxis("Rotate");
            float currentTime = Time.time;

            if (!this.InvertRotationAxis)
            {
                rotate *= -1;
            }

            if (this.InvertAccellerationAxis)
            {
                move *= -1;
            }

            if (Math.Abs(rotate) > float.Epsilon)
            {
                this.HandleRotation(currentTime, rotate);
                changed = true;
            }
            
            if (Math.Abs(move - float.Epsilon) > float.Epsilon) {
                this.target.transform.Translate(StaticSettings.DefaultMoveDirection * move);
                changed = true;
            }
            return changed;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void HandleRotation(float currentTime, float rotationValue)
        {
            // Recalculate the rotation delay
            var rotationDelay = DefaultRotateDelay * this.RotationSpeed;
            rotationDelay = Mathf.Clamp(rotationDelay, StaticSettings.MinRotationDelay, StaticSettings.MaxRotationDelay);

            // Check if enough time has passed to rotate
            if (currentTime < this.lastRotateTime + rotationDelay)
            {
                return;
            }

            // Check which direction we are rotating
            if (rotationValue > 0)
            {
                this.activeVector++;
            }
            else if (rotationValue < 0)
            {
                this.activeVector--;
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
        }
    }
}
