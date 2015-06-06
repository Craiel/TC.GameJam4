namespace Assets.Scripts.Contracts
{
    using Assets.Scripts.Logic;

    public interface IActor
    {
        IArmor Head { get; set; }
        IArmor Chest { get; set; }
        IArmor Legs { get; set; }

        IWeapon LeftWeapon { get; set; }
        IWeapon RightWeapon { get; set; }

        float GetStat(StatType type);
    }
}
