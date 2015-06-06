namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;

    using UnityEngine;

    public interface IGear
    {
        string Name { get; }

        void Update(GameObject gameObject);

        float GetInternalStat(StatType type);

        float GetInheritedStat(StatType type);

        StatDictionary GetInternalStats();

        StatDictionary GetInheritedStats();
    }
}
