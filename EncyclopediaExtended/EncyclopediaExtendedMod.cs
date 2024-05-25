global using System;
global using System.Collections.Generic;
global using System.Linq;
global using UnityEngine;
using ChronoArkMod;
using ChronoArkMod.ModData;
using ChronoArkMod.Plugin;
using HarmonyLib;

namespace EEx;

internal class EExMod : ChronoArkPlugin
{
    private Harmony? _harmony;
    public static ModInfo ModInfo => ModManager.getModInfo(Instance!.ModId);
    public static EExMod? Instance { get; private set; }

    public override void Dispose()
    {
        Instance = null;
    }

    public override void Initialize()
    {
        Instance = this;
        _harmony = new(GetGuid());
        _harmony.PatchAll();
    }
}