using System;
using System.Reflection;
using HarmonyLib;
using BBPGlue.API;
using BBPGlue.Core;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class PlayerSpawnPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("CoreGameManager, Assembly-CSharp");

            return type?.GetMethod(
                "SpawnPlayers",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            object? players = ReflectionUtil.GetField<object>(__instance, "players");

            if (players is System.Collections.IEnumerable enumerable)
            {
                foreach (object? player in enumerable)
                {
                    if (player != null)
                        BBP.Callbacks.Player.RaisePlayerSpawn(player);
                }
            }
        }
    }

    [HarmonyPatch]
    public static class PlayerHiddenChangedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("PlayerManager, Assembly-CSharp");

            return type?.GetMethod(
                "SetHidden",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(bool value)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Player.RaisePlayerHiddenChanged(value);
            });

            return !cancel;
        }
    }
}