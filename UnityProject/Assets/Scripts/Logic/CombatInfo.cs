namespace Assets.Scripts.Logic
{
    using Assets.Scripts.Logic.Enums;

    public class CombatInfo
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public CombatInfo()
        {
            this.ModValue = 1f;
            this.LogNMultiplier = 5f;
        }

        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public float Damage { get; set; }

        public float ModValue { get; set; }

        public float LogNMultiplier { get; set; }

        public DamageType DamageType { get; set; }

        public CombatType CombatType { get; set; }
    }
}
