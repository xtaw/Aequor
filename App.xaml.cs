using Aequor.Main;
using Aequor.Util;
using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Aequor
{

    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            AequorClient.INSTANCE.Init();
        }

    }

}
