using System;
using System.Reflection;
using HarmonyLib;
using BBPGlue.API;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class HudHideChangedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("HudManager, Assembly-CSharp");
            return type?.GetMethod("Hide", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(bool val)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Hud.RaiseHudHideChanged(val);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class HudTooltipShownPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("HudManager, Assembly-CSharp");
            return type?.GetMethod("SetTooltip", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(string key)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Hud.RaiseTooltipShown(key);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class MapOpenPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("CoreGameManager, Assembly-CSharp");

            return type?.GetMethod(
                "OpenMap",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(bool) },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(bool toMap)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Hud.RaiseMapOpen(toMap);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class MapClosePatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("CoreGameManager, Assembly-CSharp");

            return type?.GetMethod(
                "CloseMap",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix()
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Hud.RaiseMapClose();
            });

            return !cancel;
        }
    }
}