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
