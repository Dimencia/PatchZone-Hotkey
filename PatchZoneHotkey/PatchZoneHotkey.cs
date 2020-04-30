using System;
using PatchZone.Hatch;
using PatchZone.Hatch.Utils;
using PatchZoneHotkey.Services;

using ECS;
using Service.Achievement;
using Service.Building;
using Service.Localization;
using Service.Street;
using Service.UserWorldTasks;
using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UserInterface;

namespace PatchZoneHotkey
{
    public class PatchZoneHotkey : Singleton<PatchZoneHotkey>, IPatchZoneMod
    {
        public static IPatchZoneContext Context { get; private set; }
        public static Harmony Harmony;

        public void Init(IPatchZoneContext context)
        {
            Context = context;
            Harmony = new Harmony("PatchZoneHotkey");
        }

        public void OnBeforeGameStart()
        {
            Context.Log.Log("Adding PatchZone Hotkey");
            Harmony.PatchAll();
            Context.Log.Log("PatchZone Hotkey added");
        }
    }
}
