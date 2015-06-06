namespace Assets.Scripts.Logic
{
    using System;
    using System.Collections.Generic;

    public class StatDictionary : Dictionary<StatType, float>
    {
        private static readonly IList<StatType> StatTypes;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        static StatDictionary()
        {
            StatTypes = new List<StatType>();
            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                StatTypes.Add(type);
            }
        }

        public StatDictionary(StatDictionary source = null)
        {
            if (source != null)
            {
                this.Merge(source);
            }
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public void Merge(StatDictionary other)
        {
            if (other == null)
            {
                return;
            }

            foreach (StatType type in StatTypes)
            {
                if (!other.ContainsKey(type))
                {
                    continue;
                }

                this.MergeStat(type, other[type]);
            }
        }

        public void SetStat(StatType type, float value)
        {
            if (this.ContainsKey(type))
            {
                this[type] = value;
            }
            else
            {
                this.Add(type, value);
            }
        }

        public void RemoveStat(StatType type)
        {
            if (this.ContainsKey(type))
            {
                this.Remove(type);
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void MergeStat(StatType type, float value)
        {
            if (this.ContainsKey(type))
            {
                // Note: for now we just add stats together
                this[type] += value;
                return;
            }

            this.Add(type, value);
        }

        public float GetStat(StatType type)
        {
            if (this.ContainsKey(type))
            {
                return this[type];
            }

            return 0;
        }
    }
}
