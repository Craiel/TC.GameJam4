namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;

    public interface IGear
    {
        string Name { get; }

        float GetStat(StatType type);
    }
}
