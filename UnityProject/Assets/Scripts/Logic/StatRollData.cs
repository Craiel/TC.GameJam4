namespace Assets.Scripts.Logic
{
    using System.Collections.Generic;

    using Assets.Scripts.Logic.Enums;

    public class StatRollData
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public StatRollData()
        {
            this.RequiredStats = new List<StatType>();
            this.OptionalStats = new List<StatType>();
            this.InternalStats = new List<StatType>();
            this.BaseLineValues = new StatDictionary();
            this.BudgetValues = new StatDictionary();
            this.FixedStats = new StatDictionary();
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public int OptionalStatPicks { get; set; }

        public IList<StatType> RequiredStats { get; private set; }
        public IList<StatType> OptionalStats { get; private set; }
        public IList<StatType> InternalStats { get; private set; }

        public StatDictionary BaseLineValues { get; private set; }
        public StatDictionary BudgetValues { get; private set; }
        public StatDictionary FixedStats { get; private set; }
    }
}
