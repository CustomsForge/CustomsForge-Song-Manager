using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CustomsForgeManager.CustomsForgeManagerLib
{
    public class CrossPlatform
    {
        public static bool IsRunningOnMono()
        {
            return Type.GetType("Mono.Runtime") != null;
        }


#if WINDOWS
        public static IntPtr GetHandleFromProcessName(string s)
        {
            System.Diagnostics.Process proc = System.Diagnostics.Process.GetProcesses().Where(
                       p => Path.GetFileName(p.ProcessName) ==
                           Path.GetFileName(Path.GetFileNameWithoutExtension(s))).FirstOrDefault();
            if (proc == null)
                return IntPtr.Zero;
            return proc.MainWindowHandle;
        }


        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("USER32.DLL")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
#else
        public static IntPtr GetHandleFromProcessName(string s)
        {
            return IntPtr.Zero;
        }


        public static extern bool SetForegroundWindow(IntPtr hWnd){ return false;} 
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow) {return false;}
#endif
    }

#if WINDOWS


#else
    public class Mutex : IDisposable
    {
        public Mutex(bool initiallyOwned, string name)
        {

        }

        public bool WaitOne(int x,bool b)
        {
            return true;
        }

        public void Dispose()
        {
           
        }
    }
#endif

}
