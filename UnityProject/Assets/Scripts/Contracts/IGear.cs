namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    using UnityEngine;

    public interface IGear : IStatHolder
    {
        GearType Type { get; }

        string Name { get; }

        bool IsOverheated { get; }

        void Update(GameObject gameObject);
        
        float GetInheritedStat(StatType type);
        
        StatDictionary GetInheritedStats();
    }
}
