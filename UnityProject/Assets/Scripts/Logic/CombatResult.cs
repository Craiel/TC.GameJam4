namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public class CombatResult
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public CombatResult()
        {
            this.DamageDealtByType = new Dictionary<DamageType, float>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public Vector3 Location { get; set; }

        public CombatInfo Info { get; set; }

        public bool WasHit { get; set; }

        public int HitCount { get; set; }

        public int? SourcePlayerId { get; set; }
        public int? TargetPlayerId { get; set; }

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
