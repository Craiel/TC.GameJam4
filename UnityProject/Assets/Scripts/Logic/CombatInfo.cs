namespace Assets.Scripts.Logic
{
    using Assets.Scripts.Logic.Enums;

    public class CombatInfo
    {
        // -------------------------------------------------------------------
        // Public
        // -------------------------------------------------------------------
        public float Damage { get; set; }

        public DamageType DamageType { get; set; }

        public CombatType CombatType { get; set; }
    }
}
