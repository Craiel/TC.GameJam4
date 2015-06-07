namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    using InControl;

    using UnityEngine;

    public interface ICharacter
    {
        int Id { get; }

        string Name { get; set; }

        ICharacter Target { get; set; }

        InputDevice InputDevice { get; set; }

        IGear GetGear(GearType type);

        void SetGear(GearType type, IGear newGear);

        void RemoveGear(GearType type);

        float GetCurrentStat(StatType type);
        float GetMaxStat(StatType type);

        void SetBaseStats(StatDictionary baseStats);

        void SetTemporaryStat(StatType type, float value);

        void RemoveTemporaryStat(StatType type);

        void TakeDamage(float damage);

        void Update(GameObject gameObject);
    }
}
