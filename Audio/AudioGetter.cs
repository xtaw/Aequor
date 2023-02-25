using Aequor.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aequor.Audio
{

    internal class AudioGetter
    {

        private static readonly Dictionary<string, byte[]> CACHE = new();

        public static byte[] GetAudioBytes(string url)
        {
            if (CACHE.ContainsKey(url))
            {
                return CACHE[url];
            }
            byte[] audioBytes = HttpsUtil.SendGet(url);
            CACHE[url] = audioBytes;
            return audioBytes;
        }

    }

}
