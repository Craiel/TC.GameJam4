namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using UnityEngine;

    public class DamageResolve
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public DamageResolve()
        {
            this.DamageDealtByType = new Dictionary<DamageType, float>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public GameObject Source { get; set; }

        public GameObject Target { get; set; }

        public DamageInfo DamageInfo { get; set; }

        public bool WasHit { get; set; }

        public int HitCount { get; set; }

        public float DamageDealtTotal { get; set; }

        public IDictionary<DamageType, float> DamageDealtByType { get; private set; }

        public void RegisterHit(DamageType type, float damage)
        {
            if (!this.DamageDealtByType.ContainsKey(type))
            {
                this.DamageDealtByType.Add(type, 0);
            }

            this.DamageDealtByType[type] += damage;
            this.DamageDealtTotal += damage;
            this.WasHit = true;
            this.HitCount++;
        }
    }
}
