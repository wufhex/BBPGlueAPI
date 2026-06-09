using System;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HarmonyLib;
using BBPGlue.API;
using BBPGlue.Core;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class DoorOpenPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Door, Assembly-CSharp");

            return type?.GetMethod(
                "Open",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseDoorOpen(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class DoorClosePatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Door, Assembly-CSharp");

            return type?.GetMethod(
                "Shut",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseDoorClose(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class ElevatorButtonPressedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Elevator, Assembly-CSharp");
            return type?.GetMethod("ButtonPressed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseElevatorEnter(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class ElevatorPlayerExitPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Elevator, Assembly-CSharp");
            return type?.GetMethod("WaitingForPlayerExitStateUpdate", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            BBP.Callbacks.World.RaiseElevatorExit(__instance);
        }
    }

    [HarmonyPatch]
    public static class RandomEventQueuedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("EnvironmentController, Assembly-CSharp");
            Type? eventType = Type.GetType("RandomEvent, Assembly-CSharp");

            if (type == null || eventType == null)
                return null;

            return type.GetMethod(
                "AddEvent",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { eventType, typeof(float) },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object randomEvent)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseRandomEventQueued(randomEvent);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class RandomEventBeginPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("RandomEvent, Assembly-CSharp");

            return type?.GetMethod(
                "Begin",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseRandomEventBegin(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class RandomEventEndPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("RandomEvent, Assembly-CSharp");

            return type?.GetMethod(
                "End",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseRandomEventEnd(__instance);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class TimerWarningPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("BaldiTV, Assembly-CSharp");
            Type? ecType = Type.GetType("EnvironmentController, Assembly-CSharp");

            if (type == null || ecType == null)
                return null;

            return type.GetMethod(
                "ShowLevelTimeWarning",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { ecType },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix()
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseTimerWarning();
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class WindowBreakPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Window, Assembly-CSharp");

            return type?.GetMethod(
                "Break",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(bool) },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.World.RaiseWindowBreak(__instance);
            });

            return !cancel;
        }
    }

    
    internal static class RoomChangeTracker
    {
        private static readonly Dictionary<int, object?> LastRooms =
            new Dictionary<int, object?>();

        public static void Check(object instance)
        {
            int id = RuntimeHelpers.GetHashCode(instance);

            object? currentRoom = ReflectionUtil.GetField<object>(
                instance,
                "currentRoom"
            );

            LastRooms.TryGetValue(id, out object? lastRoom);

            if (ReferenceEquals(lastRoom, currentRoom))
                return;

            if (lastRoom != null)
                BBP.Callbacks.World.RaiseRoomExit(lastRoom);

            if (currentRoom != null)
                BBP.Callbacks.World.RaiseRoomEnter(currentRoom);

            LastRooms[id] = currentRoom;
        }
    }

    [HarmonyPatch]
    public static class PlayerRoomChangedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("PlayerManager, Assembly-CSharp");

            return type?.GetMethod(
                "Update",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            RoomChangeTracker.Check(__instance);
        }
    }

    [HarmonyPatch]
    public static class NpcRoomChangedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("NPC, Assembly-CSharp");

            return type?.GetMethod(
                "Update",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            RoomChangeTracker.Check(__instance);
        }
    }
}