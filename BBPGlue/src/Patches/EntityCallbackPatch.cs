using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using BBPGlue.API;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class NpcHearNoisePatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("NPC, Assembly-CSharp");
            return type?.GetMethod("Hear", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseNpcHearNoise(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class NpcSightPlayerPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("NPC, Assembly-CSharp");
            return type?.GetMethod("PlayerSighted", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseNpcSightPlayer(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class NpcLosePlayerPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("NPC, Assembly-CSharp");
            return type?.GetMethod("PlayerLost", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseNpcLosePlayer(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class EntitySpawnPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Entity, Assembly-CSharp");
            return type?.GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            BBP.Callbacks.Entity.RaiseEntitySpawn(__instance);
        }
    }

    [HarmonyPatch]
    public static class EntityTeleportPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Entity, Assembly-CSharp");
            return type?.GetMethod("Teleport", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            BBP.Callbacks.Entity.RaiseEntityTeleport(__instance);
        }
    }

    [HarmonyPatch]
    public static class EntityFrozenChangedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Entity, Assembly-CSharp");
            return type?.GetMethod("SetFrozen", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance, bool value)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseEntityFrozenChanged(__instance, value);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class EntityHiddenChangedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Entity, Assembly-CSharp");
            return type?.GetMethod("SetHidden", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance, bool value)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseEntityHiddenChanged(__instance, value);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class EntitySquishedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Entity, Assembly-CSharp");
            return type?.GetMethod("Squish", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseEntitySquished(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class EntityUnsquishedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Entity, Assembly-CSharp");
            return type?.GetMethod("Unsquish", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseEntityUnsquished(__instance);
            });

            return !cancel;
        }
    }
}