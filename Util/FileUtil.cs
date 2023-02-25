using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Util
{

    internal class FileUtil
    {

        public static readonly string STARTUP_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Aequor.lnk");

        public static readonly string DATA_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Aequor");
        public static readonly string BROWSER_DATA_PATH = Path.Combine(DATA_PATH, "BrowserData");
        public static readonly string CONFIG_PATH = Path.Combine(DATA_PATH, "Config.json");

        private FileUtil() { }

    }

}
