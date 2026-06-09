using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using BBPGlue.API;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class PlayerTeleportPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("PlayerManager, Assembly-CSharp");

            return type?.GetMethod(
                "Teleport",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance, Vector3 pos)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Player.RaisePlayerTeleport(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class PlayerRuleBreakPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("PlayerManager, Assembly-CSharp");

            return type?.GetMethod(
                "RuleBreak",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(string), typeof(float), typeof(float) },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(string rule)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Player.RaisePlayerRuleBreak(rule);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class PlayerClearGuiltPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("PlayerManager, Assembly-CSharp");

            return type?.GetMethod(
                "ClearGuilt",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix()
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Player.RaisePlayerGuiltClear();
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    internal static class PrincipalSendToDetentionPatch
    {
        [HarmonyTargetMethod]
        private static System.Reflection.MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("Principal"),
                "SendToDetention",
                new[] { typeof(bool) }
            );
        }

        [HarmonyPrefix]
        private static void Prefix(object __instance, bool validCollision)
        {
            BBP.Callbacks.Player.RaisePlayerDetention(
                new BBPPrincipal(__instance),
                validCollision
            );
        }
    }
}