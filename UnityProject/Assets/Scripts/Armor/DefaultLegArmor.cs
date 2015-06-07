namespace Assets.Scripts.Armor
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    public class DefaultLegArmor : BaseArmor
    {
        public DefaultLegArmor(StatDictionary instarnalStats, StatDictionary inheritedStats)
            : base(instarnalStats, inheritedStats)
        {
            this.Name = "Legs";

            this.Type = GearType.Legs;
        }
    }
}
