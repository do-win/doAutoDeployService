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

        public static string ScripteFile = "Scripts" + Path.DirectorySeparatorChar + "build.bat";

        ////////////////////////////////////////////////////////////
        public const string ConfigFile = "ConfigFile/Qiniu.config";

        
    }
}