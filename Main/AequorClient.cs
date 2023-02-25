using Aequor.Manager;
using Aequor.Model;
using Aequor.Resources;
using Aequor.UI;
using Aequor.Util;
using CefSharp;
using CefSharp.Wpf;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Aequor.Main
{

    internal class AequorClient
    {

        public static readonly AequorClient INSTANCE = new();

        public SentenceManager sentenceManager;
        public ConfigManager configManager;

        public ShanBay shanBay;

        private AequorClient() { }

        private void InitDirectory()
        {
            if (!Directory.Exists(FileUtil.DATA_PATH))
            {
                Directory.CreateDirectory(FileUtil.DATA_PATH);
            }
            if (!Directory.Exists(FileUtil.BROWSER_DATA_PATH))
            {
                Directory.CreateDirectory(FileUtil.BROWSER_DATA_PATH);
            }
        }

        private void InitCef()
        {
            CefSettings settings = new()
            {
                CachePath = FileUtil.BROWSER_DATA_PATH
            };
            Cef.Initialize(settings);
        }

        public void Init()
        {
            InitDirectory();
            InitCef();
            sentenceManager = new SentenceManager();
            sentenceManager.Load();
            configManager = new ConfigManager();
            configManager.Load();
            shanBay = new ShanBay();
            if (configManager.forceStudy && !ShanBayUtil.IsStudyFinished())
            {
                shanBay.Lock();
                shanBay.Show();
            }
        }

    }

}
