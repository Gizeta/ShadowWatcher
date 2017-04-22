using System;
using System.Reflection;

namespace ShadowWatcher.Helper
{
    public static class ReflectHelper
    {
        private const BindingFlags flags = BindingFlags.Public |
                                           BindingFlags.NonPublic |
                                           BindingFlags.Static |
                                           BindingFlags.Instance;

        public static FieldInfo GetField<T>(this T obj, string name)
            where T : class
        {
            return typeof(T).GetField(name, flags);
        }

        public static PropertyInfo GetProperty<T>(this T obj, string name)
            where T : class
        {
            return typeof(T).GetProperty(name, flags);
        }

        public static MethodInfo GetMethod<T>(this T obj, string name)
            where T : class
        {
            return typeof(T).GetMethod(name, flags | BindingFlags.OptionalParamBinding);
        }
    }
}
