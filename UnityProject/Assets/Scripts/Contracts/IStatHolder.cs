namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;
    using Assets.Scripts.Logic.Enums;

    public interface IStatHolder
    {
        float GetCurrentStat(StatType type);
        float GetMaxStat(StatType type);

        void SetBaseStats(StatDictionary baseStats);

        void SetTemporaryStat(StatType type, float value);

        void RemoveTemporaryStat(StatType type);

        void ModifyStat(StatType type, float modifier);

        void SetStat(StatType type, float value);

        void ResetCurrentStats();
    };
}
