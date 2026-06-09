using System;
using System.Reflection;
using HarmonyLib;
using BBPGlue.API;
using BBPGlue.Core;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class PlayerStaminaChangedPatch
    {
        private static float _lastStamina = -1f;

        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("PlayerMovement, Assembly-CSharp");

            return type?.GetMethod(
                "StaminaUpdate",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            float stamina = ReflectionUtil.GetField<float>(__instance, "stamina");

            if (Math.Abs(stamina - _lastStamina) < 0.001f)
                return;

            _lastStamina = stamina;
            BBP.Callbacks.Player.RaisePlayerStaminaChanged(stamina);
        }
    }
}