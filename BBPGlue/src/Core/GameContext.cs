using System;
using System.Reflection;
using UnityEngine;
using BBPGlue.API;

namespace BBPGlue.Core
{
    public static class GameContext
    {
        public static Component? Player { get; private set; }
        public static Type? PlayerType { get; private set; }

        public static Type? CoreGameManagerType { get; private set; }
        public static Type? BaseGameManagerType { get; private set; }

        private static PropertyInfo? _coreInstanceProp;
        private static PropertyInfo? _baseInstanceProp;

        private static float _nextRefreshTime;

        public static bool IsInGame => Player != null;

        public static void Refresh()
        {
            if (Time.unscaledTime < _nextRefreshTime)
                return;

            _nextRefreshTime = Time.unscaledTime + 0.5f;

            RefreshTypes();
            RefreshPlayer();
        }

        private static void RefreshTypes()
        {
            PlayerType ??= ReflectionCache.GetType("PlayerManager");

            if (CoreGameManagerType == null)
            {
                CoreGameManagerType = ReflectionCache.GetType("CoreGameManager");
                _coreInstanceProp = ReflectionCache.GetSingletonInstanceProperty(CoreGameManagerType);
            }

            if (BaseGameManagerType == null)
            {
                BaseGameManagerType = ReflectionCache.GetType("BaseGameManager");
                _baseInstanceProp = ReflectionCache.GetSingletonInstanceProperty(BaseGameManagerType);
            }
        }

        private static void RefreshPlayer()
        {
            if (Player != null)
                return;

            if (PlayerType == null)
                return;

            Player = UnityEngine.Object.FindObjectOfType(PlayerType) as Component;
        }

        public static Component? GetPlayerComponent(string componentClassName)
        {
            if (Player == null)
                return null;

            Type? targetType = ReflectionCache.GetType(componentClassName);
            if (targetType == null)
                return null;

            return Player.GetComponent(targetType);
        }

        public static object? GetCoreGameManager()
        {
            Refresh();

            return ReflectionUtil.GetStaticProperty(_coreInstanceProp);
        }

        public static object? GetBaseGameManager()
        {
            Refresh();

            return ReflectionUtil.GetStaticProperty(_baseInstanceProp);
        }

        public static void StopGameTime(bool paused)
        {
            Time.timeScale = paused ? 0f : 1f;

            object? manager = GetCoreGameManager();
            if (manager == null)
                return;

            ReflectionUtil.SetField(manager, "paused", paused);
        }

        public static void Clear()
        {
            Player = null;
        }
    }
}