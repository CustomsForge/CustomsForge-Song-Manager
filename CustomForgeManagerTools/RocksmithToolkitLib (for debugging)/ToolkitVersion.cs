﻿using System;
using System.Reflection;
namespace RocksmithToolkitLib
{
    public static class ToolkitVersion
    {
        public static string commit = "c0d65deb[CFSM]";
        public static string version
        {
            get
            {
                return String.Format("{0}.{1}.{2}.{3}-{4}",
                    Assembly.GetExecutingAssembly().GetName().Version.Major,
                    Assembly.GetExecutingAssembly().GetName().Version.Minor,
                    Assembly.GetExecutingAssembly().GetName().Version.Build,
                    Assembly.GetExecutingAssembly().GetName().Version.Revision,
                    commit);
            }
        }
    }
}
