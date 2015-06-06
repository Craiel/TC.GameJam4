namespace Assets.Scripts.Logic
{
    using System;
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using UnityEngine;

    public class PlayerCharacter : ICharacter
    {
        private readonly IDictionary<StatType, float> baseStats;

        private readonly IDictionary<StatType, float> fullStats;
 
        private IArmor head;

        private IArmor chest;

        private IArmor legs;

        private IWeapon leftWeapon;

        private IWeapon rightWeapon;

        private bool needStatUpdate = true;

        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public PlayerCharacter()
        {
            this.baseStats = new Dictionary<StatType, float>();
            this.fullStats = new Dictionary<StatType, float>();

            foreach (StatType type in StaticSettings.PlayerBaseStats.Keys)
            {
                this.baseStats.Add(type, StaticSettings.PlayerBaseStats[type]);
            }
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public string Name { get; set; }

        public IArmor Head
        {
            get
            {
                return this.head;
            }
            set
            {
                if (this.head != value)
                {
                    this.head = value;
                    this.needStatUpdate = true;
                }
            }
        }

        public IArmor Chest
        {
            get
            {
                return this.chest;
            }
            set
            {
                if (this.chest != value)
                {
                    this.chest = value;
                    this.needStatUpdate = true;
                }
            }
        }

        public IArmor Legs
        {
            get
            {
                return this.legs;
            }
            set
            {
                if (this.legs != value)
                {
                    this.legs = value;
                    this.needStatUpdate = true;
                }
            }
        }

        public IWeapon LeftWeapon
        {
            get
            {
                return this.leftWeapon;
            }
            set
            {
                if (this.leftWeapon != value)
                {
                    this.leftWeapon = value;
                    this.needStatUpdate = true;
                }
            }
        }

        public IWeapon RightWeapon
        {
            get
            {
                return this.rightWeapon;
            }
            set
            {
                if (this.rightWeapon != value)
                {
                    this.rightWeapon = value;
                    this.needStatUpdate = true;
                }
            }
        }

        public ICharacter Target { get; set; }

        public float GetStat(StatType type)
        {
            if (this.needStatUpdate)
            {
                this.UpdateStats();
            }

            if (this.fullStats.ContainsKey(type))
            {
                return this.fullStats[type];
            }

            return 0;
        }

        public void TakeDamage(float damage)
        {   
            //TODO: Apply damage to mech (shields, armor, hull, parts damage, etc.)
        }

        // -------------------------------------------------------------------
        // Private
        // -------------------------------------------------------------------
        private void UpdateStats()
        {
            this.needStatUpdate = false;

            this.fullStats.Clear();
            foreach (StatType type in this.baseStats.Keys)
            {
                this.AddFullStat(type, this.baseStats[type]);
            }

            foreach (StatType type in Enum.GetValues(typeof(StatType)))
            {
                if (this.head != null)
                {
                    this.AddFullStat(type, this.head.GetStat(type));
                }

                if (this.chest != null)
                {
                    this.AddFullStat(type, this.chest.GetStat(type));
                }

                if (this.legs != null)
                {
                    this.AddFullStat(type, this.legs.GetStat(type));
                }

                if (this.leftWeapon != null)
                {
                    this.AddFullStat(type, this.leftWeapon.GetStat(type));
                }

                if (this.rightWeapon != null)
                {
                    this.AddFullStat(type, this.rightWeapon.GetStat(type));
                }
            }
        }

        private void AddFullStat(StatType type, float value)
        {
            if (!this.fullStats.ContainsKey(type))
            {
                this.fullStats.Add(type, 0);
            }

            this.fullStats[type] = StatUtils.CombineStat(type, this.fullStats[type], value);
        }
    }
}
