using System;
using System.Linq;
using System.Reflection;

using BepInEx;
using HarmonyLib;
using UnityEngine;
using BBPGlue.API;
using BBPGlue.Core;
using BBPGlue.Tests;

namespace BBPGlue
{
    [BepInPlugin(Guid, Name, Version)]
    public sealed class Plugin : BaseUnityPlugin
    {
        public const  string Guid        = "com.wufhex.BBPGlue";
        public const  string Name        = "BBP Glue";
        public const  string Version     = "0.0.1";

        private const string GameVersion = "0.14.3";
        private const string HrmPkg      = ".patches"; 
        
        private Harmony? _harmony;

        private void Awake()
        {            
            BBP.Logger = Logger;

            if (BBP.Game.Version != GameVersion)
            {
                string warning =
                    $"BBPGlue was built for BB+ {GameVersion} " +
                    $"but detected {BBP.Game.Version}. " +
                    $"The API may not work correctly.";

                Logger.LogWarning(warning);

                BBPConsole.Warn("========================================");
                BBPConsole.Warn(" BBP GLUE VERSION MISMATCH ");
                BBPConsole.Warn($" Expected: {GameVersion}");
                BBPConsole.Warn($" Detected: {BBP.Game.Version}");
                BBPConsole.Warn(" Some features may be broken.");
                BBPConsole.Warn("========================================");
            }

            try
            {
                _harmony = new Harmony(Guid + HrmPkg);
            }
            catch (Exception ex)
            {
                BBPConsole.Error("Failed to create Harmony instance:");
                BBPConsole.Error(ex.ToString());
                return;
            }

            try
            {
                PatchAll();
            }
            catch (Exception ex)
            {
                Logger.LogError("PatchAll failed!");
                Logger.LogError(ex);

                BBPConsole.Error("PatchAll failed:");
                BBPConsole.Error(ex.ToString());

                if (ex.InnerException != null)
                {
                    BBPConsole.Error("Inner Exception:");
                    BBPConsole.Error(ex.InnerException.ToString());
                }

                return;
            }

            BBPConsole.Log("Wufhex's BBP Glue API loaded!");
            BBPConsole.Log(
                $"Game Version: {BBP.Game.Version} | API Version: {Version}"
            );

            // CallbackTest.Register();
            // CustomPrefabsTest.Register();
        }

        private void Update()
        {
            GameContext.Refresh();
            BBP.Update();

            BBPConsole.Update();
            DebugMenu.Update();

            // CustomPrefabsTest.Update();
        }

        private void OnGUI()
        {
            BBPConsole.OnGUI();
            DebugMenu.OnGUI();
        }

        private void PatchAll() {
            try
            {
                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (!type.GetCustomAttributes(typeof(HarmonyPatch), false).Any())
                        continue;

                    try
                    {
                        _harmony?.CreateClassProcessor(type).Patch();
                    }
                    catch (Exception ex)
                    {
                        BBPConsole.Error($"Failed patch: {type.FullName}");
                        BBPConsole.Error(ex.ToString());
                        BBPConsole.Error("Functions using this patch will not work.");
                        continue;
                    }
                }
            }
            catch
            {
                return;
            }
        }
    }
}