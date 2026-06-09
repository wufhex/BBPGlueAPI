using System;
using System.Reflection;
using HarmonyLib;
using BBPGlue.API;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class SeedPatch
    {
        public static MethodBase TargetMethod()
        {
            Type? managerType = Type.GetType("CoreGameManager, Assembly-CSharp");

            if (managerType == null)
                throw new Exception("Could not find CoreGameManager.");

            MethodInfo? method = managerType.GetMethod(
                "SetRandomSeed",
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance
            );

            if (method == null)
                throw new Exception("Could not find CoreGameManager.SetRandomSeed().");

            return method;
        }

        [HarmonyPrefix]
        public static bool Prefix(object __instance)
        {
            if (BBP.Seed.OverrideEnabled)
            {
                BBP.Seed.TryApplyOverride(__instance);
                return false;
            }

            return true;
        }
    }
}