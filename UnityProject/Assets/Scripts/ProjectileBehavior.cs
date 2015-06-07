﻿namespace Assets.Scripts
{
    using Assets.Scripts.Logic;
    using System;
    
    using JetBrains.Annotations;

    using UnityEngine;

    public abstract class ProjectileBehavior : MonoBehaviour
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected ProjectileBehavior()
        {
            this.IsAlive = true;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public CombatInfo DamageInfo { get; set; }

        public Vector2 Direction { get; set; }

        public float Velocity { get; set; }

        public float LifeSpan { get; set; }

        public GameObject Origin { get; set; }

        public bool IsAlive { get; set; }

        public bool IsBouncing { get; set; }
        
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                this.IsAlive = false;
                Destroy(this.gameObject);
            }
        }

        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject == this.Origin)
            {
                return;
            }

            var data = new CombatResolve(this.DamageInfo)
                              {
                                  Source = this.gameObject,
                                  Target = other.gameObject
                              };
            
            Combat.Resolve(data);
            this.Dispose();
        }
    }
}
