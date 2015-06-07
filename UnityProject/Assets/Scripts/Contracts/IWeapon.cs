namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;
    
    public interface IWeapon : IGear
    {
        long ShotsFired { get; }

        float LastShotFired { get; }
        
        long ProjectileLimit { get; }

        bool IsTargeted { get; set; }

        DamageType DamageType { get; set; }

        bool CanFire();

        void SetWeaponGearType(GearType type);

        void Fire(WeaponFireContext context);
    }
}
