namespace Assets.Scripts.Armor
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    public class DefaultChestArmor : BaseArmor
    {
        public DefaultChestArmor(StatDictionary instarnalStats, StatDictionary inheritedStats)
            : base(instarnalStats, inheritedStats)
        {
            this.Name = "Chest";

            this.Type = GearType.Chest;
        }
    }
}
