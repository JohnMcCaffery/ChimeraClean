using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxWMPLib;
using System.Windows.Forms;
using Chimera.Interfaces;

namespace Chimera.Multimedia {
    public class WMPMediaPlayer : IMediaPlayer {
        private static AxWindowsMediaPlayer sVideoPlayer;

        private static AxWindowsMediaPlayer sPlayer {
            get {
                if (sVideoPlayer == null)  {
                    sVideoPlayer = new AxWindowsMediaPlayer();

                    sVideoPlayer.Name = "videoPlayer";
                    sVideoPlayer.TabIndex = 1;
                    sVideoPlayer.Visible = true;

                    sVideoPlayer.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(videoPlayer_PlayStateChange);
                }
                return sVideoPlayer;
            }
        }

        private static void videoPlayer_PlayStateChange(object source, _WMPOCXEvents_PlayStateChangeEvent args) {
            if (args.newState == 1 && sVideoFinished != null) {
                sVideoPlayer.Visible = false;
                sVideoFinished();
            }
        }

        /// <summary>
        /// Triggered whenever a video that has been played through the interface finishes.
        /// </summary>
        private static event Action sVideoFinished;
        

        private static void sPlayVideo(string uri) {
            //videoPlayer.uiMode = "Mini";
            sPlayer.Visible = true;
            sPlayer.URL = uri;

            sPlayer.uiMode = "none";
            sPlayer.stretchToFit = true;
            sPlayer.windowlessVideo = true;
            sPlayer.Ctlcontrols.play();
        }

        public static void sPlayAudio(string uri) {
            sPlayer.URL = uri;
        }

        internal static void sStopPlayback() {
            sPlayer.Ctlcontrols.stop();
        }




        public Control Player {
            get { return sPlayer; }
        }

        /// <summary>
        /// Triggered whenever a video that has been played through the interface finishes.
        /// </summary>
        public event Action PlaybackFinished {
            add { sVideoFinished += value; }
            remove { sVideoFinished -= value; }
        }

        public void PlayVideo(string uri) {
            sPlayVideo(uri);
        }

        public void PlayAudio(string uri) {
            sPlayAudio(uri);
        }

        public void StopPlayback() {
            sStopPlayback();
        }
    }
}
