﻿namespace Assets.Scripts.Armor
{
    using Assets.Scripts.Logic;

    public class DefaultHeadArmor : BaseArmor
    {
        public DefaultHeadArmor(StatDictionary instarnalStats, StatDictionary inheritedStats)
            : base(instarnalStats, inheritedStats)
        {
            this.Name = "Head";

            this.Type = GearType.Head;
        }
    }
}
