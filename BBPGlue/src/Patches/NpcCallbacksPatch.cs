using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using BBPGlue.API;
using BBPGlue.Core;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class EnvironmentSpawnNpcPatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("EnvironmentController"),
                "SpawnNPC",
                new[]
                {
                    AccessTools.TypeByName("NPC"),
                    AccessTools.TypeByName("IntVector2")
                }
            );
        }

        [HarmonyPrefix]
        private static bool Prefix(
            object npc,
            object position,
            ref object? __result
        )
        {
            int x = ReflectionUtil.GetField<int>(position, "x");
            int z = ReflectionUtil.GetField<int>(position, "z");

            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseNpcSpawnRequested(
                    npc,
                    x,
                    z
                );
            });

            if (!cancel)
                return true;

            __result = null;
            return false;
        }

        [HarmonyPostfix]
        private static void Postfix(
            object? __result,
            object position
        )
        {
            if (__result == null)
                return;

            int x = ReflectionUtil.GetField<int>(position, "x");
            int z = ReflectionUtil.GetField<int>(position, "z");

            BBP.Callbacks.Entity.RaiseNpcSpawned(
                __result,
                x,
                z
            );
        }
    }

    [HarmonyPatch]
    public static class NpcDespawnPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("NPC, Assembly-CSharp");

            return type?.GetMethod(
                "Despawn",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Entity.RaiseNpcDespawn(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    internal static class NpcUpdatePatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("NPC"),
                "Update"
            );
        }

        [HarmonyPostfix]
        private static void Postfix(object __instance)
        {
            BBP.Callbacks.Entity.RaiseNpcUpdate(__instance);
        }
    }

    [HarmonyPatch]
    internal static class NpcTargetPositionPatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("NPC"),
                "TargetPosition",
                new[] { typeof(Vector3) }
            );
        }

        [HarmonyPrefix]
        private static void Prefix(object __instance, Vector3 target)
        {
            BBP.Callbacks.Entity.RaiseNpcTargetPosition(__instance, target);
        }
    }

    [HarmonyPatch]
    internal static class NpcNavigationDecisionPatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("NPC"),
                "MadeNavigationDecision"
            );
        }

        [HarmonyPrefix]
        private static void Prefix(object __instance)
        {
            BBP.Callbacks.Entity.RaiseNpcNavigationDecision(__instance);
        }
    }

    [HarmonyPatch]
    internal static class NpcTriggerEnterPatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("NPC"),
                "EntityTriggerEnter",
                new[]
                {
                    AccessTools.TypeByName("Entity"),
                    typeof(Collider),
                    typeof(bool)
                }
            );
        }

        [HarmonyPrefix]
        private static void Prefix(object __instance, object otherEntity, Collider other, bool validCollision)
        {
            BBP.Callbacks.Entity.RaiseNpcTriggerEnter(__instance, otherEntity, other, validCollision);
        }
    }

    [HarmonyPatch]
    internal static class NpcTriggerExitPatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("NPC"),
                "EntityTriggerExit",
                new[]
                {
                    AccessTools.TypeByName("Entity"),
                    typeof(Collider),
                    typeof(bool)
                }
            );
        }

        [HarmonyPrefix]
        private static void Prefix(object __instance, object otherEntity, Collider other, bool validCollision)
        {
            BBP.Callbacks.Entity.RaiseNpcTriggerExit(__instance, otherEntity, other, validCollision);
        }
    }

    [HarmonyPatch]
    internal static class NpcTriggerStayPatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("NPC"),
                "EntityTriggerStay",
                new[]
                {
                    AccessTools.TypeByName("Entity"),
                    typeof(Collider),
                    typeof(bool)
                }
            );
        }

        [HarmonyPrefix]
        private static void Prefix(object __instance, object otherEntity, Collider other, bool validCollision)
        {
            BBP.Callbacks.Entity.RaiseNpcTriggerStay(__instance, otherEntity, other, validCollision);
        }
    }
}