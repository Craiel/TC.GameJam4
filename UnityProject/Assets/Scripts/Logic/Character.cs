namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic.Enums;

    using InControl;

    using UnityEngine;

    public class Character : StatHolder, ICharacter
    {
        private static int nextId;

        private readonly IDictionary<GearType, IGear> gear;

        private float nextCoolingTick;


 
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public Character()
        {
            this.Id = nextId++;

            this.gear = new Dictionary<GearType, IGear>();

            // Set some defaults but will be override later
            this.SetBaseStats(StaticSettings.PlayerBaseStats);

            this.ResetCurrentStats();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public int Id { get; private set; }

        public string Name { get; set; }

        public Color myColor { get; set; }

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

            this.NeedStatUpdate = true;
        }

        public void RemoveGear(GearType type)
        {
            if (this.gear.ContainsKey(type))
            {
                this.gear.Remove(type);
                this.NeedStatUpdate = true;
            }
        }

        public void TakeDamage(float damage)
        {   
            //TODO: Apply damage to mech (shields, armor, hull, parts damage, etc.)
        }

        public void Update(GameObject gameObject)
        {
            foreach (GearType type in EnumLists.GearTypes)
            {
                this.UpdateGear(gameObject, type);
            }

            if (Time.time >= this.nextCoolingTick)
            {
                foreach (GearType type in EnumLists.GearTypes)
                {
                    this.UpdateGearCooling(type);
                }

                this.nextCoolingTick = Time.time + StaticSettings.CoolingTickDelay;
            }
        }

        private void UpdateGear(GameObject gameObject, GearType type)
        {
            if (!this.gear.ContainsKey(type) || this.gear[type] == null)
            {
                return;
            }

            IGear current = this.gear[type];

            // Check if the gear was destroyed
            float healthMax = current.GetMaxStat(StatType.Health);
            float health = current.GetCurrentStat(StatType.Health);
            if (healthMax > 0 && health <= 0)
            {
                this.RemoveGear(type);
                Debug.Log(string.Format("Item {0} is destroyed, removing!", type));
                return;
            }
            
            current.Update(gameObject);
        }

        private void UpdateGearCooling(GearType type)
        {
            if (!this.gear.ContainsKey(type) || this.gear[type] == null)
            {
                return;
            }

            IGear current = this.gear[type];

            // Deduct heat based on the cooling
            float heatMax = current.GetMaxStat(StatType.Heat);
            float heat = current.GetCurrentStat(StatType.Heat);
            float cooling = this.GetCurrentStat(StatType.HeatCoolingRate);
            if (heatMax > 0 && heat > 0)
            {
                float value = cooling;
                if (cooling > heat)
                {
                    value = heat;
                }

                current.ModifyStat(StatType.Heat, -value);
            }
        }

        protected override IList<StatDictionary> GetAdditionalMergeDictionaries()
        {
            IList<StatDictionary> results = new List<StatDictionary>();

            foreach (GearType type in this.gear.Keys)
            {
                if (this.gear[type] == null)
                {
                    continue;
                }

                StatDictionary inherited = this.gear[type].GetInheritedStats();
                if (inherited != null)
                {
                    results.Add(inherited);
                }
            }

            return results;
        }
    }
}
