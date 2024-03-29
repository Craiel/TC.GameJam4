﻿namespace Assets.Scripts.Controls
{
    using System;

    using Assets.Scripts.Contracts;

    using InControl;

    using UnityEngine;

    /*public class FreeFormPlayerController : IMovementController
    {
        private const float DefaultSpeedMultiplier = 0.02f;

        private const float DefaultRotationMultiplier = 2f;
        
        private readonly GameObject target;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public FreeFormPlayerController(GameObject target)
        {
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
            if (StaticSettings.EnableInControl && this.InputDevice == null)
            {
                return false;
            }

            bool changed = false;
            float move = (Input.GetAxis("Move")) * this.Velocity * DefaultSpeedMultiplier;
            float rotate = (Input.GetAxis("Rotate") * DefaultRotationMultiplier) * this.RotationSpeed;

            if (!this.InvertRotationAxis)
            {
                rotate *= -1;
            }

            if (Math.Abs(rotate) > float.Epsilon)
            {
                this.target.transform.Rotate(Vector3.forward, rotate);
                changed = true;
            }

            if (Math.Abs(move - float.Epsilon) > float.Epsilon) {
                this.target.transform.Translate(StaticSettings.DefaultMoveDirection * move);
                changed = true;
            }

            return changed;
        }
    }*/
}
