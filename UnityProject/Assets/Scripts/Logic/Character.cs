namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;

    using InControl;

    using UnityEngine;

    public class Character : ICharacter
    {
        private readonly StatDictionary baseStats;

        private readonly StatDictionary fullStats;

        // For buffs and temp modifications
        private readonly StatDictionary temporaryStats;

        private readonly IDictionary<GearType, IGear> gear;
 
        private bool needStatUpdate = true;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public Character()
        {
            this.baseStats = new StatDictionary();
            this.fullStats = new StatDictionary();
            this.temporaryStats = new StatDictionary();
            this.gear = new Dictionary<GearType, IGear>();

            this.baseStats.Merge(StaticSettings.PlayerBaseStats);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public string Name { get; set; }

        public InputDevice InputDevice { get; set; }

        public ICharacter Target { get; set; }

        public IGear GetGear(GearType type)
        {
            if (this.gear.ContainsKey(type))
            {
                return this.gear[type];
            }

            return null;
        }

        public void SetBaseStats(StatDictionary baseStats)
        {
            this.baseStats.Clear();
            this.baseStats.Merge(baseStats);
            needStatUpdate = true;
        }

        public void SetGear(GearType type, IGear newGear)
        {
            System.Diagnostics.Trace.Assert(newGear != null, "New Gear was null, call RemoveGear instead!");

            if (this.gear.ContainsKey(type))
            {
                this.gear[type] = newGear;
            }
            else
            {
                this.gear.Add(type, newGear);
            }

            this.needStatUpdate = true;
        }

        public void RemoveGear(GearType type)
        {
            if (this.gear.ContainsKey(type))
            {
                this.gear.Remove(type);
                this.needStatUpdate = true;
            }
        }

        public float GetStat(StatType type)
        {
            if (this.needStatUpdate)
            {
                this.UpdateStats();
            }

            return this.fullStats.GetStat(type);
        }

        public void SetTemporaryStat(StatType type, float value)
        {
            this.temporaryStats.SetStat(type, value);
            this.needStatUpdate = true;
        }

        public void RemoveTemporaryStat(StatType type)
        {
            this.temporaryStats.RemoveStat(type);
            this.needStatUpdate = true;
        }

        public void TakeDamage(float damage)
        {   
            //TODO: Apply damage to mech (shields, armor, hull, parts damage, etc.)
        }

        public void Update(GameObject gameObject)
        {
            foreach (GearType type in this.gear.Keys)
            {
                if (this.gear[type] == null)
                {
                    continue;
                }

                this.gear[type].Update(gameObject);
            }
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void UpdateStats()
        {
            this.needStatUpdate = false;

            this.fullStats.Clear();
            this.fullStats.Merge(this.baseStats);

            // Items come in second after the base stats
            foreach (GearType type in this.gear.Keys)
            {
                if (this.gear[type] == null)
                {
                    continue;
                }

                this.fullStats.Merge(this.gear[type].GetInheritedStats());
            }

            // Temporary stats are applied last
            this.fullStats.Merge(this.temporaryStats);
        }
    }
}
