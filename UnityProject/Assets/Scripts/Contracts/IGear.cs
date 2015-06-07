namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public interface IGear
    {
        GearType Type { get; }

        string Name { get; }

        void Update(GameObject gameObject);

        float GetInternalStat(StatType type);

        float GetInheritedStat(StatType type);

        StatDictionary GetInternalStats();

        StatDictionary GetInheritedStats();
    }
}
