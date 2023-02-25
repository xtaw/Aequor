using IWshRuntimeLibrary;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Aequor.Util
{

    internal class StartupUtil
    {

        private StartupUtil() { }

        public static bool IsStartup()
        {
            return System.IO.File.Exists(FileUtil.STARTUP_PATH);
        }

        public static void EnableStartup()
        {
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(FileUtil.STARTUP_PATH);
            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.Save();
        }

        public static void DisableStartup()
        {
            System.IO.File.Delete(FileUtil.STARTUP_PATH);
        }

    }

}
