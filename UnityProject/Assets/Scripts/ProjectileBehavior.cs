﻿namespace Assets.Scripts
{
    using Assets.Scripts.Logic;
    using System;
    
    using JetBrains.Annotations;

    using UnityEngine;
    using Assets.Scripts.Logic.Enums;
    using System.Collections;

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
        public ProjectileType Type { get; set; }

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
                if(this.Type == ProjectileType.bomb)
                {
                    this.gameObject.GetComponentInChildren<Animator>().SetTrigger("expload");
                    StartCoroutine(DelayDispose());
                }
                else
                    Destroy(this.gameObject);
            }
        }
        
        IEnumerator DelayDispose()
        {
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
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
                Source = this.Origin,
                Target = other.gameObject
            };

            Combat.Resolve(data);

            switch (this.Type)
            {
                case ProjectileType.bullet:
                case ProjectileType.bomb:
                    {
                        this.Dispose();
                        break;
                    }
            }
        }

        [UsedImplicitly]
        private void OnTriggerStay2D(Collider2D other)
        {
            //TODO: beam dmg cal on each mech and walls it hits
        }
    }
}
