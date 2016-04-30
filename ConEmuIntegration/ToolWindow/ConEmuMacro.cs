//
// Copyright 2016 David Roller
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace ConEmuIntegration
{
    internal sealed class ConEmuMacro
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr LoadLibrary(string libname);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private delegate int FConsoleMain3(int anWorkMode, string asCommandLine);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        private delegate int FGuiMacro(string asWhere, string asMacro, out IntPtr bstrResult);

        private string m_LibraryPath;
        private IntPtr m_ConEmuHandle;
        private FConsoleMain3 m_fnConsoleMain3;
        private FGuiMacro m_fnGuiMacro;

        public ConEmuMacro(string asLibrary)
        {
            m_ConEmuHandle = IntPtr.Zero;
            m_fnConsoleMain3 = null;
            m_fnGuiMacro = null;
            m_LibraryPath = asLibrary;
            LoadConEmuDll(asLibrary);
        }

        ~ConEmuMacro()
        {
            UnloadConEmuDll();
        }

        private void ExecuteLegacy(string asWhere, string asMacro)
        {
            if (m_ConEmuHandle == IntPtr.Zero)
            {
                return;
            }

            string cmdLine = " -GuiMacro";
            if (string.IsNullOrEmpty(asWhere) == false)
            {
                cmdLine += ":" + asWhere;
            }
            else
            {
                cmdLine += " " + asMacro;
            }

            Environment.SetEnvironmentVariable("ConEmuMacroResult", null);
            m_fnConsoleMain3.Invoke(3, cmdLine);
        }

        private void ExecuteHelper(string asWhere, string asMacro)
        {
            if (m_fnGuiMacro != null)
            {
                IntPtr bstrPtr = IntPtr.Zero;
                int result = m_fnGuiMacro.Invoke(asWhere, asMacro, out bstrPtr);
                if(result != 133 /*CERR_GUIMACRO_SUCCEEDED*/ || result != 134 /*CERR_GUIMACRO_FAILED*/)
                {
                    return; // Sucess
                }

                if (bstrPtr != IntPtr.Zero)
                {
                    Marshal.FreeBSTR(bstrPtr);
                }
            }
            else
            {
                ExecuteLegacy(asWhere, asMacro);
            }
        }

        public void Execute(string asWhere, string asMacro)
        {
            if (m_ConEmuHandle == IntPtr.Zero)
            {
                return;
            }

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                ExecuteHelper(asWhere, asMacro);
            }).Start();
        }

        private void LoadConEmuDll(string asLibrary)
        {
            if (m_ConEmuHandle != IntPtr.Zero)
            {
                return;
            }

            m_ConEmuHandle = LoadLibrary(asLibrary);
            if (m_ConEmuHandle == IntPtr.Zero)
            {
                return;
            }
            
            const string fnNameOld = "ConsoleMain3";
            IntPtr ptrConsoleMain = GetProcAddress(m_ConEmuHandle, fnNameOld);

            const string fnNameNew = "GuiMacro";
            IntPtr ptrGuiMacro = GetProcAddress(m_ConEmuHandle, fnNameNew);

            if (ptrConsoleMain == IntPtr.Zero && ptrGuiMacro == IntPtr.Zero)
            {
                UnloadConEmuDll();
                return;
            }

            m_fnGuiMacro = (FGuiMacro)Marshal.GetDelegateForFunctionPointer(ptrGuiMacro, typeof(FGuiMacro));
            m_fnConsoleMain3 = (FConsoleMain3)Marshal.GetDelegateForFunctionPointer(ptrConsoleMain, typeof(FConsoleMain3));
        }

        private void UnloadConEmuDll()
        {
            if (m_ConEmuHandle != IntPtr.Zero)
            {
                FreeLibrary(m_ConEmuHandle);
                m_ConEmuHandle = IntPtr.Zero;
            }
        }
    }
}
