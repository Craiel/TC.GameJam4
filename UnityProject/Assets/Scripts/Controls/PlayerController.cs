namespace Assets.Scripts.Controls
{
    using System;

    using JetBrains.Annotations;

    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        private const float DefaultSpeedMultiplier = 0.02f;

        private const float DefaultRotationMultiplier = 2f;

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
            float rotate = (Input.GetAxis("Rotate") * DefaultRotationMultiplier) * this.rotationSpeed;

            if (this.invertRotationAxis)
            {
                rotate *= -1;
            }

            if (Math.Abs(rotate) > float.Epsilon)
            {
                this.transform.Rotate(Vector3.forward, rotate);
            }

            if (Math.Abs(move - float.Epsilon) > float.Epsilon) {
                this.transform.Translate(this.moveDirection * move);
            }
        }
    }
}
