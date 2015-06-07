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
            foreach (GearType type in this.gear.Keys)
            {
                if (this.gear[type] == null)
                {
                    continue;
                }

                this.gear[type].Update(gameObject);
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
