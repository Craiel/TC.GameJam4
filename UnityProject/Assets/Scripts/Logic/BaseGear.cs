namespace Assets.Scripts.Logic
{
    using Assets.Scripts.Contracts;

    using UnityEngine;

    public abstract class BaseGear : IGear
    {
        private readonly StatDictionary internalStats;
        private readonly StatDictionary inheritedStats;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected BaseGear()
        {
            this.internalStats = new StatDictionary();
            this.inheritedStats = new StatDictionary();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public string Name { get; protected set; }

        public float GetInternalStat(StatType type)
        {
            return this.internalStats.GetStat(type);
        }

        public float GetInheritedStat(StatType type)
        {
            return this.inheritedStats.GetStat(type);
        }

        public StatDictionary GetInternalStats()
        {
            return new StatDictionary(this.internalStats);
        }

        public StatDictionary GetInheritedStats()
        {
            return new StatDictionary(this.inheritedStats);
        }

        public virtual void Update(GameObject gameObject)
        {
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected StatDictionary InternalStats
        {
            get
            {
                return this.internalStats;
            }
        }

        protected StatDictionary InheritedStats
        {
            get
            {
                return this.inheritedStats;
            }
        }
    }
}
