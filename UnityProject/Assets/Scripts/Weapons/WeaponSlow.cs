namespace Assets.Scripts.Weapons
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;
    
    public class WeaponSlow : BaseWeapon
    {
        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------
        public WeaponSlow(StatDictionary internalStats)
            : base(internalStats)
        {
            this.Name = "Slow";

            var stats = new StatDictionary
                {
                    { StatType.Interval, 0.1f },
                    { StatType.HeatGeneration, 1.0f },
                };

            this.SetBaseStats(stats);
        }

        // -------------------------------------------------------------------
        // Protected
        // -------------------------------------------------------------------
        protected override void DoFire(WeaponFireContext context)
        {
            // Todo: 
        }
    }
}
