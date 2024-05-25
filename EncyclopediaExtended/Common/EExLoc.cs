using ChronoArkMod.ModData;

namespace EEx.Common;

internal static class EExLoc
{
    private static string I2Loc(this ModInfo? modInfo, string key)
    {
        return modInfo?.localizationInfo.SystemLocalizationUpdate(key) ?? string.Empty;
    }

    internal static class Stat
    {
        public static readonly string Attack = EExMod.ModInfo.I2Loc("EEx/Stat/Attack");
        public static readonly string Hitpoint = EExMod.ModInfo.I2Loc("EEx/Stat/Hitpoint");
        public static readonly string Hitrate = EExMod.ModInfo.I2Loc("EEx/Stat/Hitrate");
        public static readonly string Speed = EExMod.ModInfo.I2Loc("EEx/Stat/Speed");
    }
}