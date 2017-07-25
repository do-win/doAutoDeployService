using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace doAutoDeployService.Utils
{
    public class Constants
    {

        public const string RootPath = "E:\\WebHome";

        public static string Temp = RootPath + Path.DirectorySeparatorChar + "Temp";

        public static string ShellFile = "Scripts" + Path.DirectorySeparatorChar + "build.sh";

        ////////////////////////////////////////////////////////////
        public const string ConfigFile = "ConfigFile/Qiniu.config";

        
    }
}