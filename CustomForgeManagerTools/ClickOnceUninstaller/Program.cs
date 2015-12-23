using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ClickOnceUninstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            // comment out to turn off CLI auto run
            args = new[] { "CustomsForge Manager" };

            Console.WindowWidth = 85;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            if (args.Length != 1 || string.IsNullOrEmpty(args[0]))
            {
                Console.WriteLine("Usage:  ClickOnceUninstaller appName");
                Console.WriteLine("        appName defaults to 'CustomsForge Manager' if not specified");
                Console.WriteLine();
                Console.WriteLine(@"Press any key to continue (Esc to exit) ...");
                Console.WriteLine();

                if (Console.ReadKey(true).Key == ConsoleKey.Escape)                    
                    Environment.Exit(0);

                args = new[] { "CustomsForge Manager" };
            }

            var appName = args[0];
            var uninstallInfo = UninstallInfo.Find(appName);

            if (uninstallInfo == null)
            {
                Console.WriteLine("Could not find application \"{0}\"", appName);

                // try direct removal methods
                // const string cfmX64App = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Ubisoft\Rocksmith2014";
                //const string cfmX64Uninstaller = @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 221680";

                List<string> cfmX86Reg = new List<string>();
                cfmX86Reg.Add(@"Software\Microsoft\Windows\CurrentVersion\Uninstall\73e8aceb7ff35be2");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Marks\cust..tion_8b363383a6af7bbc_0001.0000_3d3f4e2084c81522");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Marks\cust..tion_8b363383a6af7bbc_0001.0000_e308d9b1499a42d7");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Marks\cust..tion_8b363383a6af7bbc_0001.0000_none_70cf7b4cd905d8ce");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Marks\ionic.zip_edbe51ad942a3f5c_0001.0009_none_f629258aa0950b10");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Marks\newt..json_30ad4fe6b2a6aeed_0006.0000_none_92faee359ba05d0c");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Marks\zlib.net_47d7877cb3620160_0001.0000_none_755f576146efa063");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\PackageMetadata\{2ec93463-b0c3-45e1-8364-327e96aea856}_{3f471841-eef2-47d6-89c0-d028f03a4ad5}");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\StateManager\Applications\cust..tion_8b363383a6af7bbc_0001.0000_3d3f4e2084c81522");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\StateManager\Families\F_cust..tion_8b363383a6af7bbc_d7dab6a9e12184c3");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\StateManager\Families\Gi_cust..tion_8b363383a6af7bbc_332e0de7d8c59c9e");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Visibility\cust...exe_8b363383a6af7bbc_0001.0000_none_4e9ab6e058096911");
                cfmX86Reg.Add(@"Software\Classes\Software\Microsoft\Windows\CurrentVersion\Deployment\SideBySide\2.0\Visibility\cust..tion_8b363383a6af7bbc_0001.0000_none_70cf7b4cd905d8ce");

                bool foundInRegistry = false;

                foreach (var cfmRegKey in cfmX86Reg)
                {
                    using (RegistryKey key = Registry.CurrentUser.OpenSubKey(cfmRegKey))
                        if (key != null)
                        {
                            Registry.CurrentUser.DeleteSubKeyTree(cfmRegKey);
                            Console.WriteLine("Removed \"{0}\" from registry", cfmRegKey);
                            foundInRegistry = true;
                        }
                }

                if (foundInRegistry)
                    Console.WriteLine("Finished removing application \"{0}\" remnants from registry", appName);
            }
            else
            {
                Console.WriteLine("Uninstalling application \"{0}\"", appName);
                var uninstaller = new Uninstaller();
                uninstaller.Uninstall(uninstallInfo);

                Console.WriteLine("ClickOnce uninstaller completed sucessfully.");
            }

            // try to remove CFM folders
            if (Directory.Exists(Path.Combine(Environment.SpecialFolder.Programs.ToString(), "CustomsForge.com")))
            {
                Directory.Delete(Path.Combine(Environment.SpecialFolder.Programs.ToString(), "CustomsForge.com"), true);
                Console.WriteLine("Deleted CustomsForge.com folder from Start Programs menu.");
            }

            if (Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CFM")))
            {
                Directory.Delete((Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CFM")), true);
                Console.WriteLine("Deleted CFM folder from My Documents.");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue ...");
            // commented out for unattended auto run    
            //Console.ReadLine();
            MessageBox.Show("CFSM Uninstaller Finished ...", "Clean Uninstall ...", MessageBoxButtons.OK);

        }

    }
}
