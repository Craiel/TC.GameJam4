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

        private readonly IDictionary<GearType, bool> gearEnableState;

        private float nextCoolingTick;


 
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public Character()
        {
            this.Id = nextId++;

            this.gear = new Dictionary<GearType, IGear>();
            this.gearEnableState = new Dictionary<GearType, bool>();

            foreach (GearType type in EnumLists.GearTypes)
            {
                this.gearEnableState.Add(type, false);
            }

            // Set some defaults but will be override later
            this.SetBaseStats(StaticSettings.PlayerBaseStats);

            this.ResetCurrentStats();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public int Id { get; private set; }

        public string Name { get; set; }

        public Color Color { get; set; }

        public InputDevice InputDevice { get; set; }

        public bool IsDead { get; private set; }

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

            float health = this.GetCurrentStat(StatType.Health);
            this.IsDead = health <= 0;
        }

        private void UpdateGear(GameObject gameObject, GearType type)
        {
            if (!this.gear.ContainsKey(type) || this.gear[type] == null)
            {
                // There is no gear here, check if we had it enabled previously
                this.CheckGearDisableState(type, false);
                return;
            }

            IGear current = this.gear[type];

            // Check if the gear was destroyed
            float healthMax = current.GetMaxStat(StatType.Health);
            float health = current.GetCurrentStat(StatType.Health);
            if (healthMax > 0 && health <= 0)
            {
                this.RemoveGear(type);
                this.CheckGearDisableState(type, false);
                Debug.Log(string.Format("Item {0} is destroyed, removing!", type));
                return;
            }
            
            current.Update(gameObject);

            // Update the enabled state to be non-overheated
            this.CheckGearDisableState(type, !current.IsOverheated);
        }

        private void CheckGearDisableState(GearType type, bool isEnabled)
        {
            if (this.gearEnableState[type] != isEnabled)
            {
                this.gearEnableState[type] = isEnabled;
                this.NeedStatUpdate = true;
            }
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
                if (this.gear[type] == null || !this.gearEnableState[type])
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
