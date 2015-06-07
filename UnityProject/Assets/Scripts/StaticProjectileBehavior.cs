namespace Assets.Scripts
{
    using System.Collections.Generic;

    using Assets.Scripts.Logic;

    using JetBrains.Annotations;

    using UnityEngine;
    
    class StaticProjectileBehavior : ProjectileBehavior
    {
        private readonly IDictionary<Collider2D, float> lastDamageApply;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public StaticProjectileBehavior()
        {
            this.lastDamageApply = new Dictionary<Collider2D, float>();

            // Make the default large enough to not trigger
            this.ApplyDelay = float.MaxValue;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public float ApplyDelay { get; set; }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D other)
        {
            this.TryApplyDamage(other);
        }

        [UsedImplicitly]
        private void OnTriggerStay2D(Collider2D other)
        {
            this.TryApplyDamage(other);
        }

        private void TryApplyDamage(Collider2D other)
        {
            // Don't apply to ourself
            if (other.gameObject == this.Origin)
            {
                return;
            }

            float currentTime = Time.time;

            // If we tracked this instance already and are not yet ready to apply skip
            if (this.lastDamageApply.ContainsKey(other) && currentTime < this.lastDamageApply[other])
            {
                return;
            }
            
            // Resolve the combat
            var data = new CombatResolve(this.DamageInfo)
            {
                Source = this.Origin,
                Target = other.gameObject
            };

            Combat.Resolve(data);

            // Mark this as handled
            if (!this.lastDamageApply.ContainsKey(other))
            {
                this.lastDamageApply.Add(other, 0f);
            }

            this.lastDamageApply[other] = currentTime + this.ApplyDelay;
        }
    }
}
