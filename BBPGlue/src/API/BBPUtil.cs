using System;
using System.Reflection;
using BBPGlue.Core;

namespace BBPGlue.API {
    /// <summary>
    /// Utility helpers used by the API for type checks and miscellaneous helpers.
    /// </summary>
    public sealed class BBPUtil {
        /// <summary>
        /// Returns true if the provided object's runtime type is assignable to the named type.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <param name="className">The fully-qualified class name to check against.</param>
        /// <returns>True if obj is an instance of, or derives from, the named type; otherwise false.</returns>
        public static bool IsA(object obj, string className)
        {
            Type? type = ReflectionCache.GetType(className);
            return type != null && type.IsAssignableFrom(obj.GetType());
        }
    }
}