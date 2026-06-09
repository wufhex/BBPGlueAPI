using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using BBPGlue.API;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class EnvironmentPickupSpawnPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("EnvironmentController, Assembly-CSharp");
            return type?.GetMethod("CreateItem", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPostfix]
        public static void Postfix(object? __result)
        {
            if (__result != null)
                BBP.Callbacks.Items.RaisePickupSpawn(__result);
        }
    }

    [HarmonyPatch]
    public static class EnvironmentLightsChangedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("EnvironmentController, Assembly-CSharp");
            return type?.GetMethod("SetAllLights", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(bool on)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseLightsChanged(on);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class EnvironmentFlickerLightsPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("EnvironmentController, Assembly-CSharp");
            return type?.GetMethod("FlickerLights", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(bool val)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseLightsFlickerChanged(val);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class EnvironmentTimeLimitPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("EnvironmentController, Assembly-CSharp");
            return type?.GetMethod("SetTimeLimit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(float time)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseTimeLimitSet(time);
            });

            return !cancel;
        }
    }
}