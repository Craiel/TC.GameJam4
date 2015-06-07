namespace Assets.Scripts.Logic
{
    using Assets.Scripts.Contracts;

    public abstract class BaseArmor : BaseGear, IArmor
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        protected BaseArmor(StatDictionary internalStats, StatDictionary inheritedStats)
        {
            StatDictionary baseStats = new StatDictionary();
            baseStats.Merge(StaticSettings.ArmorBaseStats);
            baseStats.Merge(internalStats);
            this.SetBaseStats(baseStats);

            this.SetInheritedStats(inheritedStats);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
    }
}
