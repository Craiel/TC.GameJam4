namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    public abstract class BaseGear : IGear
    {
        private readonly IDictionary<StatType, float> stats;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected BaseGear()
        {
            this.stats = new Dictionary<StatType, float>();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public string Name { get; protected set; }

        public float GetStat(StatType type)
        {
            if (this.stats.ContainsKey(type))
            {
                return this.stats[type];
            }

            return 0;
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected void SetStat(StatType type, float value)
        {
            if (!this.stats.ContainsKey(type))
            {
                this.stats.Add(type, value);
            }

            this.stats[type] = value;
        }
    }
}
