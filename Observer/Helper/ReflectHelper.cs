using System;
using System.Reflection;

namespace ShadowWatcher.Helper
{
    public static class ReflectHelper
    {
        public static FieldInfo GetField<T>(this T obj, string name)
            where T : class
        {
            Type t = typeof(T);
            return t.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public static MethodInfo GetMethod<T>(this T obj, string name)
            where T : class
        {
            Type t = typeof(T);
            return t.GetMethod(name, BindingFlags.Instance | BindingFlags.OptionalParamBinding | BindingFlags.NonPublic);
        }
    }
}
