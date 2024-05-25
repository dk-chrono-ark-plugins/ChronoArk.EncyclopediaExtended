using ChronoArkMod.Helper;
using HarmonyLib;

namespace EEx.Implementation.Temp;

[HarmonyPatch]
public static class MonsterCollectionPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(MonsterCollection), "Init")]
    private static void OnInit(MonsterCollection __instance)
    {
        __instance.InfoWindow.GetOrAddComponent<MonsterDataDisplay>();
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MonsterCollection), "LoadMonsterStat")]
    private static void OnLoadMonsterStat(MonsterCollection __instance, int n)
    {
        CoroutineHelper.Deferred(
            () => {
                var data = __instance.GetComponentInChildren<MonsterDataDisplay>();
                data.UpdateAttackNum(__instance.AllList[n].atk);
                data.UpdateHitpointNum(__instance.AllList[n].maxhp);
                data.UpdateHitrateNum(__instance.AllList[n].hit);
                data.UpdateSpeedNum(__instance.AllList[n].spd);
            },
            () => __instance.GetComponentInChildren<MonsterDataDisplay>() != null);
    }
}