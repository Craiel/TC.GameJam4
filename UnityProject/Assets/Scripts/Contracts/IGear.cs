namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;

    using UnityEngine;

    public interface IGear
    {
        string Name { get; }

        float GetStat(StatType type);

        void Update(GameObject gameObject);
    }
}
