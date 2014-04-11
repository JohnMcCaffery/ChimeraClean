using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Chimera.Interfaces {
    public interface IMediaPlayer {
        Control Player {
            get;
        }

        /// <summary>
        /// Triggered whenever a video that has been played through the interface finishes.
        /// </summary>
        event Action PlaybackFinished;

        event Action PlaybackStarted;

        void PlayVideo(string uri);

        void PlayAudio(string uri);

        void StopPlayback();
    }

    public class DummyPlayer : IMediaPlayer {
        private Panel mPanel;

        public Control Player {
            get {
                if (mPanel == null) {
                    mPanel = new Panel();
                    mPanel.Visible = true;

                    Label l = new Label();
                    l.Text = "Dummy Meda Player";
                    l.Visible = true;
                    l.Location = new Point(3, 3);

                    mPanel.Controls.Add(l);
                }
                return mPanel;
            }
        }

        public event Action PlaybackFinished;

        public event Action PlaybackStarted;

        public void PlayVideo(string uri) {
            if (PlaybackStarted != null)
                PlaybackStarted();
            if (PlaybackFinished != null)
                PlaybackFinished();
        }

        public void PlayAudio(string uri) {
            if (PlaybackStarted != null)
                PlaybackStarted();
            if (PlaybackFinished != null)
                PlaybackFinished();
        }

        public void StopPlayback() {
            if (PlaybackFinished != null)
                PlaybackFinished();
        }
    }

}
