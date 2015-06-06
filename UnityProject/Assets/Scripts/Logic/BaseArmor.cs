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
            this.InternalStats.Merge(StaticSettings.ArmorBaseStats);
            this.InternalStats.Merge(internalStats);

            this.InheritedStats.Merge(inheritedStats);
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
    }
}
