using Aequor.Util;
using CefSharp;
using CefSharp.Handler;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aequor.Audio
{

    internal class AudioRequestHandler : RequestHandler
    {

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            if (request.ResourceType == ResourceType.Media && request.Url.EndsWith(".aac"))
            {
                disableDefaultHandling = true;
                string url = request.Url;
                Task.Run(() => {
                    using MemoryStream memoryStream = new(AudioGetter.GetAudioBytes(url));
                    using StreamMediaFoundationReader reader = new(memoryStream);
                    using WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(reader);
                    using WaveOutEvent waveOutEvent = new();
                    waveOutEvent.Init(waveStream);
                    waveOutEvent.Play();
                    Thread.Sleep((int)reader.TotalTime.TotalMilliseconds);
                });
            }
            return base.GetResourceRequestHandler(chromiumWebBrowser, browser, frame, request, isNavigation, isDownload, requestInitiator, ref disableDefaultHandling);
        }

    }

}
