namespace Assets.Scripts.Controls
{
    using System;

    using JetBrains.Annotations;

    using UnityEngine;

    public class FixedAxisPlayerController : MonoBehaviour
    {
        private const float DefaultSpeedMultiplier = 0.02f;

        private const float MinRotationDelay = 0.2f;

        private const float MaxRotationDelay = 2f;

        private const float DefaultRotateDelay = 0.5f;
        private const float RotateStep = 45.0f;

        private int activeVector;

        private float lastRotateTime;

        private Vector3 moveDirection;

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        [SerializeField]
        public float velocity = 1.0f;

        [SerializeField]
        public float rotationSpeed = 1.0f;

        [SerializeField]
        public bool invertRotationAxis = true;
        
        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private CharacterController characterController;

        [UsedImplicitly]
        private void Start()
        {
            this.characterController = this.GetComponent<CharacterController>();

            System.Diagnostics.Trace.Assert(this.characterController != null);

            this.moveDirection = new Vector3(0, 1, 0);
        }

        [UsedImplicitly]
        private void Update()
        {
            float move = (Input.GetAxis("Move")) * this.velocity * DefaultSpeedMultiplier;
            float rotate = Input.GetAxis("Rotate");
            float currentTime = Time.time;

            if (this.invertRotationAxis)
            {
                rotate *= -1;
            }

            if (Math.Abs(rotate) > float.Epsilon)
            {
                this.HandleRotation(currentTime, rotate);
            }
            
            if (Math.Abs(move - float.Epsilon) > float.Epsilon) {
                this.transform.Translate(this.moveDirection * move);
            }
        }

        private void HandleRotation(float currentTime, float rotationValue)
        {
            // Recalculate the rotation delay
            var rotationDelay = DefaultRotateDelay * this.rotationSpeed;
            rotationDelay = Mathf.Clamp(rotationDelay, MinRotationDelay, MaxRotationDelay);

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
            this.transform.rotation = Quaternion.identity;
            this.transform.Rotate(Vector3.forward, RotateStep * this.activeVector);
            this.lastRotateTime = currentTime;
        }
    }
}
