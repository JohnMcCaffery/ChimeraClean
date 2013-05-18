using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxWMPLib;
using System.Windows.Forms;

namespace Chimera.Multimedia {
    public static class VideoManager {
        private static AxWMPLib.AxWindowsMediaPlayer sVideoPlayer;

        public static AxWindowsMediaPlayer Player {
            get {
                if (sVideoPlayer == null)  {
                    sVideoPlayer = new AxWindowsMediaPlayer();
                    sVideoPlayer.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(videoPlayer_PlayStateChange);
                }

                return sVideoPlayer;            }        }

        private static void videoPlayer_PlayStateChange(object source, _WMPOCXEvents_PlayStateChangeEvent args) {
            if (args.newState == 1 && VideoFinished != null) {
                sVideoPlayer.Visible = false;
                VideoFinished();
            }
        }

        /// <summary>
        /// Triggered whenever a video that has been played through the interface finishes.
        /// </summary>
        public static event Action VideoFinished;
        

        public static void PlayVideo(string uri) {
            //videoPlayer.uiMode = "Mini";
            sVideoPlayer.Visible = true;
            sVideoPlayer.URL = uri;

            sVideoPlayer.uiMode = "none";
            sVideoPlayer.stretchToFit = true;
            sVideoPlayer.windowlessVideo = true;
            sVideoPlayer.Ctlcontrols.play();
        }

        public static void PlayAudio(string uri) {
            sVideoPlayer.URL = uri;
        }

        internal static void StopPlayback() {
            sVideoPlayer.Ctlcontrols.stop();
        }
    }
}
