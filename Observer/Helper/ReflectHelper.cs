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
