namespace Assets.Scripts.Logic
{
    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public abstract class BaseGear : StatHolder, IGear
    {
        private readonly StatDictionary inheritedStats;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected BaseGear()
        {
            this.inheritedStats = new StatDictionary();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public GearType Type { get; protected set; }

        public string Name { get; protected set; }

        public bool IsOverheated { get; protected set; }

        public float GetInheritedStat(StatType type)
        {
            return this.inheritedStats.GetStat(type);
        }
        
        public StatDictionary GetInheritedStats()
        {
            return new StatDictionary(this.inheritedStats);
        }

        public virtual void Update(GameObject gameObject)
        {
            float heatMax = this.GetCurrentStat(StatType.Heat);
            if (heatMax > 0)
            {
                float heat = this.GetCurrentStat(StatType.HeatGeneration);
                float heatGeneration = this.GetCurrentStat(StatType.HeatGeneration);

                this.IsOverheated = heat + heatGeneration > heatMax;
            }
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected void SetInheritedStats(StatDictionary stats)
        {
            this.inheritedStats.Clear();
            this.inheritedStats.Merge(stats);
        }
    }
}
