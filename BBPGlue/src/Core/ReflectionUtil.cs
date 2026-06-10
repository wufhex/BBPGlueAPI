using System;
using System.Reflection;

namespace BBPGlue.Core
{
    public static class ReflectionUtil
    {
        private const BindingFlags Flags =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.Static;

    public static object? CreateInstance(string className, params object[] args)
    {
        Type? type = ReflectionCache.GetType(className);

        if (type == null)
            return null;

        try
        {
            return Activator.CreateInstance(type, args);
        }
        catch
        {
            return null;
        }
    }

        public static T? GetField<T>(object? instance, string fieldName)
        {
            if (instance == null)
                return default;

            try
            {
                FieldInfo? field = instance.GetType().GetField(fieldName, Flags);
                object? value = field?.GetValue(instance);

                if (value is T typed)
                    return typed;
            }
            catch { }

            return default;
        }

        public static bool SetField(object? instance, string fieldName, object? value)
        {
            if (instance == null)
                return false;

            try
            {
                FieldInfo? field = instance.GetType().GetField(fieldName, Flags);

                if (field == null)
                    return false;

                field.SetValue(instance, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static T? GetProperty<T>(object? instance, string propertyName)
        {
            if (instance == null)
                return default;

            try
            {
                PropertyInfo? property = instance.GetType().GetProperty(propertyName, Flags);
                object? value = property?.GetValue(instance, null);

                if (value is T typed)
                    return typed;
            }
            catch { }

            return default;
        }

        public static bool SetProperty(object? instance, string propertyName, object? value)
        {
            if (instance == null)
                return false;

            try
            {
                PropertyInfo? property = instance.GetType().GetProperty(propertyName, Flags);

                if (property == null || !property.CanWrite)
                    return false;

                property.SetValue(instance, value, null);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static object? GetStaticProperty(PropertyInfo? property)
        {
            if (property == null)
                return null;

            try
            {
                return property.GetValue(null, null);
            }
            catch
            {
                return null;
            }
        }

        public static object? GetSingletonInstance(string className)
        {
            Type? targetType = ReflectionCache.GetType(className);
            Type? genericSingleton = ReflectionCache.GetType("Singleton`1");

            if (targetType == null || genericSingleton == null)
                return null;

            Type closedType = genericSingleton.MakeGenericType(targetType);

            PropertyInfo? instance = closedType.GetProperty(
                "Instance",
                BindingFlags.Public | BindingFlags.Static
            );

            return GetStaticProperty(instance);
        }

        public static object? GetEnumValue(string enumName, string valueName)
        {
            Type? enumType = ReflectionCache.GetType(enumName);

            if (enumType == null || !enumType.IsEnum)
                return null;

            try
            {
                return Enum.Parse(enumType, valueName);
            }
            catch
            {
                return null;
            }
        }

        public static object? Call(object? instance, string methodName, params object[] args)
        {
            if (instance == null)
                return null;

            try
            {
                MethodInfo? method = FindMethod(instance.GetType(), methodName, args);

                return method?.Invoke(instance, args);
            }
            catch
            {
                return null;
            }
        }

        public static T? Call<T>(object? instance, string methodName, params object[] args)
        {
            object? result = Call(instance, methodName, args);

            if (result is T typed)
                return typed;

            return default;
        }

        public static object? CallStatic(string className, string methodName, params object[] args)
        {
            Type? type = ReflectionCache.GetType(className);

            if (type == null)
                return null;

            try
            {
                MethodInfo? method = FindMethod(type, methodName, args);
                return method?.Invoke(null, args);
            }
            catch
            {
                return null;
            }
        }

        private static MethodInfo? FindMethod(Type type, string methodName, object[] args)
        {
            foreach (MethodInfo method in type.GetMethods(Flags))
            {
                if (method.Name != methodName)
                    continue;

                ParameterInfo[] parameters = method.GetParameters();

                if (parameters.Length != args.Length)
                    continue;

                bool compatible = true;

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (args[i] == null)
                        continue;

                    if (!parameters[i].ParameterType.IsInstanceOfType(args[i]))
                    {
                        compatible = false;
                        break;
                    }
                }

                if (compatible)
                    return method;
            }

            return null;
        }
    }
}