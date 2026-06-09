using System;
using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using BBPGlue.API;
using BBPGlue.Core;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class ItemRespawnRandomRoomPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("EnvironmentController, Assembly-CSharp");
            Type? itemType = Type.GetType("ItemObject, Assembly-CSharp");

            if (type == null || itemType == null)
                return null;

            return type.GetMethod(
                "RespawnItemInRandomRoom",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { itemType },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object item)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Items.RaiseItemRespawn(item);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class ItemRespawnRoomPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("EnvironmentController, Assembly-CSharp");
            Type? itemType = Type.GetType("ItemObject, Assembly-CSharp");
            Type? roomType = Type.GetType("RoomController, Assembly-CSharp");

            if (type == null || itemType == null || roomType == null)
                return null;

            return type.GetMethod(
                "RespawnItemInRoom",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { itemType, roomType },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object item)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Items.RaiseItemRespawn(item);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class NotebookCollectPatch
    {
        [HarmonyTargetMethods]
        public static IEnumerable<MethodBase> TargetMethods()
        {
            Type? notebookType = AccessTools.TypeByName("Notebook");
            Type? baseType = AccessTools.TypeByName("BaseGameManager");

            if (notebookType == null || baseType == null)
                yield break;

            foreach (Type type in AccessTools.AllTypes())
            {
                if (!baseType.IsAssignableFrom(type))
                    continue;

                MethodInfo? method = AccessTools.Method(
                    type,
                    "CollectNotebook",
                    new[] { notebookType }
                );

                if (method == null)
                    continue;

                if (method.DeclaringType != type)
                    continue;

                yield return method;
            }
        }

        [HarmonyPrefix]
        public static bool Prefix(object notebook)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Items.RaiseNotebookCollect(notebook);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class PickupCollectHudPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("ItemSlotsManager, Assembly-CSharp");
            Type? itemType = Type.GetType("ItemObject, Assembly-CSharp");

            if (type == null || itemType == null)
                return null;

            return type.GetMethod(
                "CollectItem",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(int), itemType },
                null
            );
        }

        [HarmonyPostfix]
        public static void Postfix(int slot, object item)
        {
            BBP.Callbacks.Items.RaisePickupCollect(item);
        }
    }

    [HarmonyPatch]
    public static class PickupDespawnHudPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("ItemSlotsManager, Assembly-CSharp");
            Type? itemType = Type.GetType("ItemObject, Assembly-CSharp");

            if (type == null || itemType == null)
                return null;

            return type.GetMethod(
                "LoseItem",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(int), itemType },
                null
            );
        }

        [HarmonyPostfix]
        public static void Postfix(int slot, object item)
        {
            BBP.Callbacks.Items.RaisePickupDespawn(item);
        }
    }

    [HarmonyPatch]
    public static class LockerItemChangedPatch
    {
        private static object?[] _lastLockerItems = new object?[3];

        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("CoreGameManager, Assembly-CSharp");

            return type?.GetMethod(
                "Update",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            Array? current = ReflectionUtil.GetField<Array>(
                __instance,
                "currentLockerItems"
            );

            if (current == null)
                return;

            int count = Math.Min(current.Length, _lastLockerItems.Length);

            for (int i = 0; i < count; i++)
            {
                object? item = current.GetValue(i);

                if (ReferenceEquals(item, _lastLockerItems[i]))
                    continue;

                _lastLockerItems[i] = item;
                BBP.Callbacks.Items.RaiseLockerItemChanged(i, item);
            }
        }
    }

    [HarmonyPatch]
    internal static class ItemManagerUseItemPatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("ItemManager"),
                "UseItem"
            );
        }

        [HarmonyPrefix]
        private static bool Prefix(object __instance)
        {
            int selectedItem =
                ReflectionUtil.GetField<int>(__instance, "selectedItem");

            object? rawItems =
                ReflectionUtil.GetField<object>(__instance, "items");

            if (rawItems is not Array items)
                return true;

            if (selectedItem < 0 || selectedItem >= items.Length)
                return true;

            object? itemObject = items.GetValue(selectedItem);

            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Items.RaiseItemUse(itemObject);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    internal static class PickupClickedPatch
    {
        [HarmonyTargetMethod]
        private static MethodBase? TargetMethod()
        {
            return AccessTools.Method(
                AccessTools.TypeByName("Pickup"),
                "Clicked",
                new[] { typeof(int) }
            );
        }

        [HarmonyPrefix]
        private static bool Prefix(object __instance, int player)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Items.RaisePickupClicked(__instance, player);
            });

            return !cancel;
        }
    }
}