namespace Assets.Scripts.Logic
{
    using Assets.Scripts.Contracts;

    using UnityEngine;

    public class WeaponFireContext
    {
        public GameObject ProjectileParent { get; set; }

        public GameObject Origin { get; set; }

        public ICharacter Character;
    }
}
