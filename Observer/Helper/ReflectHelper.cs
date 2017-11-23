// Copyright 2017 Gizeta
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

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

        public static T GetField<T>(this Type type, string name, object target)
        {
            return (T)type.GetField(name, flags).GetValue(target);
        }
        public static void SetField(this Type type, string name, object target, object value)
        {
            type.GetField(name, flags).SetValue(target, value);
        }
        public static T GetField<T>(this Object obj, string name)
        {
            return (T)obj.GetType().GetField<T>(name, obj);
        }
        public static void SetField(this Object obj, string name, object value)
        {
            obj.GetType().SetField(name, obj, value);
        }

        public static T GetProperty<T>(this Type type, string name, object target)
        {
            return (T)type.GetProperty(name, flags).GetValue(target, null);
        }
        public static void SetProperty(this Type type, string name, object target, object value)
        {
            type.GetProperty(name, flags).SetValue(target, value, null);
        }
        public static T GetProperty<T>(this Object obj, string name)
        {
            return (T)obj.GetType().GetProperty<T>(name, obj);
        }
        public static void SetProperty(this Object obj, string name, object value)
        {
            obj.GetType().SetProperty(name, obj, value);
        }

        public static void InvokeMethod(this Type type, string name, object target, params object[] param)
        {
            type.GetMethod(name, flags | BindingFlags.OptionalParamBinding).Invoke(target, param);
        }
        public static T InvokeMethod<T>(this Type type, string name, object target, params object[] param)
        {
            return (T)type.GetMethod(name, flags | BindingFlags.OptionalParamBinding).Invoke(target, param);
        }
        public static void InvokeMethod(this Object obj, string name, params object[] param)
        {
            obj.GetType().InvokeMethod(name, obj, param);
        }
        public static T InvokeMethod<T>(this Object obj, string name, params object[] param)
        {
            return obj.GetType().InvokeMethod<T>(name, obj, param);
        }
    }
}
