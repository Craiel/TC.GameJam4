namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Logic.Enums;

    public class StatHolder
    {
        private readonly StatDictionary baseStats;

        private readonly StatDictionary fullStats;

        // For buffs and temp modifications
        private readonly StatDictionary temporaryStats;

        private readonly StatDictionary currentStats;
        
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public StatHolder()
        {
            this.baseStats = new StatDictionary();
            this.fullStats = new StatDictionary();
            this.temporaryStats = new StatDictionary();
            this.currentStats = new StatDictionary();

            this.NeedStatUpdate = true;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public void SetBaseStats(StatDictionary newStats)
        {
            this.baseStats.Clear();
            this.baseStats.Merge(newStats);
            this.NeedStatUpdate = true;
        }

        public float GetCurrentStat(StatType type)
        {
            if (this.NeedStatUpdate)
            {
                this.UpdateStats();
            }

            return this.currentStats.GetStat(type);
        }

        public float GetMaxStat(StatType type)
        {
            if (this.NeedStatUpdate)
            {
                this.UpdateStats();
            }

            return this.fullStats.GetStat(type);
        }

        public void SetTemporaryStat(StatType type, float value)
        {
            this.temporaryStats.SetStat(type, value);
            this.NeedStatUpdate = true;
        }

        public void RemoveTemporaryStat(StatType type)
        {
            this.temporaryStats.RemoveStat(type);
            this.NeedStatUpdate = true;
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected bool NeedStatUpdate { get; set; }

        protected virtual IList<StatDictionary> GetAdditionalMergeDictionaries()
        {
            return null;
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void UpdateStats()
        {
            this.NeedStatUpdate = false;

            this.fullStats.Clear();
            this.fullStats.Merge(this.baseStats);

            // Can be override to add additional stat dictionaries
            IList<StatDictionary> additionalDictionaries = this.GetAdditionalMergeDictionaries();
            if (additionalDictionaries != null)
            {
                foreach (StatDictionary dictionary in additionalDictionaries)
                {
                    this.fullStats.Merge(dictionary);
                }
            }

            // Temporary stats are applied last
            this.fullStats.Merge(this.temporaryStats);

            // Save all the persistent values before we update the current stats
            StatDictionary persistentValues = new StatDictionary();
            foreach (StatType type in StaticSettings.PersistentStats)
            {
                persistentValues.SetStat(type, this.currentStats.GetStat(type));
            }

            this.currentStats.Clear();
            this.currentStats.Merge(this.fullStats);

            // Re-set the persistent stats
            foreach (StatType type in persistentValues.Keys)
            {
                this.currentStats.SetStat(type, persistentValues[type]);
            }
        }
    }
}
