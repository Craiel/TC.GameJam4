namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    using UnityEngine;

    public class WeaponInvisibility : BaseWeapon
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public WeaponInvisibility(StatDictionary internalStats)
            : base(internalStats)
        {
            this.Name = "Invisibility";

            var stats = new StatDictionary
                {
                    { StatType.Interval, 0.1f },
                    { StatType.HeatGeneration, 1.0f },
                };

            this.InternalStats.Merge(stats);
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override IList<ProjectileBehavior> DoFire(GameObject origin, ICharacter source)
        {
            // Todo: 
            return null;
        }
    }
}
