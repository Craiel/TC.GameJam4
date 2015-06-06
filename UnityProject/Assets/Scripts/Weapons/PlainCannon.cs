namespace Assets.Scripts.Weapons
{
    using System.Collections.Generic;

    using Assets.Scripts.Contracts;
    using Assets.Scripts.Logic;

    public class PlainCannon : BaseWeapon
    {
        protected override IList<IProjectile> DoFire()
        {
            // Create 1 projectile in the facing direction of the "host"

            return null;
        }
    }
}
