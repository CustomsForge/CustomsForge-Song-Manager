// code adapted from www.csharp411.com, github AVERT, stackoverflow, et.al.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GenTools
{
    public static class SysExtensions
    {
        private const string MESSAGEBOX_CAPTION = "System Extensions";

        /// <summary>
        /// Provides detailed information about the host operating system.
        /// </summary>

        #region PINVOKE

        #region GET

        #region PRODUCT INFO
        [DllImport("Kernel32.dll")]
        internal static extern bool GetProductInfo(int osMajorVersion, int osMinorVersion, int spMajorVersion, int spMinorVersion, out int edition);

        #endregion PRODUCT INFO

        #region VERSION

        [DllImport("kernel32.dll")]
        private static extern bool GetVersionEx(ref OSVERSIONINFOEX osVersionInfo);

        #endregion VERSION

        #endregion GET

        #region OSVERSIONINFOEX

        [StructLayout(LayoutKind.Sequential)]
        private struct OSVERSIONINFOEX
        {
            public int dwOSVersionInfoSize;
            public int dwMajorVersion;
            public int dwMinorVersion;
            public int dwBuildNumber;
            public int dwPlatformId;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szCSDVersion;
            public short wServicePackMajor;
            public short wServicePackMinor;
            public short wSuiteMask;
            public byte wProductType;
            public byte wReserved;
        }

        #endregion OSVERSIONINFOEX

        #region PRODUCT

        private const int PRODUCT_UNDEFINED = 0x00000000;
        private const int PRODUCT_ULTIMATE = 0x00000001;
        private const int PRODUCT_HOME_BASIC = 0x00000002;
        private const int PRODUCT_HOME_PREMIUM = 0x00000003;
        private const int PRODUCT_ENTERPRISE = 0x00000004;
        private const int PRODUCT_HOME_BASIC_N = 0x00000005;
        private const int PRODUCT_BUSINESS = 0x00000006;
        private const int PRODUCT_STANDARD_SERVER = 0x00000007;
        private const int PRODUCT_DATACENTER_SERVER = 0x00000008;
        private const int PRODUCT_SMALLBUSINESS_SERVER = 0x00000009;
        private const int PRODUCT_ENTERPRISE_SERVER = 0x0000000A;
        private const int PRODUCT_STARTER = 0x0000000B;
        private const int PRODUCT_DATACENTER_SERVER_CORE = 0x0000000C;
        private const int PRODUCT_STANDARD_SERVER_CORE = 0x0000000D;
        private const int PRODUCT_ENTERPRISE_SERVER_CORE = 0x0000000E;
        private const int PRODUCT_ENTERPRISE_SERVER_IA64 = 0x0000000F;
        private const int PRODUCT_BUSINESS_N = 0x00000010;
        private const int PRODUCT_WEB_SERVER = 0x00000011;
        private const int PRODUCT_CLUSTER_SERVER = 0x00000012;
        private const int PRODUCT_HOME_SERVER = 0x00000013;
        private const int PRODUCT_STORAGE_EXPRESS_SERVER = 0x00000014;
        private const int PRODUCT_STORAGE_STANDARD_SERVER = 0x00000015;
        private const int PRODUCT_STORAGE_WORKGROUP_SERVER = 0x00000016;
        private const int PRODUCT_STORAGE_ENTERPRISE_SERVER = 0x00000017;
        private const int PRODUCT_SERVER_FOR_SMALLBUSINESS = 0x00000018;
        private const int PRODUCT_SMALLBUSINESS_SERVER_PREMIUM = 0x00000019;
        private const int PRODUCT_HOME_PREMIUM_N = 0x0000001A;
        private const int PRODUCT_ENTERPRISE_N = 0x0000001B;
        private const int PRODUCT_ULTIMATE_N = 0x0000001C;
        private const int PRODUCT_WEB_SERVER_CORE = 0x0000001D;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT = 0x0000001E;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY = 0x0000001F;
        private const int PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING = 0x00000020;
        private const int PRODUCT_SERVER_FOR_SMALLBUSINESS_V = 0x00000023;
        private const int PRODUCT_STANDARD_SERVER_V = 0x00000024;
        private const int PRODUCT_ENTERPRISE_SERVER_V = 0x00000026;
        private const int PRODUCT_STANDARD_SERVER_CORE_V = 0x00000028;
        private const int PRODUCT_ENTERPRISE_SERVER_CORE_V = 0x00000029;
        private const int PRODUCT_PROFESSIONAL = 0x00000030;
        private const int PRODUCT_HYPERV = 0x0000002A;

        #endregion PRODUCT

        #region VERSIONS

        private const int VER_NT_WORKSTATION = 1;
        private const int VER_NT_DOMAIN_CONTROLLER = 2;
        private const int VER_NT_SERVER = 3;
        private const int VER_SUITE_SMALLBUSINESS = 1;
        private const int VER_SUITE_ENTERPRISE = 2;
        private const int VER_SUITE_TERMINAL = 16;
        private const int VER_SUITE_DATACENTER = 128;
        private const int VER_SUITE_SINGLEUSERTS = 256;
        private const int VER_SUITE_PERSONAL = 512;
        private const int VER_SUITE_BLADE = 1024;

        #endregion VERSIONS

        #endregion PINVOKE

        #region BITS (32 or 64 bit OS)

        /// <summary>
        /// Determines if the current application is 32 or 64-bit.
        /// </summary>
        public static int Bits
        {
            get { return IntPtr.Size * 8; }
        }

        #endregion BITS

        #region EDITION

        private static string s_Edition;

        /// <summary>
        /// Gets the edition of the operating system running on this computer.
        /// </summary>
        public static string Edition
        {
            get
            {
                if (s_Edition != null)
                    return s_Edition; //***** RETURN *****//

                string edition = String.Empty;

                OperatingSystem osVersion = Environment.OSVersion;
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (GetVersionEx(ref osVersionInfo))
                {
                    int majorVersion = osVersion.Version.Major;
                    int minorVersion = osVersion.Version.Minor;
                    byte productType = osVersionInfo.wProductType;
                    short suiteMask = osVersionInfo.wSuiteMask;

                    #region VERSION 4

                    if (majorVersion == 4)
                    {
                        if (productType == VER_NT_WORKSTATION)
                        {
                            // Windows NT 4.0 Workstation
                            edition = "Workstation";
                        }
                        else if (productType == VER_NT_SERVER)
                        {
                            if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
                            {
                                // Windows NT 4.0 Server Enterprise
                                edition = "Enterprise Server";
                            }
                            else
                            {
                                // Windows NT 4.0 Server
                                edition = "Standard Server";
                            }
                        }
                    }
                    #endregion VERSION 4

                    #region VERSION 5

                    else if (majorVersion == 5)
                    {
                        if (productType == VER_NT_WORKSTATION)
                        {
                            if ((suiteMask & VER_SUITE_PERSONAL) != 0)
                            {
                                // Windows XP Home Edition
                                edition = "Home";
                            }
                            else
                            {
                                // Windows XP / Windows 2000 Professional
                                edition = "Professional";
                            }
                        }
                        else if (productType == VER_NT_SERVER)
                        {
                            if (minorVersion == 0)
                            {
                                if ((suiteMask & VER_SUITE_DATACENTER) != 0)
                                {
                                    // Windows 2000 Datacenter Server
                                    edition = "Datacenter Server";
                                }
                                else if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
                                {
                                    // Windows 2000 Advanced Server
                                    edition = "Advanced Server";
                                }
                                else
                                {
                                    // Windows 2000 Server
                                    edition = "Server";
                                }
                            }
                            else
                            {
                                if ((suiteMask & VER_SUITE_DATACENTER) != 0)
                                {
                                    // Windows Server 2003 Datacenter Edition
                                    edition = "Datacenter";
                                }
                                else if ((suiteMask & VER_SUITE_ENTERPRISE) != 0)
                                {
                                    // Windows Server 2003 Enterprise Edition
                                    edition = "Enterprise";
                                }
                                else if ((suiteMask & VER_SUITE_BLADE) != 0)
                                {
                                    // Windows Server 2003 Web Edition
                                    edition = "Web Edition";
                                }
                                else
                                {
                                    // Windows Server 2003 Standard Edition
                                    edition = "Standard";
                                }
                            }
                        }
                    }
                    #endregion VERSION 5

                    #region VERSION 6

                    else if (majorVersion == 6)
                    {
                        int ed;
                        if (GetProductInfo(majorVersion, minorVersion, osVersionInfo.wServicePackMajor, osVersionInfo.wServicePackMinor, out ed))
                        {
                            switch (ed)
                            {
                                case PRODUCT_BUSINESS:
                                    edition = "Business";
                                    break;
                                case PRODUCT_BUSINESS_N:
                                    edition = "Business N";
                                    break;
                                case PRODUCT_CLUSTER_SERVER:
                                    edition = "HPC Edition";
                                    break;
                                case PRODUCT_DATACENTER_SERVER:
                                    edition = "Datacenter Server";
                                    break;
                                case PRODUCT_DATACENTER_SERVER_CORE:
                                    edition = "Datacenter Server (core installation)";
                                    break;
                                case PRODUCT_ENTERPRISE:
                                    edition = "Enterprise";
                                    break;
                                case PRODUCT_ENTERPRISE_N:
                                    edition = "Enterprise N";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER:
                                    edition = "Enterprise Server";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_CORE:
                                    edition = "Enterprise Server (core installation)";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_CORE_V:
                                    edition = "Enterprise Server without Hyper-V (core installation)";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_IA64:
                                    edition = "Enterprise Server for Itanium-based Systems";
                                    break;
                                case PRODUCT_ENTERPRISE_SERVER_V:
                                    edition = "Enterprise Server without Hyper-V";
                                    break;
                                case PRODUCT_HOME_BASIC:
                                    edition = "Home Basic";
                                    break;
                                case PRODUCT_HOME_BASIC_N:
                                    edition = "Home Basic N";
                                    break;
                                case PRODUCT_HOME_PREMIUM:
                                    edition = "Home Premium";
                                    break;
                                case PRODUCT_HOME_PREMIUM_N:
                                    edition = "Home Premium N";
                                    break;
                                case PRODUCT_HYPERV:
                                    edition = "Microsoft Hyper-V Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_MANAGEMENT:
                                    edition = "Windows Essential Business Management Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_MESSAGING:
                                    edition = "Windows Essential Business Messaging Server";
                                    break;
                                case PRODUCT_MEDIUMBUSINESS_SERVER_SECURITY:
                                    edition = "Windows Essential Business Security Server";
                                    break;
                                case PRODUCT_SERVER_FOR_SMALLBUSINESS:
                                    edition = "Windows Essential Server Solutions";
                                    break;
                                case PRODUCT_SERVER_FOR_SMALLBUSINESS_V:
                                    edition = "Windows Essential Server Solutions without Hyper-V";
                                    break;
                                case PRODUCT_SMALLBUSINESS_SERVER:
                                    edition = "Windows Small Business Server";
                                    break;
                                case PRODUCT_STANDARD_SERVER:
                                    edition = "Standard Server";
                                    break;
                                case PRODUCT_STANDARD_SERVER_CORE:
                                    edition = "Standard Server (core installation)";
                                    break;
                                case PRODUCT_STANDARD_SERVER_CORE_V:
                                    edition = "Standard Server without Hyper-V (core installation)";
                                    break;
                                case PRODUCT_STANDARD_SERVER_V:
                                    edition = "Standard Server without Hyper-V";
                                    break;
                                case PRODUCT_STARTER:
                                    edition = "Starter";
                                    break;
                                case PRODUCT_STORAGE_ENTERPRISE_SERVER:
                                    edition = "Enterprise Storage Server";
                                    break;
                                case PRODUCT_STORAGE_EXPRESS_SERVER:
                                    edition = "Express Storage Server";
                                    break;
                                case PRODUCT_STORAGE_STANDARD_SERVER:
                                    edition = "Standard Storage Server";
                                    break;
                                case PRODUCT_STORAGE_WORKGROUP_SERVER:
                                    edition = "Workgroup Storage Server";
                                    break;
                                case PRODUCT_UNDEFINED:
                                    edition = "Unknown product";
                                    break;
                                case PRODUCT_ULTIMATE:
                                    edition = "Ultimate";
                                    break;
                                case PRODUCT_ULTIMATE_N:
                                    edition = "Ultimate N";
                                    break;
                                case PRODUCT_WEB_SERVER:
                                    edition = "Web Server";
                                    break;
                                case PRODUCT_WEB_SERVER_CORE:
                                    edition = "Web Server (core installation)";
                                    break;
                                case PRODUCT_PROFESSIONAL:
                                    edition = "Professional";
                                    break;
                            }
                        }
                    }

                    #endregion VERSION 6
                }

                s_Edition = edition;
                return edition;
            }
        }

        #endregion EDITION

        #region NAME (aka Product Type)

        private static string s_Name;

        /// <summary>
        /// Gets the name of the operating system running on this computer.
        /// </summary>
        public static string Name
        {
            get
            {
                if (s_Name != null)
                    return s_Name; //***** RETURN *****//

                string name = "unknown";

                OperatingSystem osVersion = Environment.OSVersion;
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();
                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (GetVersionEx(ref osVersionInfo))
                {
                    int majorVersion = osVersion.Version.Major;
                    int minorVersion = osVersion.Version.Minor;

                    switch (osVersion.Platform)
                    {
                        case PlatformID.MacOSX:
                            name = "Mac OSX";
                            break;
                        case PlatformID.Win32Windows:
                            {
                                if (majorVersion == 4)
                                {
                                    string csdVersion = osVersionInfo.szCSDVersion;
                                    switch (minorVersion)
                                    {
                                        case 0:
                                            if (csdVersion == "B" || csdVersion == "C")
                                                name = "Windows 95 OSR2";
                                            else
                                                name = "Windows 95";
                                            break;
                                        case 10:
                                            if (csdVersion == "A")
                                                name = "Windows 98 Second Edition";
                                            else
                                                name = "Windows 98";
                                            break;
                                        case 90:
                                            name = "Windows Me";
                                            break;
                                    }
                                }
                                break;
                            }

                        case PlatformID.Win32NT:
                            {
                                byte productType = osVersionInfo.wProductType;

                                switch (majorVersion)
                                {
                                    case 3:
                                        name = "Windows NT 3.51";
                                        break;
                                    case 4:
                                        switch (productType)
                                        {
                                            case 1:
                                                name = "Windows NT 4.0";
                                                break;
                                            case 3:
                                                name = "Windows NT 4.0 Server";
                                                break;
                                        }
                                        break;
                                    case 5:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                name = "Windows 2000";
                                                break;
                                            case 1:
                                                name = "Windows XP";
                                                break;
                                            case 2:
                                                if (productType == VER_NT_WORKSTATION)
                                                    name = "Windows XP"; // handles XP 64-bit
                                                else
                                                    name = "Windows Server 2003";
                                                break;
                                        }
                                        break;
                                    case 6:
                                        switch (minorVersion)
                                        {
                                            case 0:
                                                name = "Windows Vista";
                                                break;
                                            case 1:
                                                name = "Windows 7";
                                                break;
                                            case 2:
                                                name = "Windows 8";
                                                break;
                                            case 3:
                                                name = "Windows Server 2008";
                                                break;
                                        }
                                        break;

                                    //switch (productType)
                                    //{
                                    //    case 1:
                                    //        name = "Windows Vista";
                                    //        break;
                                    //    case 3:
                                    //        name = "Windows Server 2008";
                                    //        break;
                                    //}
                                    //break;
                                }
                                break;
                            }
                    }
                }

                s_Name = name;
                return name;
            }
        }

        #endregion NAME

        #region SERVICE PACK

        /// <summary>
        /// Gets the service pack information of the operating system running on this computer.
        /// </summary>
        public static string ServicePack
        {
            get
            {
                string servicePack = String.Empty;
                OSVERSIONINFOEX osVersionInfo = new OSVERSIONINFOEX();

                osVersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(OSVERSIONINFOEX));

                if (GetVersionEx(ref osVersionInfo))
                {
                    servicePack = osVersionInfo.szCSDVersion;
                }

                return servicePack;
            }
        }

        #endregion SERVICE PACK

        #region VERSION

        #region BUILD

        /// <summary>
        /// Gets the build version number of the operating system running on this computer.
        /// </summary>
        public static int BuildVersion
        {
            get { return Environment.OSVersion.Version.Build; }
        }

        #endregion BUILD

        #region FULL

        #region STRING

        /// <summary>
        /// Gets the full version string of the operating system running on this computer.
        /// </summary>
        public static string VersionString
        {
            get { return Environment.OSVersion.Version.ToString(); }
        }

        #endregion STRING

        #region VERSION

        /// <summary>
        /// Gets the full version of the operating system running on this computer.
        /// </summary>
        public static Version Version
        {
            get { return Environment.OSVersion.Version; }
        }

        #endregion VERSION

        #endregion FULL

        #region MAJOR

        /// <summary>
        /// Gets the major version number of the operating system running on this computer.
        /// </summary>
        public static int MajorVersion
        {
            get { return Environment.OSVersion.Version.Major; }
        }

        #endregion MAJOR

        #region MINOR

        /// <summary>
        /// Gets the minor version number of the operating system running on this computer.
        /// </summary>
        public static int MinorVersion
        {
            get { return Environment.OSVersion.Version.Minor; }
        }

        #endregion MINOR

        #region REVISION

        /// <summary>
        /// Gets the revision version number of the operating system running on this computer.
        /// </summary>
        public static int RevisionVersion
        {
            get { return Environment.OSVersion.Version.Revision; }
        }

        #endregion REVISION

        #endregion VERSION

        #region .NET VERSION

        /// <summary>
        /// Gets the .Net version number that is actively running
        /// </summary>
        public static string DotNetVersion
        {
            get { return Environment.Version.ToString(); }
        }

        /// <summary>
        /// Checks if .Net 4.0.30319 is installed
        /// </summary>
        /// <param name="conditionalCheckResult">[Optional] Result of some preliminary conditional check</param>
        /// <returns></returns>
        public static bool IsDotNet4(bool conditionalCheckResult = true)
        {
            if (!DotNetVersion.Contains("4.0.30319") && conditionalCheckResult)
            {
                var envMsg = "This application runs best with .NET 4.0.30319 installed." + Environment.NewLine +
                    "You are currently running .NET " + DotNetVersion + Environment.NewLine +
                    "Install the correct version if you experinece problems running this application.   " + Environment.NewLine + Environment.NewLine +
                    "Click 'Yes' to download and install the correct version now from:" + Environment.NewLine +
                    "https://www.microsoft.com/en-us/download/confirmation.aspx?id=17718";

                if (MessageBox.Show(envMsg, "Incorrect .NET Version ...", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    Process.Start("https://www.microsoft.com/en-us/download/confirmation.aspx?id=17718");
                    Thread.Sleep(500);
                    Process.Start("https://www.howtogeek.com/118869/how-to-easily-install-previous-versions-of-the-.net-framework-in-windows-8");
                    Environment.Exit(0);
                }

                return false;
            }

            return true;
        }

        #endregion

        #region MAC
        public static bool OnMac(string rs2DlcFolder = "")
        {
            if (rs2DlcFolder.Contains("Users") && rs2DlcFolder.Contains("Library"))
                return true;

            if (rs2DlcFolder.Contains("Application Support")) // hack work around
                return true;

            // TODO: for Wine compatiblity make all paths absolute, i.e. do not use '@"/'
            //if (File.Exists(@"/System/Library/CoreServices/SystemVersion.plist") || Directory.Exists("@/Users"))
            if (Directory.Exists("/Applications") & Directory.Exists("/System") &
                Directory.Exists("/Users") & Directory.Exists("/Volumes"))
                return true;

            return false;
        }
        #endregion

        #region ADMIN RIGHTS

        /// <summary>
        /// Determine if current user has admin privileges
        /// </summary>
        public static bool IsAdmin
        {
            get
            {
                var identity = WindowsIdentity.GetCurrent();
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        #endregion

        #region Windows Version

        /// <summary>
        /// Determines Windows Version
        /// </summary>
        public static string GetWindowsVersion()
        {
            string operatingSystem = string.Empty;

            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;

            if (os.Platform == PlatformID.Win32Windows)
            {
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "2000";
                        else
                            operatingSystem = "XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Windows Vista";
                        else if (vs.Minor == 1)
                            operatingSystem = "Windows 7";
                        else if (vs.Minor == 2)
                            operatingSystem = "Windows 8";
                        else if (vs.Minor == 3)
                            operatingSystem = "Windows 8.1";
                        break;
                    default:
                        break;
                }
            }

            return operatingSystem;
        }

        #endregion

        #region System Specs

        /// <summary>
        /// Determines System Specs
        /// </summary>
        public static string GetProcessorName()
        {
            string name = string.Empty;
            using (ManagementObjectSearcher mos = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor"))
            {
                foreach (ManagementObject mo in mos.Get())
                    name = mo["Name"].ToString();
            }

            return name;
        }

        public static string GetRAM()
        {
            string ram = string.Empty;
            Int64 size = Convert.ToInt64(new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory);

            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (size == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(size);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 0);
            ram = (Math.Sign(size) * num).ToString() + suf[place];

            return ram;
        }


        #endregion

        #region Web Browser
        /// <summary>
        /// Determine current browser
        /// </summary>
        private static WebBrowsers _WebBrowsers;

        static SysExtensions()
        {
            _WebBrowsers = new WebBrowsers().GetWebBrowsers();
        }

        public static string GetInternetExplorerVersion()
        {
            return _WebBrowsers.SingleOrDefault(x => x.Name == WebBrowser.WebBrowserName.InternetExplorer).Version;
        }

        public static string GetChromeVersion()
        {
            return _WebBrowsers.SingleOrDefault(x => x.Name == WebBrowser.WebBrowserName.Chrome).Version;
        }

        public static string GetFirefoxVersion()
        {
            return _WebBrowsers.SingleOrDefault(x => x.Name == WebBrowser.WebBrowserName.FireFox).Version;
        }

        public static string GetOperaVersion()
        {
            return _WebBrowsers.SingleOrDefault(x => x.Name == WebBrowser.WebBrowserName.Opera).Version;
        }

        public static string GetSafariVersion()
        {
            return _WebBrowsers.SingleOrDefault(x => x.Name == WebBrowser.WebBrowserName.Safari).Version;
        }

        public static double GetBrowserVersion(string browserVers)
        {
            var tablicaVers = browserVers.Split(new char[] { '.' });
            if (tablicaVers[1] == null)
                tablicaVers[1] = "0";

            return Convert.ToDouble(tablicaVers[0]) + Convert.ToDouble(tablicaVers[1]) / 10;
        }
    }

        #endregion

    #region Web Browser Support Classes

    public class WebBrowsers : List<WebBrowser>
    {
        public WebBrowsers GetWebBrowsers()
        {
            WebBrowsers browsers = new WebBrowsers();

            browsers.Add(new WebBrowser() { Name = WebBrowser.WebBrowserName.InternetExplorer });
            browsers.Add(new WebBrowser() { Name = WebBrowser.WebBrowserName.Chrome });
            browsers.Add(new WebBrowser() { Name = WebBrowser.WebBrowserName.FireFox });
            browsers.Add(new WebBrowser() { Name = WebBrowser.WebBrowserName.Opera });
            browsers.Add(new WebBrowser() { Name = WebBrowser.WebBrowserName.Safari });

            return browsers;
        }
    }

    public class WebBrowser
    {
        public enum WebBrowserName
        {
            InternetExplorer,
            Chrome,
            FireFox,
            Safari,
            Opera
        }

        public WebBrowserName Name { get; set; }

        public string Version
        {
            get { return GetVersion(this.Name); }
        }

        private static string GetVersion(WebBrowserName webBrowserName)
        {
            object version = null;

            switch (webBrowserName)
            {
                case WebBrowserName.InternetExplorer:
                    string ieSubKeyName = @"Software\Microsoft\Internet Explorer";
                    version = Registry.LocalMachine.OpenSubKey(ieSubKeyName).GetValue("svcVersion"); //IE 10 and above uses this key
                    if (version == null)
                        version = Registry.LocalMachine.OpenSubKey(ieSubKeyName).GetValue("Version");
                    break;
                case WebBrowserName.FireFox:
                    object ffPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe", "", null);

                    if (ffPath != null)
                        version = FileVersionInfo.GetVersionInfo(ffPath.ToString()).FileVersion;
                    break;
                case WebBrowserName.Chrome:
                    RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Google\Update\Clients");
                    if (key != null && key.OpenSubKey(key.GetSubKeyNames()[0]).GetValueNames().Contains("pv"))
                        version = key.OpenSubKey(key.GetSubKeyNames()[0]).GetValue("pv");
                    else
                    {
                        key = Registry.CurrentUser.OpenSubKey(@"Software\Google\Chrome\BLBeacon");
                        if (key != null)
                            version = key.GetValue("version");
                    }
                    break;
                case WebBrowserName.Opera:
                    object operaPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\opera.exe", "", null);

                    if (operaPath != null)
                        version = FileVersionInfo.GetVersionInfo(operaPath.ToString().Replace("\"", "")).FileVersion;

                    break;
                case WebBrowserName.Safari:

                    break;
            }

            if (version == null)
                return "N/A";
            else
                return version.ToString();
        }
    }

    #endregion

}


