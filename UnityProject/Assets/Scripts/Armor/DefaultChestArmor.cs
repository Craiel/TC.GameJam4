namespace Assets.Scripts.Armor
{
    using Assets.Scripts.Logic;

    public class DefaultChestArmor : BaseArmor
    {
        public DefaultChestArmor(StatDictionary instarnalStats, StatDictionary inheritedStats)
            : base(instarnalStats, inheritedStats)
        {
            this.Type = GearType.Chest;
        }
    }
}
