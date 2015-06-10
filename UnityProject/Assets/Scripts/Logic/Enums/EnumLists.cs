namespace Assets.Scripts.Logic.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumLists
    {
        public static IList<CombatType> CombatTypes = Enum.GetValues(typeof(CombatType)).Cast<CombatType>().ToList();

        public static IList<DamageType> DamageTypes = Enum.GetValues(typeof(DamageType)).Cast<DamageType>().ToList();

        public static IList<GearType> GearTypes = Enum.GetValues(typeof(GearType)).Cast<GearType>().ToList();

        public static IList<StatType> StatTypes = Enum.GetValues(typeof(StatType)).Cast<StatType>().ToList();

        public static IList<PlayerControl> PlayerControls = Enum.GetValues(typeof(PlayerControl)).Cast<PlayerControl>().ToList();
    }
}
