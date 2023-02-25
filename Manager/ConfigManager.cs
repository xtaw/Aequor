using Aequor.Main;
using Aequor.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Manager
{

    internal class ConfigManager
    {

        public int sentenceCategory = 0;
        public bool forceStudy = false, restReminder = false;

        public void Save()
        {
            JObject root = new()
            {
                { "SentenceCategory", sentenceCategory },
                { "ForceStudy", forceStudy },
                { "RestReminder", restReminder }
            };
            File.WriteAllText(FileUtil.CONFIG_PATH, root.ToString());
        }

        public void Load()
        {
            if (!File.Exists(FileUtil.CONFIG_PATH))
            {
                Save();
                return;
            }
            JObject root = JObject.Parse(File.ReadAllText(FileUtil.CONFIG_PATH));
            sentenceCategory = root["SentenceCategory"]!.Value<int>();
            forceStudy = root["ForceStudy"]!.Value<bool>();
            restReminder = root["RestReminder"]!.Value<bool>();
        }

    }

}
