using System;
using System.Collections.Generic;
using System.Reflection;

namespace BBPGlue.Core
{
    public static class ReflectionCache
    {
        private const string GameAssembly = "Assembly-CSharp";

        private static readonly Dictionary<string, Type?> Types = new Dictionary<string, Type?>();
        private static Type? _genericSingletonType;

        public static Type? GetType(string className)
        {
            if (Types.TryGetValue(className, out Type? cached))
                return cached;

            Type? type = Type.GetType($"{className}, {GameAssembly}");
            Types[className] = type;

            return type;
        }

        public static PropertyInfo? GetSingletonInstanceProperty(Type? targetType)
        {
            if (targetType == null)
                return null;

            _genericSingletonType ??= Type.GetType($"Singleton`1, {GameAssembly}");

            if (_genericSingletonType == null)
                return null;

            Type closedType = _genericSingletonType.MakeGenericType(targetType);

            return closedType.GetProperty(
                "Instance",
                BindingFlags.Public | BindingFlags.Static
            );
        }
    }
}