using System;
using System.Reflection;
using HarmonyLib;
using BBPGlue.API;
using BBPGlue.Core;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class PlayerItemUsedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("ItemManager, Assembly-CSharp");

            return type?.GetMethod(
                "UseItem",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Player.RaisePlayerItemUsed(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class PlayerItemAddedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("ItemManager, Assembly-CSharp");

            return type?.GetMethod(
                "AddItem",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[]
                {
                    Type.GetType("ItemObject, Assembly-CSharp"),
                    Type.GetType("Pickup, Assembly-CSharp")
                },
                null
            );
        }

        [HarmonyPostfix]
        public static void Postfix(object item)
        {
            BBP.Callbacks.Player.RaisePlayerItemAdded(item);
        }
    }

    [HarmonyPatch]
    public static class PlayerItemRemovedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("ItemManager, Assembly-CSharp");

            return type?.GetMethod(
                "RemoveItem",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(int) },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(int val)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Player.RaisePlayerItemRemoved(val);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class PlayerItemSlotRemovedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("ItemManager, Assembly-CSharp");

            return type?.GetMethod(
                "RemoveItemSlot",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(int) },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(int val)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Player.RaisePlayerItemRemoved(val);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class PlayerSlotChangedPatch
    {
        private static int _lastSlot = -1;

        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("ItemManager, Assembly-CSharp");

            return type?.GetMethod(
                "Update",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            int currentSlot = ReflectionUtil.GetField<int>(
                __instance,
                "selectedItem"
            );

            if (currentSlot == _lastSlot)
                return;

            _lastSlot = currentSlot;

            BBP.Callbacks.Player.RaisePlayerSlotChanged(currentSlot);
        }
    }
}