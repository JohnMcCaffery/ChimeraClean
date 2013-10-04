using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxWMPLib;
using System.Windows.Forms;
using Chimera.Interfaces;
using System.Drawing;
using System.Threading;

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

                    sVideoPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(videoPlayer_PlayStateChange);
                }
                return sVideoPlayer;
            }
        }

        private static void videoPlayer_PlayStateChange(object source, _WMPOCXEvents_PlayStateChangeEvent args) {
            if (args.newState == 1 && sPlaybackFinished != null) {
                sVideoPlayer.Visible = false;
                sPlaybackFinished();
            }
        }

        /// <summary>
        /// Triggered whenever a video that has been played through the interface finishes.
        /// </summary>
        private static event Action sPlaybackFinished;
        

        private static void sPlayVideo(string uri) {
            Invoke(() => {
                //videoPlayer.uiMode = "Mini";
                sPlayer.Visible = true;
                sPlayer.URL = uri;

                sPlayer.uiMode = "none";
                sPlayer.stretchToFit = true;
                sPlayer.windowlessVideo = true;
                sPlayer.Ctlcontrols.play();
            });
        }

        private static void Invoke(Action a) {
            if (sPlayer.InvokeRequired)
                sPlayer.BeginInvoke(a);
            else
                a();
        }

        /// <summary>
        /// Note - before playing the control will have to be added to a form.
        /// TODO Fix this
        /// </summary>
        /// <param name="uri"></param>
        public static void sPlayAudio(string uri) {
            Invoke(() => {
                sPlayer.Bounds = new Rectangle(0, 0, 0, 0);
                sPlayer.Visible = true;
                sPlayer.URL = uri;
                sPlayer.Ctlcontrols.play();
            });
        }

        internal static void sStopPlayback() {
            Invoke(() => sPlayer.Ctlcontrols.stop());
        }


        public Control Player {
            get { return sPlayer; }
        }

        /// <summary>
        /// Triggered whenever a video that has been played through the interface finishes.
        /// </summary>
        public event Action PlaybackFinished {
            add { sPlaybackFinished += value; }
            remove { sPlaybackFinished -= value; }
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
