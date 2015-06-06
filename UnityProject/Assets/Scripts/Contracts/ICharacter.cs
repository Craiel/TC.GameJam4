namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;

    using InControl;

    using UnityEngine;

    public interface ICharacter
    {
        string Name { get; set; }

        IArmor Head { get; set; }
        IArmor Chest { get; set; }
        IArmor Legs { get; set; }

        IWeapon LeftWeapon { get; set; }
        IWeapon RightWeapon { get; set; }

        ICharacter Target { get; set; }

        InputDevice InputDevice { get; set; }

        float GetStat(StatType type);

        void TakeDamage(float damage);

        void Update(GameObject gameObject);
    }
}
