using Aequor.Model;
using Aequor.Resources;
using Aequor.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Manager
{

    internal class SentenceManager
    {

        private readonly Dictionary<int, Sentence> sentencesById = new();
        private readonly Dictionary<int, List<Sentence>> sentencesByCategory = new();

        public void Load()
        {
            sentencesById.Clear();
            sentencesByCategory.Clear();
            byte[][] resources = { Resource.a, Resource.b, Resource.c, Resource.d, Resource.e, Resource.f, Resource.g, Resource.h, Resource.i, Resource.j, Resource.k, Resource.l };
            string[] category = { "动画", "漫画", "游戏", "文学", "原创", "网络", "其他", "影视", "诗词", "网易云", "哲学", "抖机灵" };
            for (int i = 0; i <= resources.Length; i++)
            {
                sentencesByCategory[i] = new();
            }
            for (int i = 0; i < resources.Length; i++)
            {
                JArray root = JArray.Parse(Encoding.UTF8.GetString(resources[i]));
                foreach (JObject sentenceRoot in root)
                {
                    Sentence sentence = new(sentenceRoot["id"]!.Value<int>(), sentenceRoot["hitokoto"]!.Value<string>()!, category[i], sentenceRoot["from"]!.Value<string>() ?? "未知来源", sentenceRoot["from_who"]!.Value<string>() ?? "未知作者");
                    sentencesById[sentenceRoot["id"]!.Value<int>()] = sentence;
                    sentencesByCategory[0].Add(sentence);
                    sentencesByCategory[i + 1].Add(sentence);
                }
            }
            sentencesByCategory[0].Sort((s1, s2) => s1.id.CompareTo(s2.id));
        }

        public Sentence GetRandomSentence(int category) => sentencesByCategory[category][MathUtil.RandomNumber(GetSentenceCount(category))];

        public Sentence GetSentenceByIndex(int category, int index) => sentencesByCategory[category][index];

        public Sentence? GetSentenceById(int id) => sentencesById.ContainsKey(id) ? sentencesById[id] : null;

        public int GetSentenceIndex(int category, Sentence sentence) => sentencesByCategory[category].IndexOf(sentence);

        public int GetSentenceCount(int category) => sentencesByCategory[category].Count;

    }

}
