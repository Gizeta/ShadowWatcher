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
using System.Runtime.InteropServices;

namespace ShadowWatcher
{
    public static class Injector
    {
        [DllImport("SVInjector.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Inject(
            [MarshalAs(UnmanagedType.LPStr)]string dllName,
            [MarshalAs(UnmanagedType.LPStr)]string methodName);

        private static string dll = $"{AppDomain.CurrentDomain.BaseDirectory}Observer.dll";

        public static int Attach() => Inject(dll, "Load");

        public static int Detach() => Inject(dll, "Unload");
    }
}
