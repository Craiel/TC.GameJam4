namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    public class StatRollData
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public StatRollData()
        {
            this.RequiredStats = new List<StatType>();
            this.OptionalStats = new List<StatType>();
            this.BudgetValues = new StatDictionary();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public int OptionalStatPicks { get; set; }

        public IList<StatType> RequiredStats { get; private set; }
        public IList<StatType> OptionalStats { get; private set; }

        public StatDictionary BudgetValues { get; private set; }
    }
}
