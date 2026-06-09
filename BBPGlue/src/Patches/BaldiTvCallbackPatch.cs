using System;
using System.Reflection;
using HarmonyLib;
using BBPGlue.API;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class BaldiTvSpeakPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("BaldiTV, Assembly-CSharp");
            return type?.GetMethod("Speak", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object sound)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Hud.RaiseBaldiTvSpeak(sound);
            });

            return !cancel;
        }
    }

    [HarmonyPatch]
    public static class BaldiTvAnnounceEventPatch
    {
        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("BaldiTV, Assembly-CSharp");
            return type?.GetMethod("AnnounceEvent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        [HarmonyPrefix]
        public static bool Prefix(object sound)
        {
            bool cancel = BBPCallbacks.RunCancelable(() =>
            {
                BBP.Callbacks.Hud.RaiseAnnouncementQueued(sound);
                BBP.Callbacks.World.RaiseRandomEventAnnounced(sound);
            });

            return !cancel;
        }
    }
}