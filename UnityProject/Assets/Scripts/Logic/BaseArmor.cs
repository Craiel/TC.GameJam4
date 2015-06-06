namespace Assets.Scripts.Logic
{
    using Assets.Scripts.Contracts;

    public class BaseArmor : BaseGear, IArmor
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public BaseArmor()
        {
            foreach (StatType type in StaticSettings.ArmorBaseStats.Keys)
            {
                this.SetStat(type, StaticSettings.ArmorBaseStats[type]);
            }
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
    }
}
