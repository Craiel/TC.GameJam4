namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic.Enums;

    using InControl;

    using UnityEngine;

    public interface ICharacter : IStatHolder
    {
        int Id { get; }

        string Name { get; set; }

        Color myColor { get; set; }

        ICharacter Target { get; set; }

        InputDevice InputDevice { get; set; }

        IGear GetGear(GearType type);

        void SetGear(GearType type, IGear newGear);

        void RemoveGear(GearType type);
        
        void TakeDamage(float damage);

        void Update(GameObject gameObject);
    }
}
