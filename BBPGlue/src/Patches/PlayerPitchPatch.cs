using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using BBPGlue.Core;
using BBPGlue.API;

namespace BBPGlue.Patches
{
    [HarmonyPatch]
    public static class PlayerPitchPatch
    {
        public static float Pitch;
        public static float Sensitivity = 1f;
        public static float MinPitch = -89f;
        public static float MaxPitch = 89f;

        public static MethodBase? TargetMethod()
        {
            Type? type = Type.GetType("PlayerMovement, Assembly-CSharp");

            return type?.GetMethod(
                "MouseMove",
                BindingFlags.Instance | BindingFlags.NonPublic
            );
        }

        [HarmonyPostfix]
        public static void Postfix(object __instance)
        {
            if (!BBP.Experimental.Player.PitchUnlocked )
                return;

            object? pm = ReflectionUtil.GetField<object>(__instance, "pm");
            if (pm == null)
                return;

            Transform? cameraBase = ReflectionUtil.GetField<Transform>(pm, "cameraBase");
            if (cameraBase == null)
                return;

            object? cameraAnalogData = ReflectionUtil.GetField<object>(
                __instance,
                "cameraAnalogData"
            );

            if (cameraAnalogData == null)
                return;

            Vector2 absolute;
            Vector2 delta;

            object? inputManager = ReflectionUtil.GetSingletonInstance("InputManager");
            if (inputManager == null)
                return;

            object? pfm = ReflectionUtil.GetSingletonInstance("PlayerFileManager");
            if (pfm == null)
                return;

            // GetAnalogInput(cameraAnalogData, out absolute, out delta, 0.1f)
            object[] args =
            {
                cameraAnalogData,
                Vector2.zero,
                Vector2.zero,
                0.1f
            };

            MethodInfo? getAnalogInput = inputManager.GetType().GetMethod(
                "GetAnalogInput",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new[]
                {
                    cameraAnalogData.GetType(),
                    typeof(Vector2).MakeByRefType(),
                    typeof(Vector2).MakeByRefType(),
                    typeof(float)
                },
                null
            );

            if (getAnalogInput == null)
                return;

            getAnalogInput.Invoke(inputManager, args);

            absolute = (Vector2)args[1];
            delta = (Vector2)args[2];

            float mouseSensitivity =
                ReflectionUtil.GetField<float>(pfm, "mouseCameraSensitivity");

            float controllerSensitivity =
                ReflectionUtil.GetField<float>(pfm, "controllerCameraSensitivity");

            float pitchDelta =
                delta.y * mouseSensitivity +
                absolute.y * Time.deltaTime * controllerSensitivity;

            Pitch -= pitchDelta * Sensitivity * Time.timeScale;
            Pitch = Mathf.Clamp(Pitch, MinPitch, MaxPitch);

            cameraBase.localRotation = Quaternion.Euler(Pitch, 0f, 0f);
        }
    }
}