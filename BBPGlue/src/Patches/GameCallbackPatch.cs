using System;
using System.Reflection;
using HarmonyLib;
using BBPGlue.API;
using BBPGlue.Core;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class GameStartPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("ElevatorScreen, Assembly-CSharp");
            return type?.GetMethod("StartGame", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix()
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Game.RaiseGameStart();
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class LevelLoadRequestedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("Elevator, Assembly-CSharp");
            return type?.GetMethod("ButtonPressed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            BBP.Callbacks.Game.RaiseLevelLoadRequested();
        }
    }

    [HarmonyPatch]
    public static class LevelReadyPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("BaseGameManager, Assembly-CSharp");
            return type?.GetMethod("BeginPlay", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix()
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Game.RaiseLevelReady();
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class LevelExitPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("BaseGameManager, Assembly-CSharp");
            return type?.GetMethod("EndGame", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix()
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Game.RaiseLevelExit();
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class SeedSetRandomPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("CoreGameManager, Assembly-CSharp");
            return type?.GetMethod("SetRandomSeed", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            int seed = ReflectionUtil.GetField<int>(__instance, "seed");
            BBP.Callbacks.Game.RaiseSeedSet(seed);
        }
    }

    // Breaks keyboard listener for some reason, we comment it out for now.
    // Default game mostly calls SetRandomSeed anyways.
    // [HarmonyPatch]
    // public static class SeedSetPatch
    // {
    //     public static MethodBase? TargetMethod()
    //     {
    //         Type? type = Type.GetType("CoreGameManager, Assembly-CSharp");
    //         return type?.GetMethod(
    //             "SetSeed",
    //             BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
    //             null,
    //             new[] { typeof(int) },
    //             null
    //         );
    //     }

    //     [HarmonyPostfix]
    //     public static void Postfix(int seed)
    //     {
    //         BBP.Callbacks.Game.RaiseSeedSet(seed);
    //     }
    // }

    [HarmonyPatch]
    public static class SaveLoadedPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("GameLoader, Assembly-CSharp");
            return type?.GetMethod("LoadSavedGame", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            BBP.Callbacks.Game.RaiseSaveLoaded();
        }
    }

    [HarmonyPatch]
    public static class SaveWrittenPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("CoreGameManager, Assembly-CSharp");
            return type?.GetMethod("Save", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPostfix]
        public static void Postfix()
        {
            BBP.Callbacks.Game.RaiseSaveWritten();
        }
    }

    [HarmonyPatch]
    public static class GamePausePatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("CoreGameManager, Assembly-CSharp");

            return type?.GetMethod(
                "Pause",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[] { typeof(bool) },
                null
            );
        }

        [HarmonyPrefix]
        public static bool Prefix(bool openScreen)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Game.RaiseGamePauseChanged();
            });

            return !cancel;
        }
    }
}